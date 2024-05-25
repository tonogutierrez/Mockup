using System.Collections.Generic;
using MockUp.Models;
namespace MockUp.ViewModel
{
    public class PreguntasExamenViewModel
    {
        public int IdTema { get; set; }
        public string NombreTema { get; set; }
        public List<Pregunta> Preguntas { get; set; }
    }
}
