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
    /// <summary>
    /// Тури: перегляд і пошук - для всіх (у тому числі незареєстрованих),
    /// додавання, редагування і видалення - тільки для менеджера та адміністратора.
    /// </summary>
    [ApiController]
    [Route("api/tours")]
    public class ToursController : ControllerBase
    {
        private readonly ITourService _tours;

        public ToursController(ITourService tours)
        {
            _tours = tours;
        }

        /// <summary>GET api/tours?countryId=1&amp;type=Beach&amp;hotOnly=true&amp;sortBy=PriceAscending</summary>
        [HttpGet]
        public ActionResult<List<TourResponse>> Search([FromQuery] TourSearchQuery query)
        {
            return _tours.Search(query.ToModel()).ToResponses(t => t.ToResponse());
        }

        /// <summary>GET api/tours/5</summary>
        [HttpGet("{id:int}")]
        public ActionResult<TourResponse> GetById(int id)
        {
            return _tours.GetById(id).ToResponse();
        }

        /// <summary>POST api/tours</summary>
        [HttpPost]
        [Authorize(Roles = ApiRoles.Staff)]
        public ActionResult<TourResponse> Create([FromBody] TourRequest request)
        {
            TourResponse tour = _tours.Create(request.ToModel()).ToResponse();
            return CreatedAtAction(nameof(GetById), new { id = tour.Id }, tour);
        }

        /// <summary>PUT api/tours/5</summary>
        [HttpPut("{id:int}")]
        [Authorize(Roles = ApiRoles.Staff)]
        public ActionResult<TourResponse> Update(int id, [FromBody] TourRequest request)
        {
            return _tours.Update(id, request.ToModel()).ToResponse();
        }

        /// <summary>DELETE api/tours/5</summary>
        [HttpDelete("{id:int}")]
        [Authorize(Roles = ApiRoles.Staff)]
        public IActionResult Delete(int id)
        {
            _tours.Delete(id);
            return NoContent();
        }
    }
}
