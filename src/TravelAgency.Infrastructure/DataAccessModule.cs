using Autofac;
using TravelAgency.DAL.Abstractions.Interfaces;
using TravelAgency.DAL.EF;

namespace TravelAgency.Infrastructure
{
    /// <summary>
    /// Реєстрація рівня доступу до даних. Лише тут відомо, що IUnitOfWork - це EF + MS SQL.
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
            var options = DatabaseInitializer.CreateOptions(_connectionString);

            builder.RegisterInstance(options);
            // один контекст і одна UoW на HTTP-запит (lifetime scope запиту)
            builder.RegisterType<TravelAgencyContext>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<EfUnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();

            builder.RegisterBuildCallback(scope => DatabaseInitializer.EnsureCreated(options));
        }
    }
}
