namespace TravelAgency.BLL.Models.Users
{
    public class UserModel
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public Role Role { get; set; }
    }
}
