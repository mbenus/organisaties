using Organisations.Models;
using Organisations.Repositories.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Organisations.Services
{
	public interface IOrganisationService
    {
		IList<Organisation> Get(PagingParameterModel pagingparametermodel);

		Organisation GetById(string id);
	}
}
