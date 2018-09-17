using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Organisations.Models
{
	/// <summary>
	/// https://www.c-sharpcorner.com/article/how-to-do-paging-with-asp-net-web-api/
	/// </summary>
	public class PagingParameterModel
	{
		public string sortColumn { get; set; }

		public SortOrder sortOrder { get; set; }

		const int maxPageSize = 20;

		public int pageNumber { get; set; } = 1;

		private int _pageSize { get; set; } = 10;

		public int pageSize
		{

			get { return _pageSize; }
			set
			{
				_pageSize = (value > maxPageSize) ? maxPageSize : value;
			}
		}
	}
}
