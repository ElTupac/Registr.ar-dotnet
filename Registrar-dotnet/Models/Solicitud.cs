using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Registrar_dotnet.Models
{
    public class Solicitud
    {
        [Key][DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID {get; set;}
        [Required]
        public int From {get; set;}
        [Required]
        public string FromName {get; set;}
        [Required]
        public int To {get; set;}
        [Required]
        public string ToName {get; set;}
        [Required]
        public string Action {get; set;}
        public int RelatedReg {get; set;}
    }
}