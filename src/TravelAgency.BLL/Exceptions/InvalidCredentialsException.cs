namespace TravelAgency.BLL.Exceptions
{
    /// <summary>Неправильний email або пароль (HTTP 401).</summary>
    public class InvalidCredentialsException : BusinessException
    {
        public InvalidCredentialsException() : base("Неправильний email або пароль.")
        {
        }
    }
}
