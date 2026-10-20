using System;

namespace TravelAgency.BLL.Exceptions
{
    /// <summary>Базовий виняток бізнес-логіки: порушено правило предметної області.</summary>
    public abstract class BusinessException : Exception
    {
        protected BusinessException(string message) : base(message)
        {
        }
    }
}
