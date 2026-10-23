
# ChainMarket

An ASP.NET Core MVC e-commerce application for industrial chains and components.

Developed by **Vessy Penkova**.

## Overview

ChainMarket is an industrial chain webshop developed using C#, ASP.NET Core MVC, Entity Framework Core and SQLite.

The application supports product nomenclature, technical standards, stock management, supplier purchasing and customer ordering.

The customer-facing website is in Bulgarian.

## Product Categories

- Roller chains
- Conveyor chains
- Leaf and lifting chains
- Agricultural chains
- Stainless steel and special chains
- Connecting links and accessories


## Implemented Features

- Product category navigation with images
- Individual product details pages with technical specifications
- Product image and technical drawing display when available
- Technical information navigation from product catalogue
- Product catalogue with category filtering and search
- Product creation form
- Session-based shopping cart with add, update and remove functionality
- Shopping cart support for meter-based and piece-based quantities
- Shopping cart price and total calculations in EUR
- Shopping cart validation and session persistence during application use
- Guest checkout for individual and company customers
- Customer and delivery information collection and validation
- Company invoice details (company name, EIK, VAT number, billing address)
- Required-field validation for company orders
- Multi-product customer orders and database relationships
- Customer orders and order items saved to SQLite
- Checkout order service with database-based pricing and 20% VAT calculations
- Order confirmation page with unique order number
- Shopping cart cleared after successful order creation
- Normalized technical nomenclature and chain standards
- Automatic product seeding
- Purchase and selling prices in EUR
- Meter-based and piece-based product units
- Stock availability calculations
- Advance purchase-order draft calculations
- Supplier and stock receipt models
- Database relationships and validation
- Responsive industrial-themed website


## Technology Stack

- C#
- ASP.NET Core MVC (.NET 8)
- Entity Framework Core
- SQLite
- Bootstrap
- HTML, CSS and JavaScript
- NUnit
- Visual Studio 2022
- Git and GitHub

## Project Structure

```text
ChainMarket/
├── ChainMarket.Web/
│   ├── Controllers/
│   ├── Data/
│   ├── Models/
│   ├── Services/
│   ├── Views/
│   └── wwwroot/
├── ChainMarket.Tests/
└── ChainMarket.sln
```

## Running the Application

1. Clone the repository.
2. Open `ChainMarket.sln` in Visual Studio 2022.
3. Restore NuGet packages.
4. Configure the SQLite connection string.
5. Apply Entity Framework Core migrations using `Update-Database`.
6. Run `ChainMarket.Web`.

## Testing

Automated tests are implemented with NUnit and EF Core SQLite in-memory databases.

The project currently has **77 passing tests** covering data validation, database relationships, stock calculations, nomenclature importing, shopping cart operations, controller validation, session storage, checkout form validation and customer order creation.

## Development Status

**Work in progress.**

The following features are planned or under development:

- Customer checkout (work in progress: invoice details, order review and final validation)
- Complete customer ordering workflow
- Administration and access control
- Stock receipts and supplier purchasing interfaces
- Product images and technical specifications
- Country of origin and Intrastat reporting

The application is not yet ready for commercial deployment.

## Author

**Vessy Penkova**

© 2026 ChainMarket
