namespace TravelAgency.DAL.Abstractions.Entities
{
    /// <summary>Зареєстрований користувач (клієнт, менеджер або адміністратор).</summary>
    public class User : IEntity
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public UserRole Role { get; set; }
    }
}
