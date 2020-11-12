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
    }
}