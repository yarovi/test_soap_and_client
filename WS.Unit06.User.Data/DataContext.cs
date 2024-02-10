using Microsoft.EntityFrameworkCore;
using WS.Unit06.User.Data.Model;

namespace WS.Unit06.User.Data
{
	public class DataContext:DbContext
	{
		protected override void OnConfiguring(DbContextOptionsBuilder
optionsBuilder)
		{
			optionsBuilder.UseSqlite("Data Source=data/database.db");
		}
		public DbSet<Users> Users { get; set; }
	}
}
