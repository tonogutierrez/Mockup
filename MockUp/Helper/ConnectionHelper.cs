namespace MockUp.Helper
{
    public class ConnectionHelper
    {
        public static string GetConnectionString()
        {
            // Aquí puedes obtener la cadena de conexión desde cualquier fuente,
            // como variables de entorno, archivos de configuración, etc.
            // Por ejemplo, podrías obtenerla de una variable de entorno llamada "videoJuego":
            string connectionString = "Server=videojuegoserver.database.windows.net;Initial Catalog=videoJuegoDB;User ID=saVideoJuego;Password=Santos_2008;";


            return connectionString;
        }
    }
}
