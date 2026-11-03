using System.Collections.Generic;
using TravelAgency.BLL.Models.Trips;

namespace TravelAgency.BLL.Interfaces
{
    public interface ITripService
    {
        IReadOnlyList<TripModel> Search(TripSearchModel filter);

        TripModel GetById(int id);

        TripModel Create(TripInputModel input);
    }
}
