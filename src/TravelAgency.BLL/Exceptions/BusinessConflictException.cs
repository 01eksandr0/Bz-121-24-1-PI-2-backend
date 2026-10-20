namespace TravelAgency.BLL.Exceptions
{
    /// <summary>Операція суперечить поточному стану (немає місць, номер зайнятий, дублікат) - HTTP 409.</summary>
    public class BusinessConflictException : BusinessException
    {
        public BusinessConflictException(string message) : base(message)
        {
        }
    }
}
