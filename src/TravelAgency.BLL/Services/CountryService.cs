using System;
using System.Collections.Generic;
using System.Linq;
using TravelAgency.BLL.Common;
using TravelAgency.BLL.Exceptions;
using TravelAgency.BLL.Interfaces;
using TravelAgency.BLL.Internal;
using TravelAgency.BLL.Mapping;
using TravelAgency.BLL.Models.Countries;
using TravelAgency.DAL.Abstractions.Entities;
using TravelAgency.DAL.Abstractions.Interfaces;

namespace TravelAgency.BLL.Services
{
    /// <summary>Країни і регіони - довідник для турів і готелів.</summary>
    internal class CountryService : ICountryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CountryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        private IRepository<Country> Countries => _unitOfWork.Repository<Country>();

        private IRepository<Region> Regions => _unitOfWork.Repository<Region>();

        public IReadOnlyList<CountryModel> GetCountries()
        {
            ILookup<int, Region> regionsByCountry = Regions.GetAll().ToLookup(r => r.CountryId);
            return Countries.GetAll()
                .OrderBy(c => c.Name)
                .Select(c => c.ToModel(regionsByCountry[c.Id]))
                .ToList();
        }

        public CountryModel CreateCountry(string name)
        {
            string cleanName = Guard.RequiredText(name, "Назва країни", BusinessRules.MaxNameLength);
            if (Countries.GetAll().Any(c => SameName(c.Name, cleanName)))
            {
                throw new ConflictException("Країна \"" + cleanName + "\" вже існує.");
            }
            var country = new Country { Name = cleanName };
            Countries.Add(country);
            _unitOfWork.SaveChanges();
            return country.ToModel(new Region[0]);
        }

        public RegionModel CreateRegion(int countryId, string name)
        {
            string cleanName = Guard.RequiredText(name, "Назва регіону", BusinessRules.MaxNameLength);
            Country country = Countries.GetRequired(countryId, "Країну");
            if (Regions.FindAll(r => r.CountryId == countryId).Any(r => SameName(r.Name, cleanName)))
            {
                throw new ConflictException("Регіон \"" + cleanName + "\" у цій країні вже існує.");
            }
            var region = new Region { Name = cleanName, CountryId = countryId, Country = country };
            Regions.Add(region);
            _unitOfWork.SaveChanges();
            return region.ToModel();
        }

        private static bool SameName(string first, string second)
        {
            return string.Equals(first, second, StringComparison.CurrentCultureIgnoreCase);
        }
    }
}
