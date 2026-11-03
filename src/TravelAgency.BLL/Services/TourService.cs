using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using TravelAgency.BLL.Common;
using TravelAgency.BLL.Exceptions;
using TravelAgency.BLL.Interfaces;
using TravelAgency.BLL.Internal;
using TravelAgency.BLL.Mapping;
using TravelAgency.BLL.Models.Tours;
using TravelAgency.DAL.Abstractions.Entities;
using TravelAgency.DAL.Abstractions.Interfaces;

namespace TravelAgency.BLL.Services
{
    /// <summary>Підсистеми "додавання/редагування турів" і "виведення турів з фільтрами та пошуком".</summary>
    internal class TourService : ITourService
    {
        private const string EntityName = "Тур";

        private static readonly Expression<Func<Tour, object>>[] Details = { t => t.Region.Country };

        private readonly IUnitOfWork _unitOfWork;
        private readonly IClock _clock;

        public TourService(IUnitOfWork unitOfWork, IClock clock)
        {
            _unitOfWork = unitOfWork;
            _clock = clock;
        }

        private IRepository<Tour> Tours => _unitOfWork.Repository<Tour>();

        /// <summary>Пошук серед турів, які ще не почалися.</summary>
        public IReadOnlyList<TourModel> Search(TourSearchModel filter)
        {
            DateTime today = _clock.Today;
            // прапорці замість перевірок на null - так умову однаково виконає і EF (SQL), і будь-яке інше джерело
            bool anyCountry = !filter.CountryId.HasValue;
            int countryId = filter.CountryId ?? 0;
            bool anyRegion = !filter.RegionId.HasValue;
            int regionId = filter.RegionId ?? 0;
            bool anyType = !filter.Type.HasValue;
            TourType type = filter.Type.HasValue ? filter.Type.Value.ToEntity() : default(TourType);
            bool hotOnly = filter.HotOnly;
            DateTime dateFrom = filter.DateFrom.HasValue && filter.DateFrom.Value.Date > today ? filter.DateFrom.Value.Date : today;
            bool anyDateTo = !filter.DateTo.HasValue;
            DateTime dateTo = filter.DateTo.HasValue ? filter.DateTo.Value.Date : DateTime.MaxValue;

            IReadOnlyList<Tour> tours = Tours.FindAll(t =>
                    t.StartDate >= dateFrom
                    && (anyDateTo || t.StartDate <= dateTo)
                    && (anyCountry || t.Region.CountryId == countryId)
                    && (anyRegion || t.RegionId == regionId)
                    && (anyType || t.Type == type)
                    && (!hotOnly || t.DiscountPercent > 0),
                Details);

            IEnumerable<TourModel> result = ToModels(tours);

            // ціна зі знижкою і вільні місця обчислюються, тому ці фільтри - після mapping
            if (filter.MinPrice.HasValue)
            {
                result = result.Where(t => t.FinalPrice >= filter.MinPrice.Value);
            }
            if (filter.MaxPrice.HasValue)
            {
                result = result.Where(t => t.FinalPrice <= filter.MaxPrice.Value);
            }
            if (filter.OnlyAvailable)
            {
                result = result.Where(t => t.FreePlaces > 0);
            }
            string text = (filter.Text ?? string.Empty).Trim();
            if (text.Length > 0)
            {
                result = result.Where(t => ContainsText(t, text));
            }
            return Sort(result, filter.SortBy).ToList();
        }

        public TourModel GetById(int id)
        {
            Tour tour = Tours.GetRequired(id, EntityName, Details);
            return tour.ToModel(Occupancy.TourPlaces(_unitOfWork, new[] { id })[id]);
        }

        public TourModel Create(TourInputModel input)
        {
            Validate(input);
            if (input.StartDate.Date <= _clock.Today)
            {
                throw new ValidationFailedException("Дата початку туру має бути в майбутньому.");
            }

            var tour = new Tour();
            input.ApplyTo(tour);
            Tours.Add(tour);
            _unitOfWork.SaveChanges();
            return GetById(tour.Id);
        }

