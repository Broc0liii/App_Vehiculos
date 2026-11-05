using ControlVehicular.Models;
using Microsoft.EntityFrameworkCore;

namespace ControlVehicular.Data
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
		}

		public DbSet<CatMarca> CatMarcas { get; set; }
		public DbSet<Vehiculo> Vehiculos { get; set; }
		public DbSet<Responsable> Responsables { get; set; }
	}
}
