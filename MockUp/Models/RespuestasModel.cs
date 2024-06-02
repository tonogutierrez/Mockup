namespace MockUp.Models
{
    public class RespuestasModel
    {
        public string Correo { get; set; }
        public int IdPrueba { get; set; }
        public int IdPregunta { get; set; }
        public int IdOpcion { get; set; }
        public bool EsCorrecta { get; set; }
        public int Confianza {  get; set; }
    }
}
