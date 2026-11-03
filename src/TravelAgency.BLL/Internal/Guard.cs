using TravelAgency.BLL.Common;
using TravelAgency.BLL.Exceptions;

namespace TravelAgency.BLL.Internal
{
    /// <summary>Типові перевірки вхідних даних. Кидають ValidationFailedException з зрозумілим текстом.</summary>
    internal static class Guard
    {
        public static string RequiredText(string value, string fieldName, int maxLength)
        {
            string text = (value ?? string.Empty).Trim();
            if (text.Length == 0)
            {
                throw new ValidationFailedException("Поле \"" + fieldName + "\" обов'язкове.");
            }
            if (text.Length > maxLength)
            {
                throw new ValidationFailedException("Поле \"" + fieldName + "\" не довше " + maxLength + " символів.");
            }
            return text;
        }

        public static void InRange(int value, int min, int max, string fieldName)
        {
            if (value < min || value > max)
            {
                throw new ValidationFailedException("Поле \"" + fieldName + "\" має бути від " + min + " до " + max + ".");
            }
        }

        public static void Positive(decimal value, string fieldName)
        {
            if (value <= 0)
            {
                throw new ValidationFailedException("Поле \"" + fieldName + "\" має бути більше нуля.");
            }
        }
    }
}
