using Microsoft.EntityFrameworkCore;
using Organisations.Repositories.Models;

namespace Organisations.Repositories
{
	public class OrganisationContext : DbContext, IOrganisationContext
	{
		public OrganisationContext(DbContextOptions<OrganisationContext> options) : base(options)
		{
		}

		public DbSet<Organisation> Organisations { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Organisation>().ToTable("Organisations");
			base.OnModelCreating(modelBuilder);
		}
	}
}
