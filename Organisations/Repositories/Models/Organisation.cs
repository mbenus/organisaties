using System;

namespace Organisations.Repositories.Models
{
	public class Organisation
	{
		/// <summary>
		/// Unofficial id (self made up)
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// Organisatienaam
		/// </summary>
		public string Naam { get; set; }

		/// <summary>
		/// E.g: BevoegdgezagCode
		/// </summary>
		public string Organisatieid { get; set; }

		public string KvkNummer { get; set; }

		public string VestigingsNummer { get; set; }

		public string Rsin { get; set; }

		public string Oin { get; set; }

		public string Organisatiecode { get; set; }

		public string CbsNummer { get; set; }
	}
}