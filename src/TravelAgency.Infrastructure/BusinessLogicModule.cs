using Autofac;
using TravelAgency.BLL.Common;
using TravelAgency.BLL.Security;
using TravelAgency.BLL.Services;

namespace TravelAgency.Infrastructure
{
    /// <summary>
    /// Реєстрація BLL: усі класи *Service - за своїми інтерфейсами (новий сервіс реєструється автоматично),
    /// а також годинник і хешер паролів (без стану, тому один екземпляр на програму).
    /// </summary>
    public class BusinessLogicModule : Module
    {
        private const string ServiceSuffix = "Service";

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(TourService).Assembly)
                .Where(type => type.Name.EndsWith(ServiceSuffix))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterType<SystemClock>().As<IClock>().SingleInstance();
            builder.RegisterType<Pbkdf2PasswordHasher>().As<IPasswordHasher>().SingleInstance();
        }
    }
}
