using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelAgency.BLL.Interfaces;
using TravelAgency.WebAPI.Mapping;
using TravelAgency.WebAPI.Models.Requests;
using TravelAgency.WebAPI.Models.Responses;
using TravelAgency.WebAPI.Security;

namespace TravelAgency.WebAPI.Controllers
{
    /// <summary>Довідник країн і регіонів: переглядати може будь-хто, додавати - персонал.</summary>
    [ApiController]
    [Route("api/countries")]
    public class CountriesController : ControllerBase
    {
        private readonly ICountryService _countries;

        public CountriesController(ICountryService countries)
        {
            _countries = countries;
        }

        /// <summary>GET api/countries - країни з регіонами.</summary>
        [HttpGet]
        public ActionResult<List<CountryResponse>> GetAll()
        {
            return _countries.GetCountries().ToResponses(c => c.ToResponse());
        }

        /// <summary>POST api/countries</summary>
        [HttpPost]
        [Authorize(Roles = ApiRoles.Staff)]
        public ActionResult<CountryResponse> Create([FromBody] NameRequest request)
        {
            CountryResponse country = _countries.CreateCountry(request.Name).ToResponse();
            return Created("/api/countries", country);
        }

        /// <summary>POST api/countries/1/regions</summary>
        [HttpPost("{countryId:int}/regions")]
        [Authorize(Roles = ApiRoles.Staff)]
        public ActionResult<RegionResponse> CreateRegion(int countryId, [FromBody] NameRequest request)
        {
            RegionResponse region = _countries.CreateRegion(countryId, request.Name).ToResponse();
            return Created("/api/countries", region);
        }
    }
}
