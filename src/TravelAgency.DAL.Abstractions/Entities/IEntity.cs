namespace TravelAgency.DAL.Abstractions.Entities
{
    /// <summary>Спільний контракт усіх сутностей: цілочисельний ключ.</summary>
    public interface IEntity
    {
        int Id { get; set; }
    }
}
