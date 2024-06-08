using MockUp.Models;

namespace MockUp.Models
{
    public class Examen
    {
       public string DescPregunta { get; set; }
            public string DescRespuesta { get; set; }
            public int IdPregunta { get; set; }
            public int IdOpcion { get; set; }
            public bool EsCorrecta {  get; set; }
            public int? OpcionResp { get; set; }
            public int? Confianza { get; set; }
            public int? Calificacion { get; set; }

    }
}

