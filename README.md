# ECommerce
REST API для интернет-магазина, построенный на основе Clean Architecture, CQRS и Domain-Driven Design.
Находится в стадии разработки.

## Технологии

- .NET 10
- Entity Framework Core 10
- PostgreSQL
- MediatR (CQRS)
- FluentValidation
- Ardalis.Specification
- AutoMapper
- xUnit, Moq, FluentAssertions

## Архитектура

Проект разделен на четыре слоя:

### Domain
Содержит бизнес-сущности, Value Objects, доменные события и спецификации.

Основные сущности:
- Product
- Category
- Cart / CartItem
- Order / OrderItem
- Payment
- Review
- Notification
- User

### Application
Содержит бизнес-логику приложения. Реализован через CQRS с использованием MediatR.

- Commands: CreateProduct, UpdateProduct, DeleteProduct, RestoreProduct, ChangePrice, ChangeQuantity, ChangeDescription
- Queries: GetProductById, GetAllProducts, GetProducts (с фильтрацией и пагинацией)
- Validation: FluentValidation для всех команд
- Pipeline Behaviors: ValidationBehavior для автоматической валидации
- Result Pattern: Единообразная обработка ошибок без исключений
- Domain Events: Публикация событий через MediatR

### Infrastructure
Реализует доступ к данным и внешние сервисы.

- Entity Framework Core / PostgreSQL
- Generic Repository с поддержкой Specification
- Unit of Work с транзакциями
- Миграции

### API
ASP.NET Core Web API с минимальной логикой.

## Установка и запуск

### Предварительные требования

- .NET 10 SDK
- PostgreSQL (локально или через Docker)

### Настройка базы данных

Обнови строку подключения в `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=ECommerce;Username=postgres;Password=your_password"
  }
}
```

### Применение миграций

```bash
dotnet ef database update --project ECommerce.Infrastructure --startup-project ECommerce.API
```

### Запуск приложения

```bash
cd ECommerce.API
dotnet run
```

Swagger будет доступен по адресу `https://localhost:5001/swagger`.

## Тестирование

### Запуск тестов

```bash
dotnet test
```

### Запуск с покрытием кода

```bash
dotnet test --collect:"XPlat Code Coverage"
```

### Покрытие кода

- Application Layer: ~80%

## Реализованные функции

### Products (Товары)
- Получение списка с пагинацией и фильтрацией
- Получение товара по ID
- Создание товара
- Обновление товара (полное)
- Удаление товара (soft delete)
- Восстановление товара
- Изменение цены
- Изменение количества на складе
- Изменение описания

### Domain Events
- ProductCreatedEvent
- ProductChangedPriceEvent
- Обработчики событий для логирования

## API Endpoints

### Products
- `GET /api/products` — список товаров с пагинацией и фильтрацией
- `GET /api/products/{id}` — получение товара по ID
- `POST /api/products` — создание товара (Admin)
- `PUT /api/products/{id}` — обновление товара (Admin)
- `DELETE /api/products/{id}` — удаление товара (Admin)
- `PATCH /api/products/{id}/price` — изменение цены (Admin)
- `PATCH /api/products/{id}/quantity` — изменение количества (Admin)
- `PATCH /api/products/{id}/description` — изменение описания (Admin)
- `POST /api/products/{id}/restore` — восстановление товара (Admin)


## Архитектурные решения

### Specification Pattern
Используется для инкапсуляции логики запросов. Спецификации живут в Domain слое и используются в разных запросах.

### Result Pattern
Вместо исключений для бизнес-ошибок используется Result Pattern, что делает код более предсказуемым и тестируемым.

### MediatR Pipeline Behaviors
- ValidationBehavior — автоматическая валидация всех команд и запросов
- Логирование (планируется)
- Транзакции (планируется)

### Unit of Work + Repository
- Generic Repository с поддержкой Specification
- Управление транзакциями через Unit of Work

### Текущее состояние
Проект находится в активной стадии разработки. Реализован базовый функционал:
- Полный CRUD для управления товарами (с soft delete)
- Аутентификация и авторизация (JWT, роли User/Admin)
- Доменные события (создание товара, изменение цены)
- Покрытие тестами Application слоя >80%

Планируемые улучшения
- Корзина пользователя
- Заказы с транзакционной целостностью
- Интеграция с платежной системой Stripe
- Асинхронная обработка уведомлений через RabbitMQ
- Кеширование запросов через Redis


