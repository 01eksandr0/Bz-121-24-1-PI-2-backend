using System;

namespace TravelAgency.BLL.Common
{
    /// <summary>Джерело поточного часу. Абстракція потрібна, щоб у тестах задавати "сьогодні".</summary>
    public interface IClock
    {
        DateTime Now { get; }

        DateTime Today { get; }
    }
}
