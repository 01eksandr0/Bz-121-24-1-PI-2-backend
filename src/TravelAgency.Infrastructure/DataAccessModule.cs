using Autofac;
using TravelAgency.DAL.Abstractions.Interfaces;
using TravelAgency.DAL.EF;

namespace TravelAgency.Infrastructure
{
    /// <summary>
    /// Реєстрація рівня доступу до даних. Лише тут відомо, що IUnitOfWork - це EF + MS SQL.
    /// Модуль тільки реєструє залежності; створення бази - окремий явний крок (DatabaseSetup).
    /// </summary>
    public class DataAccessModule : Module
    {
        private readonly string _connectionString;

        public DataAccessModule(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(DatabaseInitializer.CreateOptions(_connectionString));
            // один контекст і одна UoW на HTTP-запит (lifetime scope запиту); контекст звільняє контейнер
            builder.RegisterType<TravelAgencyContext>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<EfUnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();
        }
    }
}
