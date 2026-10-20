namespace TravelAgency.BLL.Exceptions
{
    /// <summary>Користувач не має права на цю дію з цим об'єктом (HTTP 403).</summary>
    public class AccessDeniedException : BusinessException
    {
        public AccessDeniedException(string message) : base(message)
        {
        }
    }
}
