using Organisations.Repositories.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Organisations.Repositories
{
	public class DbInitializer
	{
		public static void Initialize(OrganisationContext context)
		{
			context.Database.EnsureCreated();

			if (context.Organisations.Any())
			{
				return;
			}

			var csvFile = "data/ketenorganisaties.csv";
			
			var headerRead = false;
			string line;

			// Read the file and split it line by line.  
			var file = new StreamReader(csvFile);
			while ((line = file.ReadLine()) != null)
			{
				if (!headerRead)
				{
					headerRead = true;
					continue;
				}

				var d = line.Split(";");
				AddNewOrganisation(context, d[1], d[2], d[3], d[4], d[5], d[6], d[7], d[8]);
			}

			context.SaveChanges();
		}

		private static void AddNewOrganisation(OrganisationContext context, string name, string organisatieid, string kvknummer, string vestigingsNummer, string rsin, string oin, string organisatiecode, string cbsNummer)
		{
			Organisation org = new Organisation
			{
				Id = Guid.NewGuid(),
				Naam = name,
				Organisatieid = organisatieid,
				KvkNummer = kvknummer,
				VestigingsNummer = vestigingsNummer,
				Rsin = rsin,
				Oin = oin,
				Organisatiecode = organisatiecode,
				CbsNummer = cbsNummer
			};

			context.Organisations.Add(org);
		}
	}
}
