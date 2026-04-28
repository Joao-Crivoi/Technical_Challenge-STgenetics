# 🍔 GoodHamburger API

> A RESTful API for managing hamburger orders with an automated discount engine.
> Built as a technical challenge for STgenetics.

**Repository:** [github.com/Joao-Crivoi/Technical_Challenge-STgenetics](https://github.com/Joao-Crivoi/Technical_Challenge-STgenetics)

---

## 🇺🇸 English | [🇧🇷 Português](#-versão-em-português)

---

## 📐 Architecture Overview

This project follows **Clean Architecture** principles, organized as a multi-project .NET Solution:

```
Technical_Challenge-STgenetics/
├── src/
│   ├── GoodHamburger.Api/        # REST API — Controllers, Services, Repositories
│   ├── GoodHamburger.Shared/     # Shared Kernel — DTOs, ApiResponse, Route Constants
│   └── GoodHamburger.Web/        # Blazor WASM Frontend (in progress)
└── tests/
    └── GoodHamburger.Tests/      # Integration & Unit Tests
```

The `GoodHamburger.Shared` project is intentionally consumed by **both** the API and the
frontend — eliminating model duplication and ensuring a single source of truth for all
contracts across the system.

### Internal API Layer Structure

```
GoodHamburger.Api/
├── Domain/           # Entities, Enums, Domain Exceptions (zero external dependencies)
├── Application/      # Services, Interfaces, DTOs, AutoMapper Profiles
│   └── Strategies/   # Discount Strategy Pattern implementations
├── Infrastructure/   # EF Core DbContext, Repositories, Database Seed
└── Web/              # Controllers, Exception Middleware, Route Constants
```

---

## 🧠 Key Design Decisions

### Strategy Pattern for Discounts

Instead of a chain of `if/else` statements, each discount rule is an **isolated,
independently testable class** implementing `IDiscountStrategy`.
The service automatically selects the highest applicable discount — respecting the
**Open/Closed Principle**: adding a new discount rule requires only a new class, with
zero changes to existing code.

| Strategy | Rule | Discount |
|---|---|---|
| `FullComboStrategy` | Sandwich + Side + Drink | 20% |
| `SandwichDrinkStrategy` | Sandwich + Drink | 15% |
| `SandwichSideStrategy` | Sandwich + Side | 10% |

### Shared Kernel

`GoodHamburger.Shared` centralizes:
- `ApiResponse<T>` — standardized envelope for all API responses
- `ApiRoutes` — typed route constants, eliminating magic strings across the API and the Blazor frontend

```csharp
// Both the API tests and the Blazor frontend consume the same constants
var result = await _http.GetFromJsonAsync<ApiResponse<IEnumerable<ProductResponseDTO>>>(ApiRoutes.Products);
```

---

## 🛠 Tech Stack

| Layer | Technology |
|---|---|
| Framework | .NET 8 / ASP.NET Core |
| Database | SQLite + Entity Framework Core |
| Mapping | AutoMapper |
| Validation | FluentValidation |
| Testing | xUnit, Moq, FluentAssertions |
| API Docs | Swagger / OpenAPI |

---

## 🧪 Testing Strategy

A hybrid approach ensures both correctness and long-term confidence:

### Integration Tests (`/tests/Integration/`)

Uses a custom `WebApplicationFactory` with a **Singleton SQLite In-Memory connection**.
Every test run gets a fresh, isolated database — no physical `.db` files, no dirty state
between runs, no flaky tests due to leftover data.

```csharp
// CustomWebApplicationFactory replaces the real DB with :memory:
services.AddSingleton<DbConnection>(_ => {
    var connection = new SqliteConnection("DataSource=:memory:");
    connection.Open();
    return connection;
});
```

### Unit Tests (`/tests/Unit/`)

Pure business logic validation using **Moq** — no database, no HTTP, no external
dependencies. Covers all discount combinations via `[Theory]` + `[InlineData]`:

```
✓ Full Combo (Sandwich + Side + Drink)  → 20% discount
✓ Sandwich + Drink                       → 15% discount
✓ Sandwich + Side                        → 10% discount
✓ Sandwich only                          → 0%  discount
```

---

## 🚀 Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

### Running the API

```bash
git clone https://github.com/Joao-Crivoi/Technical_Challenge-STgenetics.git
cd Technical_Challenge-STgenetics

dotnet restore
dotnet run --project GoodHamburger.Api
```

The API will be available at: `http://localhost:5042`
Swagger UI: `http://localhost:5042/swagger`

The database is created and seeded automatically on first run via `EnsureCreated()` + `DbInitializer.Seed()`.
No manual migration steps required.

### Running Tests

```bash
dotnet test
```

Tests use an in-memory database — no setup required.

---

## 📋 API Endpoints

### Menu

| Method | Endpoint | Description |
|---|---|---|
| GET | `/api/Products` | Get full menu |

### Orders

| Method | Endpoint | Description |
|---|---|---|
| POST | `/api/Orders` | Create a new order |
| GET | `/api/Orders` | List all orders |
| GET | `/api/Orders/{id}` | Get order by ID (GUID) |
| PUT | `/api/Orders/{id}` | Update an order |
| DELETE | `/api/Orders/{id}` | Delete an order |

### Menu — Product IDs

| ID | Product | Category | Price |
|---|---|---|---|
| 1 | X Burger | Sandwich | R$ 5.00 |
| 2 | X Egg | Sandwich | R$ 4.50 |
| 3 | X Bacon | Sandwich | R$ 7.00 |
| 4 | Batata frita | Side | R$ 2.00 |
| 5 | Refrigerante | Drink | R$ 2.50 |

### Example Request

```json
POST /api/Orders
{
  "productIds": [1, 4, 5]
}
```

### Example Response

```json
{
  "success": true,
  "data": {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "subtotal": 9.50,
    "discountAmount": 1.90,
    "total": 7.60,
    "items": [
      { "productName": "X Burger", "unitPrice": 5.00 },
      { "productName": "Batata frita", "unitPrice": 2.00 },
      { "productName": "Refrigerante", "unitPrice": 2.50 }
    ]
  }
}
```

### Validation Rules

- Each order may contain **at most one** sandwich, one side, and one drink
- Duplicate items return a clear `400 Bad Request` with an error message
- Non-existent product IDs return `404 Not Found`
- Invalid order IDs return `404 Not Found`

---

## 🗺️ Roadmap — Production Vision

This challenge implements the core order engine. The full production vision includes:

### Authentication & Roles
- `Admin` — full CRUD on Products, Staff, and audit log access
- `Attendant` — accepts orders, updates status, issues order ticket

### Order Lifecycle
```
Created → Accepted → In Preparation → Ready → Delivered
```
Each transition is logged immutably for full audit trail.

### Queue System
- Sequential order number generated on creation (visible to customer)
- FIFO queue with configurable concurrent attendant slots (e.g. 3 at a time)
- Estimated preparation time based on item composition

### Order Ticket
- Printable receipt with order number, items, totals, payment method, and timestamp
- Stored as an immutable log — queryable by Admin
- Order number displayed on the customer-facing queue screen

### Real-time
- Customer-facing live queue display via SignalR
- "Your order is ready" notification when status changes to Ready

---

## ⚠️ Known Limitations

- **`EnsureCreated()` in production**: Currently used for simplicity. In a production
  environment, `dotnet ef database update` with proper Migrations would be used to
  evolve the schema safely without data loss.
- **No authentication**: Out of scope for this challenge (see Roadmap above).
- **SQLite vs. production DB**: Tests use SQLite In-Memory. In a CI/CD pipeline,
  TestContainers with the production database engine (e.g. PostgreSQL) would provide
  full parity.

---

---

## 🇧🇷 Versão em Português

## 📐 Visão Geral da Arquitetura

O projeto segue princípios de **Clean Architecture**, organizado como uma Solution .NET
multi-projeto. O `GoodHamburger.Shared` é consumido tanto pela API quanto pelo frontend
Blazor — eliminando duplicação de modelos e garantindo uma única fonte de verdade para
todos os contratos do sistema.

## 🧠 Decisões de Design

### Strategy Pattern para Descontos

Cada regra de desconto é uma **classe isolada e testável independentemente**,
implementando `IDiscountStrategy`. O serviço seleciona automaticamente o maior desconto
aplicável, respeitando o **Princípio Aberto/Fechado** — adicionar uma nova regra exige
apenas uma nova classe, sem alterar o código existente.

### Shared Kernel

Centraliza `ApiResponse<T>` e constantes de rota, eliminando strings mágicas tanto na
API quanto no frontend Blazor. Ambos os projetos referenciam o mesmo contrato.

## 🧪 Estratégia de Testes

**Testes de Integração**: Utiliza `WebApplicationFactory` customizada com **SQLite
In-Memory via conexão Singleton**. Banco virgem a cada execução — sem arquivos `.db`
físicos, sem estado sujo entre testes.

**Testes de Unidade**: Validação pura da lógica de negócio com **Moq**, sem banco de
dados ou dependências externas. Cobre todas as combinações de desconto (20%, 15%, 10%
e 0%) via `[Theory]` + `[InlineData]`.

## 🚀 Como Executar

```bash
git clone https://github.com/Joao-Crivoi/Technical_Challenge-STgenetics.git
cd Technical_Challenge-STgenetics

dotnet restore
dotnet run --project GoodHamburger.Api
```

API disponível em: `http://localhost:5042`
Swagger: `http://localhost:5042/swagger`

O banco é criado e populado automaticamente na primeira execução. Nenhum passo de
migration manual é necessário.

### Rodar os Testes

```bash
dotnet test
```

## 🗺️ Roadmap — Visão de Produção

### Autenticação e Perfis
- `Admin` — CRUD completo de Produtos, Funcionários e acesso aos logs
- `Atendente` — aceita pedidos, atualiza status, emite comprovante

### Ciclo de Vida do Pedido
```
Criado → Aceito → Em Preparo → Pronto → Entregue
```
Cada transição registrada de forma imutável para auditoria completa.

### Sistema de Fila
- Número sequencial de pedido gerado na criação (visível ao cliente)
- Fila FIFO com slots configuráveis por atendente (ex: 3 simultâneos)
- Tempo estimado de preparo baseado na composição do pedido

### Nota/Comprovante
- Recibo imprimível com número do pedido, itens, totais, forma de pagamento e hora
- Armazenado como log imutável — consultável pelo Admin

### Tempo Real
- Painel de fila visível ao cliente via SignalR
- Notificação de "Seu pedido está pronto" ao mudar o status para Pronto

## ⚠️ Limitações Conhecidas

- **`EnsureCreated()` em produção**: Usado por simplicidade. Em produção, seria
  substituído por `dotnet ef database update` com Migrations para evitar perda de dados.
- **Sem autenticação**: Fora do escopo deste desafio (ver Roadmap).
- **SQLite vs. banco de produção**: Os testes usam SQLite In-Memory. Em CI/CD,
  TestContainers com o banco real garantiria paridade total.
