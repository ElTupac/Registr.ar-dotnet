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
        public string Administradores {get; set;}
        [NotMapped]
        public List<UserEssentials> Admins {get; set;} //No guardar datos en db aca
        public bool Premium {get; set;}
        
        public string FreeCheckName {get; set;}
        public string Adicionales {get; set;}

        public bool pauseLogs {get; set;}
        public bool pauseRegs {get; set;}
    }
}