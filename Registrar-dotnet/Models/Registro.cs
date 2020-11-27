using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Registrar_dotnet.Models
{
    public class Registro
    {
        [Key][DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID {get; set;}
        [Required]
        public string Nombre {get; set;}
        [Required]
        public int CreadorID {get; set;}
        public bool Premium {get; set;}
        public string nombrePropiedad {get; set;}
        public string Adicionales {get; set;}

        public bool pauseLogs {get; set;}
        public bool pauseRegs {get; set;}
    }
}