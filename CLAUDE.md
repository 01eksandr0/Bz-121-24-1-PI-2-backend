Контекст для Claude Code — бекенд курсової

Курсова робота «Архітектура та проектування ПЗ», варіант 2 «Туристична агенція». Виконавець: Крижановський Олександр Романович, група Бз-121-24-1-ПІ, 2026 (одноосібно). Фронтенд — окремий репозиторій Bz-121-24-1-PI-2-frontend (Vue 3).

Вимоги ТЗ, які не можна порушувати
Шари: PL (ASP.NET Core WebAPI) → BLL (бібліотека) → DAL; PL не містить бізнес-логіки і не звертається до даних.
MS SQL + EF Core Code First; доступ з BLL тільки через IRepository<T> + IUnitOfWork (DAL.Abstractions).
BLL посилається лише на DAL.Abstractions (не на EF). Сутності DAL не виходять за BLL — mapping на кожній межі.
DI — Autofac (модулі в TravelAgency.Infrastructure). WebAPI посилається на DAL.EF тільки транзитивно через Infrastructure.
Модульні тести — тільки BLL, xUnit, принцип AAA (коментарі // Arrange // Act // Assert), stub-и замість БД.
Code conventions: мінімальні модифікатори доступу (internal де можна), без «магічних чисел» (BusinessRules, ConfigurationConstants), файл на тип.
Принципи: SOLID, LoD, DRY, YAGNI, KISS. Нічого зайвого поза ТЗ.
Структура
src/TravelAgency.DAL.Abstractions — сутності, IRepository<T>, IUnitOfWork
src/TravelAgency.DAL.EF — TravelAgencyContext, Configurations/, EfRepository (generic), EfRepositoryProvider, EfUnitOfWork, DatabaseInitializer (EnsureCreated + DemoDataSeeder)
src/TravelAgency.BLL — 9 сервісів, Models/, Mapping/ (EntityMapper, EnumMapper), Exceptions/ (BusinessException → 400/401/403/404/409), Internal/, Common/ (IClock, Pricing, BusinessRules), Security/ (PBKDF2)
src/TravelAgency.Infrastructure — DataAccessModule, BusinessLogicModule (Autofac)
src/TravelAgency.WebAPI — контролери, Models/Requests|Responses, Mapping/ApiMapper, Security/ (JWT), Middleware/ExceptionHandlingMiddleware
tests/TravelAgency.BLL.Tests — 56 тестів, Fakes/ (InMemoryUnitOfWork, FixedClock, FakePasswordHasher, TestData)
postman/ — колекція з 36 запитів; docs/uml — PlantUML; docs/backlog.md; scripts/create-github-issues.sh
Запуск
docker compose up -d
dotnet run --project src/TravelAgency.WebAPI   # http://localhost:5080/swagger
dotnet test
Стан і що лишилося перевірити
Код написано без доступу до .NET SDK: BLL і тести перевірені через Mono (56/56), контролери — типізацією з заглушками. Першим ділом: dotnet build і dotnet test, виправити помилки збірки, якщо будуть (EF Core 8, Autofac 8.1, Autofac.Extensions.DependencyInjection 10, JwtBearer 8).
Прогнати Postman-колекцію на свіжій БД (docker compose down -v && docker compose up -d).
За бажанням: перейти з EnsureCreated на міграції (dotnet ef migrations add Initial --project src/TravelAgency.DAL.EF, у DatabaseInitializer — Migrate()).
Git: гілки feature/TA-<N>-... → merge --no-ff у main (GitHub Flow). Нові задачі — нові гілки з тим самим іменуванням.