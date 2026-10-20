using System;

namespace TravelAgency.DAL.Abstractions.Entities
{
    /// <summary>Спільні поля всіх видів бронювань (тур, номер, квиток).</summary>
    public interface IBookingEntity : IEntity
    {
        int UserId { get; set; }

        BookingStatus Status { get; set; }

        decimal TotalPrice { get; set; }

        DateTime CreatedAt { get; set; }
    }
}
