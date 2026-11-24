
# Restaurant Menu & Order Management API

A RESTful API for managing restaurant menu items, customer orders, and daily sales reports.

Built with C#, ASP.NET Core Minimal APIs, Entity Framework Core, and SQLite, this project demonstrates backend development, database management, API design, and error handling.

## 🚀 Tech Stack

- **Language:** C#
- **Framework:** ASP.NET Core Minimal APIs
- **ORM:** Entity Framework Core
- **Database:** SQLite
- **API Documentation:** OpenAPI & Scalar
- **Testing:** Postman
- **Development Tools:** Visual Studio, .NET CLI, Git & GitHub

## ✨ Features

### Menu Management
- Retrieve all menu items.
- Create new menu items.
- Retrieve individual menu items by ID.
- Update existing menu items.
- Delete menu items.

### Order Management
- Create customer orders.
- Retrieve all orders.
- Retrieve individual orders by ID.
- Calculate order totals.
- Manage order items and order status.

### Reporting
- Generate daily sales reports.
- Retrieve order and revenue information for reporting purposes.

### Additional Engineering Features
- Entity Framework Core database integration.
- Database migrations.
- Automatic database seeding.
- Data Transfer Objects (DTOs).
- Request validation.
- Centralized global exception handling.
- OpenAPI documentation.
- Interactive API exploration using Scalar.

## 🏗️ Architecture

The application follows a modular structure, separating API endpoints, models, data access, and supporting functionality.

Main application components:

- **Endpoints:** Handle incoming HTTP requests.
- **Models:** Represent menu items, orders, order items, and order statuses.
- **DTOs:** Define data transferred between clients and the API.
- **Database:** SQLite persistence managed through Entity Framework Core.
- **Exception Handling:** Centralized handling of application errors.
- **Database Seeder:** Initializes the database with sample data.

## 🔌 API Endpoints

### Menu

| Method | Endpoint | Description |
|---|---|---|
| GET | `/menu` | Retrieve all menu items |
| POST | `/menu` | Create a menu item |
| GET | `/menu/{id}` | Retrieve a menu item |
| PUT | `/menu/{id}` | Update a menu item |
| DELETE | `/menu/{id}` | Delete a menu item |

### Orders

| Method | Endpoint | Description |
|---|---|---|
| GET | `/orders` | Retrieve all orders |
| GET | `/orders/{id}` | Retrieve an order |
| POST | `/orders` | Create a new order |

### Reports

| Method | Endpoint | Description |
|---|---|---|
| GET | `/reports/daily` | Retrieve the daily sales report |

## 🗄️ Database

The application uses SQLite as its relational database.

Entity Framework Core manages database access and schema changes through migrations.

Main entities:

- **MenuItem:** Represents a restaurant menu item.
- **Order:** Represents a customer order.
- **OrderItem:** Represents individual items within an order.
- **OrderStatus:** Defines the status of an order.

Database migrations are used to manage schema changes, while a database seeder provides initial sample data.

## ⚙️ Getting Started

### Prerequisites

- .NET 10 SDK
- Visual Studio or Visual Studio Code
- Git

### 1. Clone the repository

```bash
git clone https://github.com/konstantinos32/RestaurantApi.git
```

### 2. Navigate to the project directory

```bash
cd RestaurantApi
```

### 3. Restore dependencies

```bash
dotnet restore
```

### 4. Apply database migrations

```bash
dotnet ef database update
```

Make sure the database connection is configured correctly before running migrations.

### 5. Run the application

```bash
dotnet run
```

The application will start locally. Check the terminal output for the actual address and port.

## 📖 API Documentation

The project uses OpenAPI and Scalar to provide interactive API documentation.

Once the application is running, open the Scalar endpoint configured by the application to explore and test the available API operations.

## 🧪 Testing

API endpoints were tested using:

- Postman
- Scalar interactive API documentation

Testing covered menu management, order creation and retrieval, and daily reporting.

## 🎯 Project Goals

This project was developed to strengthen practical software engineering skills in:

- RESTful API design.
- Backend application development.
- Relational database modeling.
- Entity Framework Core and database migrations.
- DTOs and request validation.
- Error handling and application structure.
- API documentation and testing.

## 👨‍💻 Author

**Konstantinos Theodoropoulos**

- GitHub: [@konstantinos32](https://github.com/konstantinos32)
- Repository: [RestaurantApi](https://github.com/konstantinos32/RestaurantApi)
