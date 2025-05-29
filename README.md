# E-Commerce Payment Integration

## Overview

This project implements a backend service for an **e-commerce platform** that integrates with the **Balance Management** service to facilitate payment processing. Since the Balance Management service may experience occasional failures, this solution is designed with **robust error handling and resilience patterns** to ensure high reliability.

## Features

- 📌 **Product Listing**: Retrieves available products and their pricing.
- 🛍 **Order Creation**: Enables users to place orders and reserve funds.
- ✅ **Order Completion**: Finalizes payments and completes the order process.
- 🛠 **Error Handling**: Implements **retry mechanisms** and **circuit breakers** for Balance Management failures.
- 🔒 **Data Validation**: Ensures the integrity and security of all requests.
- 💾 **Database Persistence**: Supports **in-memory**.
- 📜 **API Documentation**: OpenAPI-based documentation for easy integration.

## Installation & Setup

### Prerequisites
- .NET 9+

### Steps

```sh
git clone https://github.com/aktsahmt/ECommerce.git
cd ECommerce
dotnet restore
dotnet run
```

## API Endpoints

### **Balances**
- **Get Balance by User ID**  
  ```http
  GET /api/Balances/GetBalanceByUserId/{userId}
  ```
  *Retrieves the balance information for a specific user.*

- **Get All Balances**  
  ```http
  GET /api/Balances/GetAllBalance
  ```
  *Returns all stored balance records.*

### **Orders**
- **Get Order by ID**  
  ```http
  GET /api/Orders/GetOrderById/{orderId}
  ```
  *Fetches details of a specific order.*

- **Get All Orders**  
  ```http
  GET /api/Orders/GetAllOrders
  ```
  *Lists all stored orders.*

- **Create Order**  
  ```http
  POST /api/Orders/CreateOrder
  ```
  *Creates a new order and reserves funds.*

- **Complete Order**  
  ```http
  POST /api/Orders/CompleteOrder
  ```
  *Finalizes an order and processes payment.*

- **Cancel Order**  
  ```http
  POST /api/Orders/CancelOrder
  ```
  *Cancels a pre-order.*

### **Products**
- **Get All Products**  
  ```http
  GET /api/Products
  ```
  *Returns available products with pricing information.*

## Error Handling Strategy

1. **Retry Mechanism**: Automatic retries for transient failures.
2. **Circuit Breaker**: Prevents excessive requests to failing services.(Not yet)
3. **Graceful Fallback**: Implements alternative workflows when Balance Management is unavailable.(Not yet)

## Test Strategy & Coverage

✔️ **Unit Tests**: Covers core services using mocks.  
✔️ **Integration Tests**: Validates end-to-end interactions with Balance Management.  
✔️ **Error Handling Scenarios**: Ensures resilience against network failures, payment issues, and timeouts.  
✔️ **Coverage Reports**: CI/CD integration for detailed test insights.(Not yet)

## Architecture

The project adheres to **Clean Architecture**, ensuring modularity, maintainability, and scalability.

```
├── src
│   ├── Domain
│   ├── Application
│   ├── Infrastructure
│   ├── API
├── tests
│   ├── UnitTests
│   ├── IntegrationTests(Not yet)
```

## Deployment (Optional)

Run the service inside a Docker container:

```sh
docker build -t ECommerce .
docker run -p 8080:80 Ecommerce
```

## Security Considerations

- ✅ **Input Validation**: Prevents invalid requests and data corruption.
- ✅ **Rate Limiting**: Mitigates abuse of API endpoints.
- ✅ **CORS** Configuration: Restricts API access to trusted origins while preventing unauthorized cross-origin requests.
- ✅ **Authentication**: Secures transactions and sensitive data.(Not yet)


## License

This project is **MIT Licensed**.

---

🚀 **Built for reliability, resilience, and seamless payment integration!**  
Let me know if you want further enhancements. 😎
