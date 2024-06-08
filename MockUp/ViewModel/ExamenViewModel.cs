using System.Collections.Generic;
using MockUp.Models;
namespace MockUp.ViewModel
{
    public class ExamenViewModel
    {
        public int IdTema { get; set; }

        public int? Calificacion { get; set; }

        public List<Examen> Examen { get; set; }
    }
}
