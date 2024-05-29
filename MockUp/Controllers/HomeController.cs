using Firebase.Auth;
using Microsoft.AspNetCore.Mvc;
using MockUp.Models;
using System.Diagnostics;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System.Data;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Reflection;
using NuGet.Protocol.Plugins;
using MockUp.Helper;


namespace FirebaseLoginAuth.Controllers
{
    public class HomeController : Controller
    {

		
		FirebaseAuthProvider auth;

        public HomeController()
        {
            auth = new FirebaseAuthProvider(
                            new FirebaseConfig("AIzaSyAIqtBcJRJbGyz6rh8frHJ38GPuhBqHsMo\r\n"));
        }


        public IActionResult Index()
        {
            var token = HttpContext.Session.GetString("_UserToken");

            if (token != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("SignIn");
            }
        }

        //metodo de registro
        public IActionResult Registration()
        {
            return View();
        }

        public IActionResult SignIn()
        {
            return View();
        }

        public IActionResult LogOut()
        {
            HttpContext.Session.Remove("_UserToken");
            return RedirectToAction("SignIn");
        }

        //This will validate and process the registration request submitted by the user

        //POST

        [HttpPost]
        //Utilizo async task porque espero devolver un await
        public async Task<IActionResult> Registration(LoginModel loginModel)
        {

			try
            {
                //create the user
                await auth.CreateUserWithEmailAndPasswordAsync(loginModel.Email, loginModel.Password);
                //log in the new user
                var fbAuthLink = await auth
                                .SignInWithEmailAndPasswordAsync(loginModel.Email, loginModel.Password);
                string token = fbAuthLink.FirebaseToken;
                //salvo el token en una variable de sesion 
                if (token != null)
                {
                    HttpContext.Session.SetString("_UserToken", token);
                    HttpContext.Session.SetString("_UserEmail", loginModel.Email); // Guardar el correo electrónico del usuario

                    using (var connection = new SqlConnection(ConnectionHelper.GetConnectionString()))
					{
						connection.Open();

						using (var command = new SqlCommand())
						{
							command.Connection = connection;
							command.CommandText = "dbo.registrarAlumnos";
							command.CommandType = System.Data.CommandType.StoredProcedure;
							command.Parameters.Add("@Nombre", SqlDbType.VarChar);
							command.Parameters.Add("@ApellidoPaterno", SqlDbType.VarChar);
							command.Parameters.Add("@ApellidoMaterno", SqlDbType.VarChar);
							command.Parameters.Add("@Correo", SqlDbType.VarChar);
							//Poniendo valores 
							command.Parameters["@Nombre"].Value = loginModel.Nombre;
							command.Parameters["@ApellidoPaterno"].Value = loginModel.ApellidoPaterno;
							command.Parameters["@ApellidoMaterno"].Value = loginModel.ApellidoMaterno;
							command.Parameters["@Correo"].Value = loginModel.Email;

                           //No espero Respuestas
                            command.ExecuteNonQuery();

                            
                        }

					}

					return RedirectToAction("Temas", "CuestionarioAlumnos");                 }
            }
            //si hay un error
            catch (FirebaseAuthException ex)
            {
                var firebaseEx = JsonConvert.DeserializeObject<FirebaseError>(ex.ResponseData); //convierto el Json a objeto

                string customError = firebaseEx.error.message;

                if (customError == "EMAIL_EXISTS") customError = "Ya existe el correo";

                if (customError == "MISSING_EMAIL") customError = "Debes proporcionar un email";

                if (customError == "INVALID_EMAIL") customError = "El correo no es correcto";

                if (customError == "WEAK_PASSWORD : Password should be at least 6 characters") customError = "La contraseña debe de tener 6 caracteres";

                ModelState.AddModelError(String.Empty, customError);
                return View(loginModel);
            }

            return View();

        }

        //POST SignIn
        [HttpPost]
        public async Task<IActionResult> SignIn(SignInModel signInModel)
        {
            string tipo = "";
            if (!ModelState.IsValid)
            {
                return View(signInModel);
            }

            try
            {
                //log in an existing user
                var fbAuthLink = await auth
                                .SignInWithEmailAndPasswordAsync(signInModel.Email, signInModel.Password);
                string token = fbAuthLink.FirebaseToken;
                //save the token to a session variable
                if (token != null)
                {
                    HttpContext.Session.SetString("_UserToken", token);
                    HttpContext.Session.SetString("_UserEmail", signInModel.Email); // Guardar el correo electrónico del usuario o profesor

                    //Base de datos
                    using (var connection = new SqlConnection(ConnectionHelper.GetConnectionString()))
					{
						connection.Open();
						using (var command = new SqlCommand())
						{
							command.Connection = connection;
							command.CommandText = "dbo.verificarUsuario";
							command.CommandType = System.Data.CommandType.StoredProcedure;
							command.Parameters.Add("@Correo", SqlDbType.VarChar);
							//Poniendo valores
							command.Parameters["@Correo"].Value = signInModel.Email;

                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while(reader.Read())
                                {

                                    tipo = reader["TipoUsuario"].ToString();
                                }
                            }

						}
						connection.Close();
					}
                    
                    if(tipo == "Maestro")
                    {
						return RedirectToAction("MisAlumnos", "MisAlumnos");
					}
                    else if(tipo == "Alumno")
                    {
						return RedirectToAction("Temas", "CuestionarioAlumnos");
					}
                 
					
                }

            }
            catch (FirebaseAuthException ex)
            {
                var firebaseEx = JsonConvert.DeserializeObject<FirebaseError>(ex.ResponseData);
                string customError = firebaseEx.error.message;

                if (customError == "INVALID_LOGIN_CREDENTIALS") customError = "El correo o contraseña son incorrectos";

                ModelState.AddModelError(String.Empty, customError);
                return View(signInModel);
            }

            return View();
        }

    }
}