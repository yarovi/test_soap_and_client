using Microsoft.EntityFrameworkCore;
using WS.Unit06.User.Data.Model;

namespace WS.Unit06.User.Data
{
	public class DataContext:DbContext
	{
		protected override void OnConfiguring(DbContextOptionsBuilder
optionsBuilder)
		{
            //optionsBuilder.UseSqlite("Data Source=data/database.db");
            var dbPath = Path.Combine(AppContext.BaseDirectory, "data", "database.sqlite");
            optionsBuilder.UseSqlite($"Data Source={dbPath}");
        }
		public DbSet<Users> Users { get; set; }
	}
}
