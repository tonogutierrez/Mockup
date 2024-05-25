using Microsoft.AspNetCore.Mvc;
using MockUp.Helper;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
namespace MockUp.Controllers
{
    public class MisAlumnosController : Controller
    {

		

        [Route("/MisAlumnos")]
        public IActionResult MisAlumnos()
        {
            List<Models.Class> listaDeAlumnos = new List<Models.Class>();
            using (var connection = new SqlConnection(ConnectionHelper.GetConnectionString()))
            {
                connection.Open();
                
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "dbo.mostrarAlumnos";
                    command.CommandType = System.Data.CommandType.Text;
                    using (SqlDataReader reader = command.ExecuteReader()) //Estoy leyendo los datos 
                    {
                        while (reader.Read())
                        {
                            Models.Class alumnos = new Models.Class();
                            alumnos.IdUsuario = Convert.ToInt32(reader["IdUsuario"]);
                            alumnos.Nombre = reader["Nombre"].ToString();
                            alumnos.ApellidoPaterno = reader["ApellidoPaterno"].ToString();
                            alumnos.ApellidoMaterno = reader["ApellidoMaterno"].ToString();
                            listaDeAlumnos.Add(alumnos);
                        }
                    }
                }
                connection.Close();
            }

            /*
                Models.Class c1 = new Models.Class();
                c1.IdUsuario = 1;
                c1.Nombre = "Nombre 1";
                c1.ApellidoMaterno = "Materno 1";
                c1.ApellidoPaterno = "Paterno 1";
                Models.Class c2 = new Models.Class();
                c2.IdUsuario = 2;
                c2.Nombre = "Nombre 2";
                c2.ApellidoMaterno = "Materno 2";
                c2.ApellidoPaterno = "Paterno 2";

                Models.Class[] clases = {c1,c2};
            */

                return View(listaDeAlumnos);
        }
    }
}
