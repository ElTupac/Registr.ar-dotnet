using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Registrar_dotnet.Models
{
    public class User
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
        
        //Despues hay que asociarle todos los registros que arma
        //Seria una lista de registros
    }
}