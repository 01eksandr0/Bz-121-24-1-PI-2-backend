#!/usr/bin/env bash
# Створює мітки, задачі (issues) і дошку GitHub Projects для курсової роботи.
# Використання: ./scripts/create-github-issues.sh <owner> <backend-repo> <frontend-repo>
# Приклад:      ./scripts/create-github-issues.sh 01eksandr0 Bz-121-24-1-PI-2-backend Bz-121-24-1-PI-2-frontend
set -euo pipefail
OWNER="$1"; BACKEND="$OWNER/$2"; FRONTEND="$OWNER/$3"

for REPO in "$BACKEND" "$FRONTEND"; do
  for LABEL in dal bll tests infrastructure pl docs ui ci; do
    gh label create "$LABEL" --repo "$REPO" --force >/dev/null
  done
done

PROJECT=$(gh project create --owner "$OWNER" --title "TravelAgency (курсова, варіант 2)" --format json --jq .number)
echo "Створено дошку #$PROJECT"

add_issue() {
  local repo="$1" label="$2" title="$3" body="$4"
  local url
  url=$(gh issue create --repo "$repo" --label "$label" --title "$title" --body "$body")
  gh project item-add "$PROJECT" --owner "$OWNER" --url "$url" >/dev/null
  echo "$url"
}

add_issue "$BACKEND" "dal" "TA-1: DAL: сутності, IRepository<T>, IUnitOfWork" "Сутності всіх підсистем, generic-репозиторій і UoW без залежності від EF.\n\nВиконавець: Крижановський О.Р.. Оцінка: 3 SP. Гілка: feature/TA-1-..."
add_issue "$BACKEND" "dal" "TA-2: DAL: EF Core Code First, конфігурації, generic-репозиторій, UoW, демо-дані" "MS SQL, IEntityTypeConfiguration, EnsureCreated, DemoDataSeeder.\n\nВиконавець: Крижановський О.Р.. Оцінка: 5 SP. Гілка: feature/TA-2-..."
add_issue "$BACKEND" "bll" "TA-3: BLL: моделі, винятки, mapping, хешування паролів" "Моделі BLL, ієрархія BusinessException, EntityMapper, PBKDF2.\n\nВиконавець: Крижановський О.Р.. Оцінка: 3 SP. Гілка: feature/TA-3-..."
add_issue "$BACKEND" "bll" "TA-4: BLL: підсистема турів (пошук, фільтри, сортування, CRUD), країни і регіони" "TourService, GeographyService, гарячі тури, ціна зі знижкою, вільні місця.\n\nВиконавець: Крижановський О.Р.. Оцінка: 5 SP. Гілка: feature/TA-4-..."
add_issue "$BACKEND" "bll" "TA-5: BLL: бронювання турів, номерів у готелях і квитків" "TourBookingService, HotelService, RoomBookingService, TransportService, TicketBookingService, спільні правила скасування.\n\nВиконавець: Крижановський О.Р.. Оцінка: 8 SP. Гілка: feature/TA-5-..."
add_issue "$BACKEND" "bll" "TA-6: BLL: реєстрація, вхід, керування ролями" "AuthService, UserService.\n\nВиконавець: Крижановський О.Р.. Оцінка: 3 SP. Гілка: feature/TA-6-..."
add_issue "$BACKEND" "tests" "TA-7: Tests: модульні тести BLL (xUnit, AAA, stub-и)" "InMemoryUnitOfWork, FixedClock, FakePasswordHasher; усі методи TourService і TourBookingService.\n\nВиконавець: Крижановський О.Р.. Оцінка: 5 SP. Гілка: feature/TA-7-..."
add_issue "$BACKEND" "infrastructure" "TA-8: Infrastructure: DI-модулі Autofac" "DataAccessModule, BusinessLogicModule, час життя об'єктів.\n\nВиконавець: Крижановський О.Р.. Оцінка: 2 SP. Гілка: feature/TA-8-..."
add_issue "$BACKEND" "pl" "TA-9: WebAPI: JWT, ролі, middleware винятків, моделі PL, mapping, Auth/Users" "Program.cs, JwtTokenService, ExceptionHandlingMiddleware, ApiMapper.\n\nВиконавець: Крижановський О.Р.. Оцінка: 5 SP. Гілка: feature/TA-9-..."
add_issue "$BACKEND" "pl" "TA-10: WebAPI: контролери турів, країн, готелів, рейсів і бронювань" "GET/POST/PUT/DELETE, [Authorize(Roles)], CreatedAtAction.\n\nВиконавець: Крижановський О.Р.. Оцінка: 5 SP. Гілка: feature/TA-10-..."
add_issue "$BACKEND" "docs" "TA-11: Docs: Postman-колекція і README" "36 запитів з перевірками статус-кодів.\n\nВиконавець: Крижановський О.Р.. Оцінка: 2 SP. Гілка: feature/TA-11-..."
add_issue "$BACKEND" "docs" "TA-12: Docs: UML-діаграми і пояснювальна записка" "Діаграми всіх шарів, опис процесу розробки.\n\nВиконавець: Крижановський О.Р.. Оцінка: 5 SP. Гілка: feature/TA-12-..."
add_issue "$FRONTEND" "ui" "TA-13: UI: HTTP-клієнт, стан входу, маршрути з ролями, вхід/реєстрація" "axios + інтерцептори, Pinia, router guards.\n\nВиконавець: Крижановський О.Р.. Оцінка: 3 SP. Гілка: feature/TA-13-..."
add_issue "$FRONTEND" "ui" "TA-14: UI: головна, пошук турів з фільтрами, сторінка туру з бронюванням" "TourCard, PriceTag, CountryRegionSelect.\n\nВиконавець: Крижановський О.Р.. Оцінка: 5 SP. Гілка: feature/TA-14-..."
add_issue "$FRONTEND" "ui" "TA-15: UI: готелі, вільні номери, рейси і квитки" "HotelsView, HotelDetailsView, TransportView.\n\nВиконавець: Крижановський О.Р.. Оцінка: 5 SP. Гілка: feature/TA-15-..."
add_issue "$FRONTEND" "ui" "TA-16: UI: мої бронювання зі скасуванням" "BookingsPanel (спільний для клієнта і менеджера).\n\nВиконавець: Крижановський О.Р.. Оцінка: 2 SP. Гілка: feature/TA-16-..."
add_issue "$FRONTEND" "ui" "TA-17: UI: керування турами, усі бронювання, довідники, ролі" "Сторінки manager/ і admin/.\n\nВиконавець: Крижановський О.Р.. Оцінка: 5 SP. Гілка: feature/TA-17-..."
add_issue "$BACKEND" "ci" "TA-18: CI: GitHub Actions (build + test, build фронтенду)" "Перевірка кожного Pull Request.\n\nВиконавець: Крижановський О.Р.. Оцінка: 1 SP. Гілка: feature/TA-18-..."
