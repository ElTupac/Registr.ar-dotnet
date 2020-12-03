using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Registrar_dotnet.Models
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options)
        :base(options)
        {}

        public DbSet<User> Users {get; set;}
        public DbSet<Registro> Registros {get; set;}
        public DbSet<UsuarioFinal> UsuariosFinales {get; set;}
        public DbSet<Solicitud> Solicitudes {get; set;}
    }
}