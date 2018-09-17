using Microsoft.EntityFrameworkCore;
using Organisations.Repositories.Models;

namespace Organisations.Repositories
{
	public interface IOrganisationContext
	{
		DbSet<Organisation> Organisations { get; set; }

		/// <seealso cref="DbContext"/>
		/// <remarks>
		/// This methode is implemented by DbContext. Unfortunatly DbContext doesn't have an interface. We specify the methode here so we can use the methode while we have only an IDeloContext reference to DeloContext.
		/// </remarks>
		int SaveChanges();
	}
}