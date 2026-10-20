using System;
using System.Linq;
using TravelAgency.BLL.Models;
using TravelAgency.DAL.Abstractions.Entities;
using Xunit;

namespace TravelAgency.BLL.Tests.Mapping
{
    /// <summary>Переліки BLL і DAL перетворюються за числовим значенням - назви і значення мають збігатися.</summary>
    public class EnumMappingTests
    {
        private static void AssertSameMembers(Type dalEnum, Type bllEnum)
        {
            var dal = Enum.GetValues(dalEnum).Cast<object>().Select(v => Convert.ToInt32(v) + ":" + v).ToArray();
            var bll = Enum.GetValues(bllEnum).Cast<object>().Select(v => Convert.ToInt32(v) + ":" + v).ToArray();
            Assert.Equal(dal, bll);
        }

        [Fact]
        public void UserRole_MatchesRole() => AssertSameMembers(typeof(UserRole), typeof(Role));

        [Fact]
        public void BookingStatus_MatchesBookingState() => AssertSameMembers(typeof(BookingStatus), typeof(BookingState));

        [Fact]
        public void TransportType_MatchesTransportKind() => AssertSameMembers(typeof(TransportType), typeof(TransportKind));

        [Fact]
        public void TourType_MatchesTourKind() => AssertSameMembers(typeof(TourType), typeof(TourKind));
    }
}
