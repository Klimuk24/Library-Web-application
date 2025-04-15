# Library Web API

✅Backend-часть приложения для управления библиотекой. Реализованы CRUD-операции для книг и авторов.

✅Unit-tests реализованы (для модели Автора).

✅Пагинация с фильтрами реализована (только для модели Автора, как пример). 

❌Frontend-клиентская часть не реализована.

❌Policy-based авторизация не реализована.

❌Docker конфигурации нет.

## Технологии

- .NET 8.0
- Entity Framework Core 9.0.0+
- MS SQL Server
- Swagger (OpenAPI)
- xUnit (тестирование)

## Требования

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- MS SQL Server (локальный) или любую другую БД, но тогда измените путь
- EF 9.0.0+ (либо другой совместимый с .NET 8.0)
- IDE (Visual Studio 2022, Rider или VS Code)

## Архитектура/Подходы/Принципы

- Clean Architechture 
- Code First
- REST

## Запуск и работа с приложением

1. **Клонируйте репозиторий**:
   ```bash
   git clone https://github.com/ваш-репозиторий/library-api.git
   cd library-api

2. **Настройте базу данных**:

- Обновите строку подключения в appsettings.json при необходимости (если используете другой провайдер):
```bash
"ConnectionStrings": {
  "LibraryConnection": "Server=localhost\SQLEXPRESS;Database=LibraryDB;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;"
}
```

3. **Примените миграции**:

```bash
dotnet ef database update --project Library-Web-application
```
Или через меню в вашей IDE и убедитесь, что БД создалась и исправно работает.


![image](https://github.com/user-attachments/assets/10e7751c-2e60-48f8-9778-f3037674fc51)

4. **Запустите приложение**:

- Приложение будет доступно по адресу: https://localhost:5001

5. **Документация API**:

После запуска должна открытся страница документации
Swagger UI: https://localhost:5001/swagger


![swagger_ui](https://github.com/user-attachments/assets/adae48fa-708a-47a9-a2f2-8da6c3808aa5)

6. **Проверка http запросов**

Проверьте работу всех базовых http запросов через Swagger. Изменения должны корректно возврашатся http ответом и отображатся в БД


![image](https://github.com/user-attachments/assets/7c2278c7-e05b-4981-9837-ccac99890bf0)


## Тестирование

Для запуска тестов выполните:
```bash
dotnet test
```
или через команду Tests --> Run test в вашей IDE

## Основные endpoints

**Автор**:
- GET /api/authors - список всех авторов (с пагинацией и фильтром для firstName и LastName)

- POST /api/authors - добавить автора

- GET /api/authors/{id} - автор по id

- PUT /api/authors/{id} - обновить автора

- DELETE /api/authors/{id} - удаление автора

- GET /api/authors/{authorId}/books - Список книг автора



**Книги**:
- GET /api/books - список всех книг

- POST /api/books - добавить книгу

- GET /api/books/{id} - книга по id

- PUT /api/books/{id} - обновить книгу

- DELETE /api/books/{id} - удаление книги

- GET /api/books/isbn/{isbn} - книна по isbn

- POST /api/books/{id}/borrow - выдача книги со сроком её сдачи

- POST /api/books/{id}/return - возврат книги

- PUT /api/books/overdue - Список книг с истекшим сроком возврата

- POST /api/books/{id}/upload-image - загрузить изображение для книги

## Примеры JSON запросов
- При добавлении сущностей id указывать не обязательно (генерируются автоматически)
- Все required поля должны быть заполнены при необходимости
  
**Добавление автора**:
```bash
{
  "firstName": "Лев",
  "lastName": "Толстой",
  "country": "Россия",
  "birthDate": "1828-09-09"
}
```

**Добавление книги**:
```bash
{
    "id": 0,
    "isbn": "978-5-17-090823-5",
    "title": "Война и мир",
    "description": null,
    "imagePath": null,
    "genre": 1,
    "borrowedTime": null,
    "returnDueTime": null,
    "authorId": 1
  }
```
  
**Выдача книги**:
- В теле запроса указываем время истечения срока возврата взятой книги
```bash
"bookId": 1,
"2024-12-31T00:00:00"'
```

**Получение книг с истекшим сроком**:
- Вернет список книг, если те были взяты, а время срока возврата истекло


- **Получение всех авторов**:
- Без фильтра вернет всех авторов разделив данные на страницы
- В фильтр можно указать firstName, lastName, country автора.


**Загрузка изображения книги**:
- Введите id книги и добавьте путь к изображению. После отработки запроса изображение будет хранится внутри проекта в пути: wwwroot/images/books
