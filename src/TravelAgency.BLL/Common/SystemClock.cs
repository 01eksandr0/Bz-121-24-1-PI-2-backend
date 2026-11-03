using System;

namespace TravelAgency.BLL.Common
{
    internal class SystemClock : IClock
    {
        public DateTime Now => DateTime.Now;

        public DateTime Today => DateTime.Today;
    }
}
