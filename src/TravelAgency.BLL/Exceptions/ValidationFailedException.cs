namespace TravelAgency.BLL.Exceptions
{
    /// <summary>Неприпустимі дані (HTTP 400).</summary>
    public class ValidationFailedException : BusinessException
    {
        public ValidationFailedException(string message) : base(message)
        {
        }
    }
}
