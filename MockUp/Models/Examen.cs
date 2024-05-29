namespace MockUp.Models
{
    public class Examen
    {
       public string DescPregunta { get; set; }
        public string DescRespuesta { get; set; }
        public int IdPregunta { get; set; }
        public int IdOpcion { get; set; }
        public bool EsCorrecta {  get; set; }

    }
}
