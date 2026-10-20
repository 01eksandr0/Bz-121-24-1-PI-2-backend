using System;
using System.Collections.Generic;
using System.Linq;
using TravelAgency.DAL.Abstractions.Entities;
using TravelAgency.DAL.Abstractions.Interfaces;

namespace TravelAgency.BLL.Tests.Fakes
{
    /// <summary>
    /// Stub для IUnitOfWork: дані живуть тільки в пам'яті конкретного тесту,
    /// тому тести ніяк не впливають на реальну базу. SaveCount дозволяє перевірити, чи зберігались зміни.
    /// </summary>
    internal class InMemoryUnitOfWork : IUnitOfWork
    {
        private readonly Dictionary<Type, object> _repositories = new Dictionary<Type, object>();

        public InMemoryUnitOfWork()
        {
            Users = Register<User>(u => { });
            Countries = Register<Country>(c => { });
            Regions = Register<Region>(r => r.Country = Countries.Raw(r.CountryId));
            Tours = Register<Tour>(t => t.Region = WithCountry(Regions.Raw(t.RegionId)));
            TourBookings = Register<TourBooking>(b => { b.Tour = Tours.Raw(b.TourId); b.User = Users.Raw(b.UserId); });
            Hotels = Register<Hotel>(h =>
            {
                h.Region = WithCountry(Regions.Raw(h.RegionId));
                h.Rooms = Rooms.RawAll().Where(r => r.HotelId == h.Id).ToList();
            });
            Rooms = Register<HotelRoom>(r => r.Hotel = Hotels.Raw(r.HotelId));
            RoomBookings = Register<RoomBooking>(b =>
            {
                b.Room = Rooms.Raw(b.RoomId);
                b.Room.Hotel = Hotels.Raw(b.Room.HotelId);
                b.User = Users.Raw(b.UserId);
            });
            Trips = Register<TransportTrip>(t => { });
            TicketBookings = Register<TicketBooking>(b => { b.Trip = Trips.Raw(b.TripId); b.User = Users.Raw(b.UserId); });
        }

        public InMemoryRepository<User> Users { get; }

        public InMemoryRepository<Country> Countries { get; }

        public InMemoryRepository<Region> Regions { get; }

        public InMemoryRepository<Tour> Tours { get; }

        public InMemoryRepository<TourBooking> TourBookings { get; }

        public InMemoryRepository<Hotel> Hotels { get; }

        public InMemoryRepository<HotelRoom> Rooms { get; }

        public InMemoryRepository<RoomBooking> RoomBookings { get; }

        public InMemoryRepository<TransportTrip> Trips { get; }

        public InMemoryRepository<TicketBooking> TicketBookings { get; }

        public int SaveCount { get; private set; }

        public IRepository<TEntity> Repository<TEntity>() where TEntity : class, IEntity
        {
            return (IRepository<TEntity>)_repositories[typeof(TEntity)];
        }

        public void SaveChanges()
        {
            SaveCount++;
        }

        public void Dispose()
        {
        }

        private Region WithCountry(Region region)
        {
            if (region != null)
            {
                region.Country = Countries.Raw(region.CountryId);
            }
            return region;
        }

        private InMemoryRepository<TEntity> Register<TEntity>(Action<TEntity> fillReferences) where TEntity : class, IEntity
        {
            var repository = new InMemoryRepository<TEntity>(fillReferences);
            _repositories.Add(typeof(TEntity), repository);
            return repository;
        }
    }
}
