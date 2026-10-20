namespace TravelAgency.BLL.Exceptions
{
    /// <summary>Об'єкт не знайдено (HTTP 404).</summary>
    public class EntityNotFoundException : BusinessException
    {
        public EntityNotFoundException(string entityName, int id)
            : base(entityName + " з Id = " + id + " не знайдено.")
        {
        }
    }
}
