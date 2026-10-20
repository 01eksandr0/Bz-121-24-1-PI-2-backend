namespace TravelAgency.DAL.Abstractions.Entities
{
    /// <summary>Роль користувача. Незареєстрований користувач у базі не зберігається.</summary>
    public enum UserRole
    {
        Customer = 1,
        Manager = 2,
        Admin = 3
    }
}
