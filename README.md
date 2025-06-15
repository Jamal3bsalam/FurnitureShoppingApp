# 🛒 Furniture Shopping App - E-commerce API

This is a backend API project for an E-commerce Furniture Application built using ASP.NET Core Web API. The project supports core e-commerce features including Authentication, User Profile Management, Product Browsing, Reviews, Orders, Cart management, and more.

---

## 🚀 Project Features

- 🔐 **Authentication & Authorization** (Register, Login, JWT Token)
- 👤 **User Profile Management**
  - View, update profile details
  - Upload, update, or delete profile images
- 🚚 **Shipping Address Management**
  - Add, update, delete one or multiple shipping addresses
- 🛋️ **Product Management**
  - Get all products
  - Get product by ID
  - Get products by category
  - Get all categories
- ⭐ **Review Module**
  - Add reviews for products
  - Manage and update reviews
- 🛒 **Cart Module**
  - Add to cart
  - View and manage cart items
- 📦 **Order Module**
  - Place orders
  - Manage order history
- 💳 **Payment Integration** (Stripe API)

---

## 🛠️ Technologies Used

- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- Onion Architecture
- Generic Repository Pattern
- Unit of Work Pattern
- Specification Design Pattern
- JWT Authentication
- Stripe Payment Gateway

---

## 🗂️ Project Architecture

The project follows **Onion Architecture**:
- Presentation Layer (API Controllers)
- Application Layer (Business Logic)
- Domain Layer (Entities & Interfaces)
- Infrastructure Layer (Repositories, Database Context)

---

## ⚙️ Getting Started

### Prerequisites:
- [.NET SDK](https://dotnet.microsoft.com/en-us/download)
- SQL Server
- Visual Studio or VS Code

### Setup Instructions:
1. Clone the repository:
   ```bash
   git clone https://github.com/Jamal3bsalam/FurnitureShoppingApp.git
