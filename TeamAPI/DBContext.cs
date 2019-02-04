using MySql.Data.Entity;
using System.Data.Entity;
using TeamAPI.Models;

namespace DBContext
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class TeamDBContext : DbContext
    {
        public DbSet<Team> Teams { get; set; }
        public TeamDBContext() : base("Default") { }
    }
}