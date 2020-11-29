using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Registrar_dotnet.Models
{
    public class UserEssentials
    {
        [Key]
        public int ID {get; set;}
        [Required]
        public string Username {get; set;}
        [Required]
        public string Mail {get; set;}
    }
}