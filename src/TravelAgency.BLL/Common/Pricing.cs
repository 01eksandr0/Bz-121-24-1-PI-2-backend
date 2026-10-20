using System;

namespace TravelAgency.BLL.Common
{
    /// <summary>Розрахунок цін. Окремий клас, бо ціну зі знижкою потрібно і показувати, і списувати при бронюванні.</summary>
    public static class Pricing
    {
        private const int FullPercent = 100;
        private const int MoneyDecimals = 2;

        public static decimal ApplyDiscount(decimal price, int discountPercent)
        {
            return Math.Round(price * (FullPercent - discountPercent) / FullPercent, MoneyDecimals, MidpointRounding.AwayFromZero);
        }
    }
}
