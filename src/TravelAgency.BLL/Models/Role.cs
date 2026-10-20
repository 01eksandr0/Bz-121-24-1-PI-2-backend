namespace TravelAgency.BLL.Models
{
    /// <summary>Роль користувача на рівні BLL. Значення збігаються з DAL (перевіряється тестом).</summary>
    public enum Role
    {
        Customer = 1,
        Manager = 2,
        Admin = 3
    }
}
