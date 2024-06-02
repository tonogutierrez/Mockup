using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using MockUp.Helper;
namespace MockUp.Controllers
{
    public class CuestionarioController : Controller
    {
        
        [Route("/Cuestionario")]
        public IActionResult Cuestionario()
        {
            string userCorreo = HttpContext.Session.GetString("_UserCorreo"); // Recupero el correo de la sesión
            string userType = "Maestro"; // Tipo de usuario alumno
            List<Models.TemasModel> listaDeTemas = new List<Models.TemasModel>(); //estoy creando una lista de los temas 
            //conexion con la base de datos 
            using(var connection = new SqlConnection(ConnectionHelper.GetConnectionString()))
            {
                connection.Open();

                //utilizo la clase sqlcommand para hacer un procedimiento 
                using(var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "dbo.mostrarTemas"; //obtengo el procedimiento con la funcion commandText
                    command.CommandType = System.Data.CommandType.StoredProcedure; //quiero que el commandtext sea texto
                    command.Parameters.AddWithValue("@Correo", userCorreo);
                    command.Parameters.AddWithValue("@UserType", userType);
                    using (SqlDataReader reader = command.ExecuteReader()) //leo los datos
                    {
                        while (reader.Read())
                        {
                            Models.TemasModel temas = new Models.TemasModel();
                            temas.IdTema = Convert.ToInt32(reader["IdTema"]);
                            temas.Tema = reader["Tema"].ToString();//leo la columa de mi tabla 
                            temas.LinkImagen = reader["LinkImagen"].ToString();
                            listaDeTemas.Add(temas);//se guarda en la lista
                        }
                    }
                }
                connection.Close();
            }
            return View(listaDeTemas);
        }
    }
}
