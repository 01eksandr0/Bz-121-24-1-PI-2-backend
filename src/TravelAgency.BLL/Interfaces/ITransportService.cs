using System.Collections.Generic;
using TravelAgency.BLL.Models.Transport;

namespace TravelAgency.BLL.Interfaces
{
    public interface ITransportService
    {
        IReadOnlyList<TripModel> Search(TripFilterModel filter);

        TripModel GetById(int id);

        TripModel Create(TripInputModel input);
    }
}
