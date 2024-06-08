using Microsoft.AspNetCore.Mvc;
using MockUp.Models;
using MockUp.ViewModel;
using MockUp.Helper;
using System.Data;
using System.Data.SqlClient;

namespace MockUp.Controllers
{
    public class ObservarRespuestasController : Controller
    {
        

        [HttpGet]
        [Route("ObservarRespuestas/{idPrueba}/{idPregunta}")]
        public IActionResult ObservarRespuestas(int idPregunta, int idPrueba)
        {

            Models.Pregunta pregunta = new Models.Pregunta();
            List<Models.OpcionRespuestaModel>opcionRespuesta = new List<Models.OpcionRespuestaModel>();
            
            using (var connection = new SqlConnection(ConnectionHelper.GetConnectionString()))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "dbo.GetPreguntaYRespuestas";//storedprocedured para conseguir la pregunta por el id
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@IdPregunta", idPregunta); // cambiar esto
                    command.Parameters.AddWithValue("@IdPrueba", idPrueba);


                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            pregunta.DescPregunta = reader["DescPregunta"].ToString();
                            pregunta.IdPregunta = Convert.ToInt32(reader["IdPregunta"]);
                            pregunta.IdPrueba = Convert.ToInt32(reader["IdPrueba"]);
                            pregunta.IdTema = Convert.ToInt32(reader["IdTema"]);
                            pregunta.NombreTema = reader["Tema"].ToString();
                        }

                        reader.NextResult(); //lee otro select
                        while (reader.Read())
                        {
                            Models.OpcionRespuestaModel opcionesRespuesta = new Models.OpcionRespuestaModel();
                            opcionesRespuesta.TextoOpcion = reader["DescRespuesta"].ToString();
                            opcionesRespuesta.EsCorrecta = (bool)reader["EsCorrecta"];
                            opcionesRespuesta.IdOpcion = Convert.ToInt32(reader["IdOpcion"]);
                            opcionRespuesta.Add(opcionesRespuesta);
                        }
                        
                    }

                }
                connection.Close();
            }

            var model = new Pregunta
            {
                IdPregunta = idPregunta,
                IdPrueba = idPrueba,
                DescPregunta = pregunta.DescPregunta,
                Respuestas = opcionRespuesta,
                IdTema = pregunta.IdTema,
                NombreTema = pregunta.NombreTema
            };


            return View(model);

            
        }

        [HttpPost]
        public IActionResult ModificarPreguntasYRespuestas(int idPregunta, int idPrueba, string descPregunta,string respuesta1, string respuesta2, string respuesta3, string respuesta4, byte respuestaCorrecta1, byte respuestaCorrecta2, byte respuestaCorrecta3, byte respuestaCorrecta4)
        {

            Models.Pregunta pregunta = new Models.Pregunta();
            List<Models.OpcionRespuestaModel> opcionRespuesta = new List<Models.OpcionRespuestaModel>();
            using (var connection = new SqlConnection(ConnectionHelper.GetConnectionString()))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "dbo.ModificarPreguntasYRespuestas";//storedprocedured para modificar las preguntas y respuestas
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    //Parametros necesarios para el stored procedure 
                    command.Parameters.AddWithValue("@IdPrueba", idPrueba);
                    command.Parameters.AddWithValue("@IdPregunta", idPregunta);
                    command.Parameters.AddWithValue("@DescPregunta", descPregunta);
                    command.Parameters.AddWithValue("@DescRespuesta1", respuesta1);
                    command.Parameters.AddWithValue("@DescRespuesta2", respuesta2);
                    command.Parameters.AddWithValue("@DescRespuesta3", respuesta3);
                    command.Parameters.AddWithValue("@DescRespuesta4", respuesta4);
                    command.Parameters.AddWithValue("@EsCorrecta1", respuestaCorrecta1);
                    command.Parameters.AddWithValue("@EsCorrecta2", respuestaCorrecta2);
                    command.Parameters.AddWithValue("@EsCorrecta3", respuestaCorrecta3);
                    command.Parameters.AddWithValue("@EsCorrecta4", respuestaCorrecta4);

                    //comando para obtener las filas afectadas
                    int rowsAffected = command.ExecuteNonQuery();

                }
                connection.Close();
            }
            var model = new Pregunta
            {
                IdPregunta = idPregunta,
                DescPregunta = pregunta.DescPregunta,
                Respuestas = opcionRespuesta
            };
            return RedirectToAction("ObservarRespuestas", new { idPregunta = idPregunta, idPrueba = idPrueba });
        }


    }
}
