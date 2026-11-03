namespace TravelAgency.DAL.Abstractions
{
    /// <summary>
    /// Максимальні довжини текстових полів - одне джерело для схеми БД (DAL.EF) і для перевірок у BLL.
    /// </summary>
    public static class EntityLimits
    {
        public const int NameLength = 100;
        public const int TitleLength = 150;
        public const int EmailLength = 200;
        public const int DescriptionLength = 4000;
    }
}
