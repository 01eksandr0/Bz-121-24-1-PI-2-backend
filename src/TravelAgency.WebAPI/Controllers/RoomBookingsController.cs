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
    /// <summary>Бронювання номерів у готелях: створює і скасовує зареєстрований користувач, усі бронювання бачить персонал.</summary>
    [ApiController]
    [Route("api/room-bookings")]
    [Authorize]
    public class RoomBookingsController : ControllerBase
    {
        private readonly IRoomBookingService _bookings;

        public RoomBookingsController(IRoomBookingService bookings)
        {
            _bookings = bookings;
        }

        /// <summary>GET api/room-bookings/my - мої бронювання.</summary>
        [HttpGet("my")]
        public ActionResult<List<RoomBookingResponse>> GetMine()
        {
            return _bookings.GetByUser(User.GetUserId()).ToResponses(b => b.ToResponse());
        }

        /// <summary>GET api/room-bookings - усі бронювання (персонал).</summary>
        [HttpGet]
        [Authorize(Roles = ApiRoles.Staff)]
        public ActionResult<List<RoomBookingResponse>> GetAll()
        {
            return _bookings.GetAll().ToResponses(b => b.ToResponse());
        }

        /// <summary>POST api/room-bookings</summary>
        [HttpPost]
        public ActionResult<RoomBookingResponse> Book([FromBody] BookRoomRequest request)
        {
            RoomBookingResponse booking = _bookings.Book(User.GetUserId(), request.ToModel()).ToResponse();
            return Created("/api/room-bookings/my", booking);
        }

        /// <summary>POST api/room-bookings/5/cancel - скасування (своє - клієнт, будь-яке - персонал).</summary>
        [HttpPost("{id:int}/cancel")]
        public IActionResult Cancel(int id)
        {
            _bookings.Cancel(id, User.GetUserId());
            return NoContent();
        }
    }
}
