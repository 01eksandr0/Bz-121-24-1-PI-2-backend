namespace TravelAgency.BLL.Exceptions
{
    /// <summary>Операція суперечить поточному стану (немає місць, номер зайнятий, дублікат) - HTTP 409.</summary>
    public class ConflictException : BusinessException
    {
        public ConflictException(string message) : base(message)
        {
        }
    }
}
