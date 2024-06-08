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
            string userCorreo = HttpContext.Session.GetString("_UserCorreo"); // Recupero el correo de la sesión
            List<Models.Examen> listaExamen = new List<Models.Examen>(); //Inicio modelo del examen(preguntas y respuestas)
            int? calificacion = null;

            using(var connection = new SqlConnection(ConnectionHelper.GetConnectionString())) //Me conecto a la base de datos
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "dbo.ExamenAlumno";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@IdTema", idTema); //Parametro de sql 
                    command.Parameters.AddWithValue("@Correo", userCorreo);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            
                           calificacion = reader["Calificacion"] != DBNull.Value ? (int?)Convert.ToInt32(reader["Calificacion"]) : null;
                        }


                        reader.NextResult(); //lee otro select
                        while (reader.Read())
                        {
                            //Leo el select de storedProcedure
                            Models.Examen examen = new Models.Examen();
                            examen.DescPregunta = reader["DescPregunta"].ToString(); 
                            examen.DescRespuesta = reader["DescRespuesta"].ToString();
                            examen.IdPregunta = Convert.ToInt32(reader["IdPregunta"]);
                            examen.IdOpcion = Convert.ToInt32(reader["IdOpcion"]);
                            examen.EsCorrecta = reader.GetBoolean(reader.GetOrdinal("EsCorrecta"));
                            //Para que reciba DBNull 
                            examen.OpcionResp = reader["OpcionResp"] != DBNull.Value ? (int?)Convert.ToInt32(reader["OpcionResp"]) : null;
                            examen.Confianza = reader["Confianza"] != DBNull.Value ? (int?)Convert.ToInt32(reader["Confianza"]) : null;

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
                Calificacion = calificacion,
                Examen = listaExamen
            };

            return View("Examen",model);
        }

        [HttpPost]
        public IActionResult GuardarRespuestas([FromBody] List<RespuestasModel> respuestas)
        {
            string userCorreo = HttpContext.Session.GetString("_UserCorreo"); //Recupero el correo de la sesion

            if (string.IsNullOrEmpty(userCorreo))
            {
                //Si no encuentra un correo
                return RedirectToAction("Login", "Home");
            }
            using (var connection = new SqlConnection(ConnectionHelper.GetConnectionString()))
            {
                connection.Open();
                foreach (var respuesta in respuestas)
                {
                    using (var command = connection.CreateCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = "dbo.GuardarRespuestas";
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Correo", userCorreo);
                        command.Parameters.AddWithValue("@IdPrueba", respuesta.IdPrueba);
                        command.Parameters.AddWithValue("@IdPregunta", respuesta.IdPregunta);
                        command.Parameters.AddWithValue("@IdOpcion", respuesta.IdOpcion);
                        command.Parameters.AddWithValue("@EsCorrecta", respuesta.EsCorrecta);
                        command.Parameters.AddWithValue("@Confianza", respuesta.Confianza);
                        command.ExecuteNonQuery();
                    }
                }
                connection.Close();
            }
            return Ok();



        }
       
    }
}
