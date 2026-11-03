using System;
using System.Linq;
using Xunit;
using Entities = TravelAgency.DAL.Abstractions.Entities;

namespace TravelAgency.BLL.Tests.Mapping
{
    /// <summary>Однойменні переліки BLL і DAL перетворюються за числовим значенням - назви і значення мають збігатися.</summary>
    public class EnumMappingTests
    {
        [Fact]
        public void UserRole_SameInDalAndBll() => AssertSameMembers(typeof(Entities.UserRole), typeof(Models.UserRole));

        [Fact]
        public void BookingStatus_SameInDalAndBll() => AssertSameMembers(typeof(Entities.BookingStatus), typeof(Models.BookingStatus));

        [Fact]
        public void TransportType_SameInDalAndBll() => AssertSameMembers(typeof(Entities.TransportType), typeof(Models.TransportType));

        [Fact]
        public void TourType_SameInDalAndBll() => AssertSameMembers(typeof(Entities.TourType), typeof(Models.TourType));

        private static void AssertSameMembers(Type dalEnum, Type bllEnum)
        {
            // Arrange
            Func<Type, string[]> describe = type => Enum.GetValues(type).Cast<object>()
                .Select(value => Convert.ToInt32(value) + ":" + value)
                .ToArray();

            // Act
            string[] dal = describe(dalEnum);
            string[] bll = describe(bllEnum);

            // Assert
            Assert.Equal(dal, bll);
        }
    }
}
