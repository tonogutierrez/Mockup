using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace MockUp.Models
{

    //Creamos las propiedades para el login
    public class LoginModel
    {
        // [Required]
        // [EmailAddress]
        public string? Email { get; set; }

        // [Required]
        public string? Password { get; set; }

        public string Nombre {  get; set; }
        public string ApellidoPaterno {  get; set; }
        public string ApellidoMaterno { get; set;}
    }
    //Los objetos de correo electrónico y contraseña creados anteriormente se utilizarán para manejar los datos
    //pasados ​​entre la autenticación de Firebase y la vista o interfaz de usuario que crearemos más adelante.
}
