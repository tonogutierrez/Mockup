namespace MockUp.Models
{
    public class Pregunta
    {
        public int IdPregunta {  get; set; }
        public string DescPregunta { get; set; }
        public int IdPrueba { get; set; }

        public List<OpcionRespuestaModel> Respuestas { get; set; }
    }
}
