namespace TravelAgency.DAL.EF.Configurations
{
    /// <summary>Спільні обмеження для колонок, щоб не повторювати "магічні числа".</summary>
    internal static class ConfigurationConstants
    {
        public const int NameLength = 100;
        public const int TitleLength = 150;
        public const int EmailLength = 200;
        public const int PasswordHashLength = 256;
        public const int DescriptionLength = 4000;
        public const int MoneyPrecision = 18;
        public const int MoneyScale = 2;
    }
}
