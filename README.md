
# Backtesting Dashboard

🔗 **Live demo:** [vertical-slice-architecture.vercel.app/backtesting/moving-average](https://vertical-slice-architecture.vercel.app/backtesting/moving-average)

## Table of Content

- [Overview](#overview)
- [Architecture](#architecture)
- [Current Status](#current-status)
- [Installation](#installation)

## Overview

Dashboard is a full-stack finance tool for exploring trading strategies against historical market data. It pairs a .NET backend built with Vertical Slice Architecture (VSA) with a Next.js frontend, and currently ships a moving-average crossover backtester.

The project combines:
- **Quant Fundamentals**: Backtests strategies (e.g. moving-average crossovers) against historical daily ticker data.
- **External Data Integration**: Pulls and persists historical ticker data from the [Polygon.io](https://polygon.io) API.
- **Visualization**: Interactive, zoomable price/strategy charts built with [SciChart](https://www.scichart.com/) and Redux-managed state.
- **Software Engineering Practices**: Built with Vertical Slice Architecture (VSA) for clean separation of features, testability, and scalability.

## Architecture

```
VerticalSliceArchitecture/
 ├── dashboard.ui/                      # Next.js frontend (React, MUI, Redux, SciChart)
 │    ├── app/(routes)/                 # Pages (backtesting/moving-average, notes)
 │    ├── app/api/                      # Next.js API routes
 │    ├── app/_redux/                   # Redux slices & store
 │    └── app/_shared/                  # Shared API clients, types, and local data
 │
 ├── Dashboard.Application/             # ASP.NET Core Web API + vertical slices (features)
 │    └── Slice/
 │         ├── Backtesting/             # BacktestingController, BacktestingService
 │         └── ExternalData/            # Polygon.io ingestion (ExternalDataService)
 │
 ├── Dashboard.Infrastructure/          # EF Core DbContext, entity configurations, repositories
 │
 ├── Dashboard.Domain/                  # Core domain entities, value objects, events, exceptions
 │
 ├── Dashboard.Tests/                   # Backend unit/integration tests
 │
 └── Dashboard.Benchmarks/              # BenchmarkDotNet performance benchmarks
```

## Current Status

The frontend is hosted on Vercel at [vertical-slice-architecture.vercel.app/backtesting/moving-average](https://vertical-slice-architecture.vercel.app/backtesting/moving-average), but the frontend and backend are not currently hosted together. The backend (`Dashboard.Application`) requires a SQL Server instance and a Polygon.io API key, which isn't practical to stand up alongside a hosted frontend demo.

To keep the UI usable standalone, `dashboard.ui` ships its own Next.js API routes (`app/api/backtesting/*`) that read from a bundled CSV snapshot of daily ticker data (`app/_shared/dailyTickerSummary.csv`) and compute the moving-average backtest in-browser/server-side, instead of calling the .NET API. This means the hosted UI works out of the box, but reflects a fixed data snapshot rather than live/refreshable data.

To run against the real backend (live data ingestion, SQL Server persistence, etc.), point `NEXT_PUBLIC_API_URL` at a locally running instance of `Dashboard.Application` — see [Installation](#installation).

## Installation

### Backend (`Dashboard.Application`)

1. Provision a SQL Server instance and set the `DashboardDb` connection string in `Dashboard.Application/appsettings.Development.json`.
2. Add a Polygon.io API key for external data ingestion.
3. From the repo root: `dotnet run --project Dashboard.Application`.

### Frontend (`dashboard.ui`)

```bash
cd dashboard.ui
bun install
bun run dev
```

By default the UI runs standalone against the bundled CSV snapshot. To point it at a local backend instead, set:

```
NEXT_PUBLIC_API_URL=https://localhost:7212
```

Open [http://localhost:3000](http://localhost:3000) to view the app.
