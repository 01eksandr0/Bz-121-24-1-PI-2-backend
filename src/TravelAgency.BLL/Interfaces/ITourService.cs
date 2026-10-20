using System.Collections.Generic;
using TravelAgency.BLL.Models.Tours;

namespace TravelAgency.BLL.Interfaces
{
    public interface ITourService
    {
        IReadOnlyList<TourModel> Search(TourFilterModel filter);

        TourModel GetById(int id);

        TourModel Create(TourInputModel input);

        TourModel Update(int id, TourInputModel input);

        void Delete(int id);
    }
}
