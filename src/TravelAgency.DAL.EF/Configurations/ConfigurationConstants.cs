using TravelAgency.DAL.Abstractions;

namespace TravelAgency.DAL.EF.Configurations
{
    /// <summary>
    /// Спільні обмеження для колонок, щоб не повторювати "магічні числа".
    /// Довжини текстів беруться з EntityLimits - ті самі значення перевіряє BLL.
    /// </summary>
    internal static class ConfigurationConstants
    {
        public const int NameLength = EntityLimits.NameLength;
        public const int TitleLength = EntityLimits.TitleLength;
        public const int EmailLength = EntityLimits.EmailLength;
        public const int DescriptionLength = EntityLimits.DescriptionLength;
        public const int PasswordHashLength = 256;
        public const int MoneyPrecision = 18;
        public const int MoneyScale = 2;
    }
}
