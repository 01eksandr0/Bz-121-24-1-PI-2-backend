namespace TravelAgency.BLL.Exceptions
{
    /// <summary>Неприпустимі дані (HTTP 400).</summary>
    public class BusinessValidationException : BusinessException
    {
        public BusinessValidationException(string message) : base(message)
        {
        }
    }
}
