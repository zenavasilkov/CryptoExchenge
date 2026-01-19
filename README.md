# CryptoExchenge
---
<img width="1901" height="920" alt="image" src="https://github.com/user-attachments/assets/3ee561aa-7e56-48a9-856b-918c06ef4879" />

Short description
-----------------
**CryptoExchenge** — Crypto Exchange Simulator — is a simulator project that demonstrates a simplified cryptocurrency exchange: user accounts and balances, market & limit orders, an order matching engine, trade recording, and a web UI/API for interacting with the system.

Repository languages
--------------------
- C# (backend, ~59%)
- HTML (frontend pages, ~21%)
- CSS (styling, ~11%)
- JavaScript (frontend interactivity, ~9%)

Key features
------------
- Order types: market and limit orders
- Matching engine: price-time priority (FIFO within same price)
- Account and balance management (basic debit/credit on trades)
- Trade history and orderbook endpoints
- Frontend UI to inspect the order book, recent trades, and place orders

Getting started (local)
-----------------------
Prerequisites:
- .NET SDK (recommended 6.0+/7.0+ depending on the project)
- (Optional) Docker & Docker Compose

Run locally with dotnet:
1. Restore and build:
   - dotnet restore
   - dotnet build
2. Run the backend API:
   - dotnet run --project src/CryptoExchenge.Api
   (Adjust the project path to the actual API project in the repository.)
3. Open the frontend:
   - If the frontend is in a separate project, run that project or serve the static files (open index.html or use a static server).
4. Run tests:
   - dotnet test

Contact / Maintainer
--------------------
Maintainer: @zenavasilkov
