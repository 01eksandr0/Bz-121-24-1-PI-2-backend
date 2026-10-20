# TravelAgency — бекенд (курсова робота, варіант 2 «Туристична агенція»)

Виконав: Крижановський Олександр Романович, група Бз-121-24-1-ПІ, 2026.

- Бекенд: https://github.com/01eksandr0/Bz-121-24-1-PI-2-backend
- Фронтенд: https://github.com/01eksandr0/Bz-121-24-1-PI-2-frontend

ASP.NET Core 8 WebAPI + багатошарова архітектура + EF Core (Code First, MS SQL) + Autofac + xUnit.
Фронтенд — окремий репозиторій [Bz-121-24-1-PI-2-frontend](https://github.com/01eksandr0/Bz-121-24-1-PI-2-frontend) (Vue 3).

## Структура

| Проєкт | Шар | Залежить від |
|---|---|---|
| `src/TravelAgency.DAL.Abstractions` | сутності, `IRepository<T>`, `IUnitOfWork` | — |
| `src/TravelAgency.DAL.EF` | EF Core Code First, MS SQL, generic-репозиторій, UoW, демо-дані | DAL.Abstractions |
| `src/TravelAgency.BLL` | бізнес-логіка (9 сервісів), моделі, mapping, винятки | DAL.Abstractions |
| `src/TravelAgency.Infrastructure` | DI-модулі Autofac | BLL, DAL.EF |
| `src/TravelAgency.WebAPI` | PL: контролери, моделі PL, JWT, обробка винятків | BLL (+ Infrastructure у Program.cs) |
| `tests/TravelAgency.BLL.Tests` | модульні тести BLL (xUnit, stub-и) | BLL |

## Запуск

Потрібні .NET 8 SDK і SQL Server (LocalDB встановлюється разом із Visual Studio).

    dotnet run --project src/TravelAgency.WebAPI

- API: http://localhost:5080/api/tours
- Swagger: http://localhost:5080/swagger (кнопка **Authorize** — вставити токен з `/api/auth/login`)

База `TravelAgencyDb` створюється автоматично при першому запуску і заповнюється демо-даними.
Рядок підключення — `appsettings.json` → `ConnectionStrings:TravelAgency`.

### Демо-користувачі

| Роль | Email | Пароль |
|---|---|---|
| Адміністратор | admin@travel.ua | Admin123! |
| Менеджер | manager@travel.ua | Manager123! |
| Клієнт | olena@mail.com | User123! |

## Тести

    dotnet test

## Postman

`postman/TravelAgency.postman_collection.json` — імпортувати і запустити Run collection.
Токени менеджера і клієнта зберігаються в змінних колекції автоматично.

## Міграції (за бажанням)

    dotnet tool install --global dotnet-ef
    dotnet ef migrations add Initial --project src/TravelAgency.DAL.EF

і в `DatabaseInitializer.EnsureCreated` замінити `EnsureCreated()` на `Migrate()`.

## Робота з Git

- `main` — стабільна гілка, прямі коміти заборонені (branch protection).
- Кожна задача з GitHub Projects — окрема гілка `feature/TA-<номер>-<коротко>` або `fix/TA-<номер>-<коротко>`.
- Злиття — тільки через Pull Request із самоперевіркою за чек-листом і зеленим `dotnet test` (GitHub Actions).
