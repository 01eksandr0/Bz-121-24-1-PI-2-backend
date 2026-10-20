# Беклог курсової роботи (GitHub Projects)

Дошка: колонки **Backlog → To Do → In Progress → In Review → Done**. Кожна картка — Issue з міткою шару, виконавцем і оцінкою (story points).

| ID | Репозиторій | Мітка | Задача | Виконавець | SP |
|---|---|---|---|---|---|
| TA-1 | backend | dal | DAL: сутності, IRepository<T>, IUnitOfWork | Крижановський О.Р. | 3 |
| TA-2 | backend | dal | DAL: EF Core Code First, конфігурації, generic-репозиторій, UoW, демо-дані | Крижановський О.Р. | 5 |
| TA-3 | backend | bll | BLL: моделі, винятки, mapping, хешування паролів | Крижановський О.Р. | 3 |
| TA-4 | backend | bll | BLL: підсистема турів (пошук, фільтри, сортування, CRUD), країни і регіони | Крижановський О.Р. | 5 |
| TA-5 | backend | bll | BLL: бронювання турів, номерів у готелях і квитків | Крижановський О.Р. | 8 |
| TA-6 | backend | bll | BLL: реєстрація, вхід, керування ролями | Крижановський О.Р. | 3 |
| TA-7 | backend | tests | Tests: модульні тести BLL (xUnit, AAA, stub-и) | Крижановський О.Р. | 5 |
| TA-8 | backend | infrastructure | Infrastructure: DI-модулі Autofac | Крижановський О.Р. | 2 |
| TA-9 | backend | pl | WebAPI: JWT, ролі, middleware винятків, моделі PL, mapping, Auth/Users | Крижановський О.Р. | 5 |
| TA-10 | backend | pl | WebAPI: контролери турів, країн, готелів, рейсів і бронювань | Крижановський О.Р. | 5 |
| TA-11 | backend | docs | Docs: Postman-колекція і README | Крижановський О.Р. | 2 |
| TA-12 | backend | docs | Docs: UML-діаграми і пояснювальна записка | Крижановський О.Р. | 5 |
| TA-13 | frontend | ui | UI: HTTP-клієнт, стан входу, маршрути з ролями, вхід/реєстрація | Крижановський О.Р. | 3 |
| TA-14 | frontend | ui | UI: головна, пошук турів з фільтрами, сторінка туру з бронюванням | Крижановський О.Р. | 5 |
| TA-15 | frontend | ui | UI: готелі, вільні номери, рейси і квитки | Крижановський О.Р. | 5 |
| TA-16 | frontend | ui | UI: мої бронювання зі скасуванням | Крижановський О.Р. | 2 |
| TA-17 | frontend | ui | UI: керування турами, усі бронювання, довідники, ролі | Крижановський О.Р. | 5 |
| TA-18 | backend | ci | CI: GitHub Actions (build + test, build фронтенду) | Крижановський О.Р. | 1 |

Гілка для задачі: `feature/TA-<ID>-<коротко>` (наприклад, `feature/TA-5-bookings`). Після злиття PR картка автоматично переходить у Done (у PR вказати `Closes #<номер issue>`).

Створити мітки, задачі і дошку автоматично: `scripts/create-github-issues.sh` (потрібен GitHub CLI `gh`, виконаний `gh auth login`).
