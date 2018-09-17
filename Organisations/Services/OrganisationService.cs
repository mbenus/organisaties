using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Organisations.Models;
using Organisations.Repositories;
using Organisations.Repositories.Models;

namespace Organisations.Services
{

	public class OrganisationService : IOrganisationService
	{
		private readonly IOrganisationContext _context;

		public OrganisationService(IOrganisationContext context)
		{
			_context = context;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="pagingparametermodel"></param>
		/// <returns></returns>
		public IList<Organisation> Get(PagingParameterModel pagingparametermodel)
		{
			var query = _context.Organisations.AsQueryable();
			var sortColumn = string.IsNullOrEmpty(pagingparametermodel.sortColumn) ? "naam" : pagingparametermodel.sortColumn;
			switch (sortColumn.ToLower())
			{
				case "naam":
					query = pagingparametermodel.sortOrder == SortOrder.Ascending ? query.OrderBy(c => c.Naam) : query.OrderByDescending(c => c.Naam);
					break;
				case "kvknummer":
					query = pagingparametermodel.sortOrder == SortOrder.Ascending ? query.OrderBy(c => c.KvkNummer) : query.OrderByDescending(c => c.KvkNummer);
					break;
				case "vestigingsnummer":
					query = pagingparametermodel.sortOrder == SortOrder.Ascending ? query.OrderBy(c => c.VestigingsNummer) : query.OrderByDescending(c => c.VestigingsNummer);
					break;
				case "rsin":
					query = pagingparametermodel.sortOrder == SortOrder.Ascending ? query.OrderBy(c => c.Rsin) : query.OrderByDescending(c => c.Rsin);
					break;
				case "oin":
					query = pagingparametermodel.sortOrder == SortOrder.Ascending ? query.OrderBy(c => c.Oin) : query.OrderByDescending(c => c.Oin);
					break;
				case "organisatiecode":
					query = pagingparametermodel.sortOrder == SortOrder.Ascending ? query.OrderBy(c => c.Organisatiecode) : query.OrderByDescending(c => c.Organisatiecode);
					break;
				case "cbsnummer":
					query = pagingparametermodel.sortOrder == SortOrder.Ascending ? query.OrderBy(c => c.CbsNummer) : query.OrderByDescending(c => c.CbsNummer);
					break;
				default:
					break;
			}



			// Get's No of Rows Count   
			int count = query.Count();

			// Parameter is passed from Query string if it is null then it default Value will be pageNumber:1  
			int CurrentPage = pagingparametermodel.pageNumber;

			// Parameter is passed from Query string if it is null then it default Value will be pageSize:20  
			int PageSize = pagingparametermodel.pageSize;

			// Display TotalCount to Records to User  
			int TotalCount = count;

			// Calculating Totalpage by Dividing (No of Records / Pagesize)  
			int TotalPages = (int)Math.Ceiling(count / (double)PageSize);

			// Returns List of Customer after applying Paging   
			var items = query.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();

			// if CurrentPage is greater than 1 means it has previousPage  
			var previousPage = CurrentPage > 1 ? true : false;

			// if TotalPages is greater than CurrentPage means it has nextPage  
			var nextPage = CurrentPage < TotalPages ? true : false;

			return items;
		}

		public Organisation GetById(string id)
		{
			return _context.Organisations.FirstOrDefault(o => o.Organisatieid == id);
		}
	}
}
