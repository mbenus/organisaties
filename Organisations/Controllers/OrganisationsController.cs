using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Organisations.Models;
using Organisations.Repositories.Models;
using Organisations.Services;

namespace Organisations.Controllers
{
    [Route("api/[controller]")]
	public class OrganisationsController : Controller
	{
		private readonly IOrganisationService _organisationService;

		public OrganisationsController(IOrganisationService service)
		{
			_organisationService = service;
		}

        // GET api/organisations
        [HttpGet]
        public IEnumerable<Organisation> Get(PagingParameterModel pagingParams)
        {
			return _organisationService.Get(pagingParams);
        }

        // GET api/organisations/5
        [HttpGet("{id}")]
        public Organisation Get(string id)
        {
			return _organisationService.GetById(id);
        }
    }
}