        public TourModel Update(int id, TourInputModel input)
        {
            Tour tour = Tours.GetRequired(id, EntityName);
            if (tour.StartDate.Date <= _clock.Today)
            {
                throw new ConflictException("Тур уже почався або завершився, його не можна змінювати.");
            }
            Validate(input);
            if (input.StartDate.Date != tour.StartDate.Date && input.StartDate.Date <= _clock.Today)
            {
                throw new ValidationFailedException("Нова дата початку туру має бути в майбутньому.");
            }
            int booked = Occupancy.TourPlaces(_unitOfWork, new[] { id })[id];
            if (input.Capacity < booked)
            {
                throw new ConflictException("Уже заброньовано " + booked + " місць - місткість не може бути меншою.");
            }

            input.ApplyTo(tour);
            Tours.Update(tour);
            _unitOfWork.SaveChanges();
            return GetById(id);
        }

        /// <summary>Тур з історією бронювань не видаляється, щоб не втратити дані клієнтів.</summary>
        public void Delete(int id)
        {
            Tour tour = Tours.GetRequired(id, EntityName);
            if (_unitOfWork.Repository<TourBooking>().Any(b => b.TourId == id))
            {
                throw new ConflictException("Тур має бронювання, тому його не можна видалити.");
            }
            Tours.Remove(tour);
            _unitOfWork.SaveChanges();
        }

        private void Validate(TourInputModel input)
        {
            Guard.RequiredText(input.Title, "Назва", BusinessRules.MaxTitleLength);
            if (input.Title.Trim().Length < BusinessRules.MinTitleLength)
            {
                throw new ValidationFailedException("Назва туру має містити щонайменше " + BusinessRules.MinTitleLength + " символів.");
            }
            if ((input.Description ?? string.Empty).Length > BusinessRules.MaxDescriptionLength)
            {
                throw new ValidationFailedException("Опис занадто довгий.");
            }
            if (!Enum.IsDefined(typeof(Models.TourType), input.Type))
            {
                throw new ValidationFailedException("Невідомий тип туру.");
            }
            if (input.EndDate.Date <= input.StartDate.Date)
            {
                throw new ValidationFailedException("Дата завершення має бути пізніше за дату початку.");
            }
            Guard.Positive(input.PricePerPerson, "Ціна");
            Guard.InRange(input.DiscountPercent, 0, BusinessRules.MaxDiscountPercent, "Знижка, %");
            Guard.InRange(input.Capacity, BusinessRules.MinQuantity, BusinessRules.MaxTourCapacity, "Кількість місць");
            _unitOfWork.Repository<Region>().GetRequired(input.RegionId, "Регіон");
        }

        private IEnumerable<TourModel> ToModels(IReadOnlyList<Tour> tours)
        {
            Dictionary<int, int> booked = Occupancy.TourPlaces(_unitOfWork, tours.Select(t => t.Id).ToList());
            return tours.Select(t => t.ToModel(booked[t.Id]));
        }

        private static bool ContainsText(TourModel tour, string text)
        {
            return new[] { tour.Title, tour.Description, tour.RegionName, tour.CountryName }
                .Any(value => value != null && value.IndexOf(text, StringComparison.CurrentCultureIgnoreCase) >= 0);
        }

        private static IEnumerable<TourModel> Sort(IEnumerable<TourModel> tours, Models.TourSortOrder sortBy)
        {
            switch (sortBy)
            {
                case Models.TourSortOrder.PriceAscending:
                    return tours.OrderBy(t => t.FinalPrice);
                case Models.TourSortOrder.PriceDescending:
                    return tours.OrderByDescending(t => t.FinalPrice);
                case Models.TourSortOrder.DiscountDescending:
                    return tours.OrderByDescending(t => t.DiscountPercent).ThenBy(t => t.StartDate);
                default:
                    return tours.OrderBy(t => t.StartDate).ThenBy(t => t.Title);
            }
        }
    }
}
