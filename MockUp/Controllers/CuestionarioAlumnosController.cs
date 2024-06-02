using Microsoft.AspNetCore.Mvc;
using MockUp.Models;
using System.Data.SqlClient;
using MockUp.Helper;
namespace MockUp.Controllers
{
    public class CuestionarioAlumnosController : Controller
    {
        [Route("/Temas")]
        public IActionResult Temas()
        {
            string userCorreo = HttpContext.Session.GetString("_UserCorreo"); // Recupero el correo de la sesión
            string userType = "Alumno"; // Tipo de usuario alumno
            List<TemasModel> listTemas = new List<TemasModel>(); //lista de temas para el alumno
            //conexcion a la base de datos
            using(var connection = new SqlConnection(ConnectionHelper.GetConnectionString()))
            {
                connection.Open();
                //Creo un procedimiento
                using(var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "dbo.mostrarTemas"; //Obtengo el nombre del procedimiento
                    command.CommandType = System.Data.CommandType.StoredProcedure;
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
                            temas.Calificacion = Convert.ToInt32(reader["Calificacion"]);
                            listTemas.Add(temas);//se guarda en la lista
                        }
                    }
                }
                connection.Close();
            }
            return View(listTemas);
        }
    }
}
