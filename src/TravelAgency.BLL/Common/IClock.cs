using System;

namespace TravelAgency.BLL.Common
{
    /// <summary>
    /// Джерело поточного часу. Абстракція потрібна, щоб у тестах задавати "сьогодні".
    /// Усі дати предметної області (тури, рейси, бронювання) - місцевий час агенції, тому Now, а не UtcNow.
    /// </summary>
    internal interface IClock
    {
        DateTime Now { get; }

        DateTime Today { get; }
    }
}
