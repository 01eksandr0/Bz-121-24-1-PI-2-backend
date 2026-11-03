using System;

namespace TravelAgency.DAL.Abstractions.Interfaces
{
    /// <summary>Транзакцію скасовано через конфлікт з паралельним запитом; операцію можна повторити.</summary>
    public class ConcurrencyConflictException : Exception
    {
        public ConcurrencyConflictException(Exception innerException)
            : base("Транзакцію скасовано через паралельний запит.", innerException)
        {
        }
    }
}
