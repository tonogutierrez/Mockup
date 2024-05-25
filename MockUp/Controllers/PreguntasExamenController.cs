using Microsoft.AspNetCore.Mvc;
using MockUp.Helper;
using MockUp.Models;
using MockUp.ViewModel;
using System.Data;
using System.Data.SqlClient;

namespace MockUp.Controllers
{
    public class PreguntasExamenController : Controller

    {
        [HttpGet]
        [Route("Preguntas/{idTema}/{nombreTema}")]
        public IActionResult PreguntasExamen(int idTema,string nombreTema)
        {
            //Lista para almacenar las preguntas asociadas con el tema
            List<Models.Pregunta> preguntas = new List<Models.Pregunta>();
            //Base de datos
            using(var connection = new SqlConnection(ConnectionHelper.GetConnectionString()))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "dbo.GetPreguntasByTemaId";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@IdTema", idTema);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Models.Pregunta pregunta = new Models.Pregunta();
                            pregunta.DescPregunta = reader["DescPregunta"].ToString();
                            pregunta.IdPregunta = Convert.ToInt32(reader["IdPregunta"]);
                            pregunta.IdPrueba = Convert.ToInt32(reader["IdPrueba"]);
                            preguntas.Add(pregunta);
                        }
                    }
                    
                }
                connection.Close();
            }
            var model = new PreguntasExamenViewModel
            {
                IdTema = idTema,
                NombreTema = nombreTema,
                Preguntas = preguntas
            };
            return View(model);
        }

        
    }
}
