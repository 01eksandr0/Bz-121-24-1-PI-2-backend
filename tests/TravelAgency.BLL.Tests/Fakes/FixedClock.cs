using System;
using TravelAgency.BLL.Common;

namespace TravelAgency.BLL.Tests.Fakes
{
    /// <summary>Stub годинника: "сьогодні" завжди 1 червня 2025, 10:00. Тести не залежать від реальної дати.</summary>
    internal class FixedClock : IClock
    {
        public static readonly DateTime FixedNow = new DateTime(2025, 6, 1, 10, 0, 0);

        public DateTime Now => FixedNow;

        public DateTime Today => FixedNow.Date;
    }
}
