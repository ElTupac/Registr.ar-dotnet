using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Registrar_dotnet.Models
{
    public class Cliente
    {
        [Key][DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID {get; set;}
        [Required]
        public string Mail {get; set;}
        [Required]
        public string UserName {get; set;}
        [Required]
        public string Password {get; set;}
        [Required]
        public bool EmailVerif {get; set;}

        public bool FreeCheck {get; set;}

        public string Adicionales {get; set;}
        [Required]
        public int RegistroID {get; set;}
    }
}