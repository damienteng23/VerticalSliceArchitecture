
# Quantitative Dashboard

## Table of Content

- [Overview](#overview)
- [Architecture](#architecture)
- [Installation](#installation)

## Overview

Dashboard is a cross‑platform desktop client built with .NET and Avalonia UI from scratch that demonstrates option pricing and risk analysis in an interactive environment. The application serves as both a learning tool and a portfolio project, designed to highlight expertise in quantitative finance, numerical methods, and modern UI development.

The project combines:
- Quant Finance Fundamentals: Implements classical models such as Black‑Scholes, Binomial Trees, and Monte Carlo simulations to calculate option prices and risk metrics.
- Numerical Methods: Showcases practical applications of stochastic modeling, iterative algorithms, and sensitivity analysis.
- Visualization Skills: Provides real‑time charting of option Greeks (Delta, Gamma, Vega, Theta, Rho) and scenario analysis tools (e.g., volatility shocks) to make risk dynamics visually intuitive.
- Software Engineering Practices: Built with a Vertical Slice Architecture (VSA) for clean separation of features, testability, and scalability, reflecting enterprise‑ready design principles.
By integrating financial models with a responsive desktop UI, Dashboard demonstrates how technical engineering skills and quantitative reasoning can be combined to produce tools that are both analytically rigorous and user‑friendly. This project is intended to serve as a showcase of applied knowledge in quantitative finance, numerical computation, and enterprise‑grade C# development.


## Architecture
```
Dashboard/
 ├── Dashboard.UI/                     # Avalonia UI project
 │    ├── Views/                       # XAML views (OptionInputView, RiskDashboardView)
 │    ├── ViewModels/                  # MVVM bindings per slice
 │    └── Assets/                      # Styles, icons
 │
 ├── Dashboard.Application/            # Vertical slices (features)
 │    ├── Pricing/
 │    │    ├── feature1/
 │    │    │    ├── feature1Command.cs
 │    │    │    ├── feature1Handler.cs
 │    │    │    ├── feature1Validator.cs
 │    │    │    ├── feature1ResultDto.cs
 │    │    │    └── feature1Tests.cs
 │    │    └── feature2/
 │    │         ├── feature2Command.cs
 │    │         ├── feature2Handler.cs
 │    │         └── feature2Tests.cs
 │    ├── Greeks/
 │    │    ├── CalculateGreeksCommand.cs
 │    │    ├── CalculateGreeksHandler.cs
 │    │    ├── GreeksDto.cs
 │    │    └── GreeksChartViewModel.cs
 │    │
 │    └── Shared/
 │         ├── Result.cs                # Standardized result wrapper
 │         ├── Exceptions.cs
 │         └── Interfaces.cs
 │
 ├── Dashboard.Infrastructure/          # Data & external services
 │    ├── Persistence/
 │    │    ├── MarketDataDbContext.cs   # EF Core or repository
 │    │    ├── Configurations/
 │    │    └── Migrations/
 │    ├── Services/
 │    │    ├── MarketDataApiClient.cs   # External API for live prices
 │    │    └── LoggingService.cs
 │    └── Repositories/
 │         └── OptionRepository.cs
 │
 ├── Dashboard.Domain/                 # Core domain models
 │    ├── Entities/
 │    │    ├── Option.cs
 │    │    ├── Portfolio.cs
 │    │    └── RiskMetrics.cs
 │    ├── ValueObjects/
 │    │    ├── Strike.cs
 │    │    ├── Expiry.cs
 │    │    └── Volatility.cs
 │    └── Events/
 │         └── PricingCalculatedEvent.cs
 │
 └── tests/
      ├── Dashboard.UnitTests/
      └── Dashboard.IntegrationTests/
```

# Installation
