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
    /// <summary>Бронювання турів: створює і скасовує зареєстрований користувач, усі бронювання бачить персонал.</summary>
    [ApiController]
    [Route("api/tour-bookings")]
    [Authorize]
    public class TourBookingsController : ControllerBase
    {
        private readonly ITourBookingService _bookings;

        public TourBookingsController(ITourBookingService bookings)
        {
            _bookings = bookings;
        }

        /// <summary>GET api/tour-bookings/my - мої бронювання.</summary>
        [HttpGet("my")]
        public ActionResult<List<TourBookingResponse>> GetMine()
        {
            return _bookings.GetByUser(User.GetUserId()).ToResponses(b => b.ToResponse());
        }

        /// <summary>GET api/tour-bookings - усі бронювання (персонал).</summary>
        [HttpGet]
        [Authorize(Roles = ApiRoles.Staff)]
        public ActionResult<List<TourBookingResponse>> GetAll()
        {
            return _bookings.GetAll().ToResponses(b => b.ToResponse());
        }

        /// <summary>POST api/tour-bookings</summary>
        [HttpPost]
        public ActionResult<TourBookingResponse> Book([FromBody] BookTourRequest request)
        {
            TourBookingResponse booking = _bookings.Book(User.GetUserId(), request.ToModel()).ToResponse();
            return Created("/api/tour-bookings/my", booking);
        }

        /// <summary>POST api/tour-bookings/5/cancel - скасування (своє - клієнт, будь-яке - персонал).</summary>
        [HttpPost("{id:int}/cancel")]
        public IActionResult Cancel(int id)
        {
            _bookings.Cancel(id, User.GetUserId());
            return NoContent();
        }
    }
}
