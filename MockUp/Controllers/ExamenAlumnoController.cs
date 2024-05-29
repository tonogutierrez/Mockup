using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MockUp.Models;
using MockUp.ViewModel;
using System.Data.SqlClient;
using MockUp.Helper;

namespace MockUp.Controllers
{
    public class ExamenAlumnoController : Controller
    {

        [Route("/Examen/{idTema}")]
        public IActionResult Examen(int idTema)
        {
            List<Models.Examen> listaExamen = new List<Models.Examen>(); //Inicio modelo del examen(preguntas y respuestas)
            using(var connection = new SqlConnection(ConnectionHelper.GetConnectionString())) //Me conecto a la base de datos
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "dbo.ExamenAlumno";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@IdTema", idTema); //Parametro de sql 

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            //Leo el select de storedProcedure
                            Models.Examen examen = new Models.Examen();
                            examen.DescPregunta = reader["DescPregunta"].ToString(); 
                            examen.DescRespuesta = reader["DescRespuesta"].ToString();
                            examen.IdPregunta = Convert.ToInt32(reader["IdPregunta"]);
                            examen.IdOpcion = Convert.ToInt32(reader["IdOpcion"]);
                            examen.EsCorrecta = reader.GetBoolean(reader.GetOrdinal("EsCorrecta"));

                            //Guardo el objeto a una lista
                            listaExamen.Add(examen);
                        }
                    }
                }
                connection.Close();
            }
            //Necesita IdTema para saber que examen va a tener
            var model = new ExamenViewModel
            {
                IdTema = idTema,
                Examen = listaExamen
            };

            return View("Examen",model);
        }
    }
}
