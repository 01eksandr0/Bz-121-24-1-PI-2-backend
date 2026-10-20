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
    /// <summary>Рейси транспорту: пошук - для всіх, додавання - персонал.</summary>
    [ApiController]
    [Route("api/trips")]
    public class TripsController : ControllerBase
    {
        private readonly ITransportService _transport;

        public TripsController(ITransportService transport)
        {
            _transport = transport;
        }

        /// <summary>GET api/trips?type=Plane&amp;toCity=Анталія&amp;date=2025-07-01</summary>
        [HttpGet]
        public ActionResult<List<TripResponse>> Search([FromQuery] TripSearchQuery query)
        {
            return _transport.Search(query.ToModel()).ToResponses(t => t.ToResponse());
        }

        /// <summary>GET api/trips/5</summary>
        [HttpGet("{id:int}")]
        public ActionResult<TripResponse> GetById(int id)
        {
            return _transport.GetById(id).ToResponse();
        }

        /// <summary>POST api/trips</summary>
        [HttpPost]
        [Authorize(Roles = ApiRoles.Staff)]
        public ActionResult<TripResponse> Create([FromBody] TripRequest request)
        {
            TripResponse trip = _transport.Create(request.ToModel()).ToResponse();
            return CreatedAtAction(nameof(GetById), new { id = trip.Id }, trip);
        }
    }
}
