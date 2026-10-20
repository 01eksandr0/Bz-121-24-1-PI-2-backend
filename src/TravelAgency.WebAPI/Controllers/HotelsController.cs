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
    /// <summary>Готелі і номери: пошук і вільні номери - для всіх, додавання - персонал.</summary>
    [ApiController]
    [Route("api/hotels")]
    public class HotelsController : ControllerBase
    {
        private readonly IHotelService _hotels;

        public HotelsController(IHotelService hotels)
        {
            _hotels = hotels;
        }

        /// <summary>GET api/hotels?countryId=1&amp;minStars=4</summary>
        [HttpGet]
        public ActionResult<List<HotelResponse>> Search([FromQuery] HotelSearchQuery query)
        {
            return _hotels.Search(query.ToModel()).ToResponses(h => h.ToResponse());
        }

        /// <summary>GET api/hotels/5</summary>
        [HttpGet("{id:int}")]
        public ActionResult<HotelResponse> GetById(int id)
        {
            return _hotels.GetById(id).ToResponse();
        }

        /// <summary>GET api/hotels/5/available-rooms?checkIn=2025-07-01&amp;checkOut=2025-07-05&amp;guests=2</summary>
        [HttpGet("{id:int}/available-rooms")]
        public ActionResult<List<HotelRoomResponse>> GetAvailableRooms(int id, [FromQuery] AvailableRoomsQuery query)
        {
            return _hotels.GetAvailableRooms(id, query.CheckIn.Value, query.CheckOut.Value, query.Guests)
                .ToResponses(r => r.ToResponse());
        }

        /// <summary>POST api/hotels</summary>
        [HttpPost]
        [Authorize(Roles = ApiRoles.Staff)]
        public ActionResult<HotelResponse> Create([FromBody] HotelRequest request)
        {
            HotelResponse hotel = _hotels.Create(request.ToModel()).ToResponse();
            return CreatedAtAction(nameof(GetById), new { id = hotel.Id }, hotel);
        }

        /// <summary>POST api/hotels/5/rooms</summary>
        [HttpPost("{id:int}/rooms")]
        [Authorize(Roles = ApiRoles.Staff)]
        public ActionResult<HotelRoomResponse> AddRoom(int id, [FromBody] HotelRoomRequest request)
        {
            HotelRoomResponse room = _hotels.AddRoom(id, request.ToModel()).ToResponse();
            return CreatedAtAction(nameof(GetById), new { id }, room);
        }
    }
}
