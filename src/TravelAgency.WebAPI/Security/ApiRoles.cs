namespace TravelAgency.WebAPI.Security
{
    /// <summary>Назви ролей у токені (збігаються з назвами переліку Role у BLL).</summary>
    internal static class ApiRoles
    {
        public const string Customer = "Customer";
        public const string Manager = "Manager";
        public const string Admin = "Admin";

        /// <summary>Персонал агенції: менеджер і адміністратор.</summary>
        public const string Staff = Manager + "," + Admin;
    }
}
