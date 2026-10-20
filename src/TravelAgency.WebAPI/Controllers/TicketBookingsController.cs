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
    /// <summary>Бронювання квитків на транспорт: створює і скасовує зареєстрований користувач, усі бронювання бачить персонал.</summary>
    [ApiController]
    [Route("api/ticket-bookings")]
    [Authorize]
    public class TicketBookingsController : ControllerBase
    {
        private readonly ITicketBookingService _bookings;

        public TicketBookingsController(ITicketBookingService bookings)
        {
            _bookings = bookings;
        }

        /// <summary>GET api/ticket-bookings/my - мої бронювання.</summary>
        [HttpGet("my")]
        public ActionResult<List<TicketBookingResponse>> GetMine()
        {
            return _bookings.GetByUser(User.GetUserId()).ToResponses(b => b.ToResponse());
        }

        /// <summary>GET api/ticket-bookings - усі бронювання (персонал).</summary>
        [HttpGet]
        [Authorize(Roles = ApiRoles.Staff)]
        public ActionResult<List<TicketBookingResponse>> GetAll()
        {
            return _bookings.GetAll().ToResponses(b => b.ToResponse());
        }

        /// <summary>POST api/ticket-bookings</summary>
        [HttpPost]
        public ActionResult<TicketBookingResponse> Book([FromBody] BookTicketRequest request)
        {
            TicketBookingResponse booking = _bookings.Book(User.GetUserId(), request.ToModel()).ToResponse();
            return Created("/api/ticket-bookings/my", booking);
        }

        /// <summary>POST api/ticket-bookings/5/cancel - скасування (своє - клієнт, будь-яке - персонал).</summary>
        [HttpPost("{id:int}/cancel")]
        public IActionResult Cancel(int id)
        {
            _bookings.Cancel(id, User.GetUserId());
            return NoContent();
        }
    }
}
