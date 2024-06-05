using Microsoft.AspNetCore.Mvc;
using MockUp.Helper;
using System.Data.SqlClient;

namespace MockUp.Controllers
{
    public class PerformanceController : Controller
    {
        [Route("/Performance/{idUsuario}/{idPrueba}")]
        
        public IActionResult GetPerformance(string idUsuario, int idPrueba)
        {
            

            //Para guardar el performance
            Models.Grafica performance = new Models.Grafica();

            using (var connection = new SqlConnection(ConnectionHelper.GetConnectionString()))
            {
                connection.Open();
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "dbo.GraficarPerformance";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@idUsuario", idUsuario);
                    command.Parameters.AddWithValue("@IdPrueba", idPrueba);

                    using(SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Models.Grafica numPerformance = new Models.Grafica();
                            numPerformance.Performance = Convert.ToInt32(reader["Performance"]);
                            performance = numPerformance;
                            
                        }
                    }
                }
                connection.Close();
            }
            return View(performance);
        }
    }
}
