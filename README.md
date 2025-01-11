# Library Management API

## Overview

The **Library Management API** is a simple ASP.NET Core Web API for managing books and authors in a library system. This project is built with a focus on clean code, best practices, and extensibility. It supports features like retrieving, adding, updating, and deleting books and authors, along with CORS and Swagger documentation.

------

### Design Decisions

I started the project by creating the `Book` and `Author` models with proper validation and relationships, followed by the `SeedData` class and `LibraryContext` to manage the in-memory database. I configured `Program.cs` to set up services like Swagger, CORS, and database seeding. Next, I developed the `BooksController` and `AuthorsController` to handle CRUD operations. For the bonus features, I implemented a search endpoint for books and added pagination support. I also created a test project to verify controller functionality, and finally, I configured CORS to enable cross-origin access during development.

- **In-Memory Database**:
  Used for storing data to avoid external setup. It's simple to use and great for testing, though the data resets on every run.

- **Pagination and Search**:
  The `GET /api/books` endpoint includes pagination, and a `/api/books/search` endpoint allows filtering by title. These features improve performance and usability for larger datasets.

- **CORS Configuration**:
  Configured to allow all origins, methods, and headers. This makes development and testing easier but is not ideal for production security.

- **Swagger Integration**:
  Swagger is used for automatic API documentation. It simplifies testing and provides an easy way to explore the API endpoints.

- **Error Handling**

  I implemented error handling in both `BooksController` and `AuthorsController` to ensure invalid inputs and non-existent resources are handled gracefully. For example, I added checks for invalid IDs, pagination parameters, and duplicate entries, returning appropriate responses like `BadRequest`, `NotFound`, or `Conflict`. This improves the application's robustness and provides meaningful feedback to users.

## Prerequisites

- .NET 8 SDK

------

## Running the Application

### Clone the Repository

```bash
git clone https://github.com/yourusername/LibraryManagementAPI.git
cd LibraryManagementAPI
```

### Run the Application

1. Navigate to the API project folder:

   ```bash
   cd LibraryManagementAPI
   ```

2. Run the application:

   ```bash
   dotnet run
   ```

3. Open a browser and go to:

   ```
   https://localhost:5001/swagger
   ```

### Run Unit Tests

1. Navigate to the root directory:

   ```bash
   cd ..
   ```

2. Run the tests:

   ```bash
   dotnet test
   ```

------

## Project Structure

```
plaintextCopy codeLibraryManagementAPI
├── LibraryManagementAPI/               # Main API project
│   ├── Controllers/                    # API controllers
│   ├── Data/                           # DbContext and seed data
│   ├── Models/                         # Data models (Book, Author)
│   ├── Program.cs                      # Application entry point
│   ├── appsettings.json                # Application configuration
│   ├── .gitignore                      # Git ignore file
├── LibraryManagementAPI.Tests/         # Unit test project
│   ├── AuthorsControllerTests.cs       # Tests for AuthorsController
│   ├── .gitignore                      # Git ignore file
├── LibraryManagementSolution.sln       # Solution file
├── README.md                           # Instructions and documentation
```