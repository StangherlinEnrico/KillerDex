# KillerDex 2.0 - Dead by Daylight API

A comprehensive REST API for Dead by Daylight game data, built with .NET 9 and Clean Architecture.

> **Disclaimer:** This project is not affiliated with or endorsed by Behaviour Interactive Inc.

## Features

- Complete CRUD operations for all game entities
- API Key authentication for write operations
- Clean Architecture (Domain, Application, Infrastructure, API)
- Entity Framework Core with SQL Server
- OpenAPI documentation with Scalar UI
- Localization support (11 languages)

## Tech Stack

- **.NET 9.0**
- **Entity Framework Core 9.0**
- **SQL Server**
- **Scalar** (API Documentation)

## Project Structure

```
Backend/
├── Domain/                 # Entities, Value Objects, Enums, Interfaces
│   ├── Entities/          # Core domain entities
│   ├── ValueObjects/      # Value objects (Power)
│   ├── Enums/             # Role, Rarity, ItemType, etc.
│   ├── Interfaces/        # Repository interfaces
│   └── Events/            # Domain events
│
├── Application/           # Use cases, Services, DTOs
│   ├── DTOs/              # Data Transfer Objects
│   │   └── Requests/      # Create/Update request DTOs
│   ├── Interfaces/        # Service interfaces
│   ├── Services/          # Service implementations
│   └── Mappings/          # Entity to DTO mappings
│
├── Infrastructure/        # Data access, External services
│   ├── Persistence/       # DbContext, Configurations
│   │   ├── Configurations/
│   │   └── Repositories/
│   └── Interceptors/      # EF Core interceptors
│
└── API/                   # REST API, Controllers
    ├── Controllers/       # API endpoints
    └── Authentication/    # API Key handler
```

## Entities

| Entity | Description |
|--------|-------------|
| **Killer** | Playable killer characters with power, stats, and lore |
| **Survivor** | Playable survivor characters with lore |
| **Chapter** | Game chapters/DLCs containing killers and survivors |
| **Perk** | Character abilities (killer or survivor specific) |
| **KillerAddon** | Power add-ons for killers |
| **SurvivorAddon** | Item add-ons for survivors |
| **Item** | Survivor items (Medkit, Flashlight, Toolbox, Map, Key) |
| **Offering** | Pre-match offerings for both roles |
| **Realm** | Map realms (optionally killer-specific) |
| **Map** | Individual maps within realms |
| **StatusEffect** | Buffs and debuffs |

## Getting Started

### Prerequisites

- .NET 9.0 SDK
- SQL Server (LocalDB or full instance)

### Configuration

1. Update the connection string in `API/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=KillerDex;Trusted_Connection=True;MultipleActiveResultSets=true"
  },
  "Authentication": {
    "ApiKey": "YOUR-SECRET-API-KEY-CHANGE-THIS"
  }
}
```

2. Apply database migrations:

```bash
cd Backend
dotnet ef database update --project Infrastructure --startup-project API
```

### Running the API

```bash
cd Backend/API
dotnet run
```

The API will be available at:
- **HTTPS:** `https://localhost:5001`
- **HTTP:** `http://localhost:5000`

### API Documentation

When running in Development mode, access the interactive documentation at:
- **Scalar UI:** `https://localhost:5001/scalar/v1`
- **OpenAPI JSON:** `https://localhost:5001/openapi/v1.json`

## Authentication

The API uses **API Key authentication** for write operations (POST, PUT, DELETE).

### Public Endpoints (No Authentication)
All `GET` endpoints are publicly accessible.

### Protected Endpoints (Require API Key)
All `POST`, `PUT`, and `DELETE` endpoints require the API Key.

### Usage

Include the API Key in the request header:

```
X-Api-Key: your-api-key-here
```

Example with cURL:
```bash
curl -X POST https://localhost:5001/api/killers \
  -H "Content-Type: application/json" \
  -H "X-Api-Key: your-api-key-here" \
  -d '{"name": "The Example", ...}'
```

## API Endpoints

See [docs/API.md](docs/API.md) for complete API documentation.

### Quick Reference

| Resource | Endpoint | Methods |
|----------|----------|---------|
| Killers | `/api/killers` | GET, POST, PUT, DELETE |
| Survivors | `/api/survivors` | GET, POST, PUT, DELETE |
| Chapters | `/api/chapters` | GET, POST, PUT, DELETE |
| Perks | `/api/perks` | GET, POST, PUT, DELETE |
| Killer Addons | `/api/killer-addons` | GET, POST, PUT, DELETE |
| Survivor Addons | `/api/survivor-addons` | GET, POST, PUT, DELETE |
| Items | `/api/items` | GET, POST, PUT, DELETE |
| Offerings | `/api/offerings` | GET, POST, PUT, DELETE |
| Realms | `/api/realms` | GET, POST, PUT, DELETE |
| Maps | `/api/maps` | GET, POST, PUT, DELETE |
| Status Effects | `/api/status-effects` | GET, POST, PUT, DELETE |

## Enums

### Role
- `Killer` - Killer-specific
- `Survivor` - Survivor-specific
- `All` - Both roles (only for Offerings and StatusEffects)

### Rarity
- `Common`
- `Uncommon`
- `Rare`
- `VeryRare`
- `UltraRare`
- `Event`

### ItemType
- `Medkit`
- `Flashlight`
- `Toolbox`
- `Map`
- `Key`

### KillerHeight
- `Short`
- `Average`
- `Tall`

### StatusEffectType
- `Buff`
- `Debuff`

## Development

### Building

```bash
cd Backend
dotnet build
```

### Creating Migrations

```bash
cd Backend
dotnet ef migrations add MigrationName --project Infrastructure --startup-project API
```

### Running Tests

```bash
cd Backend
dotnet test
```

## License

This project is for educational purposes only. Dead by Daylight is a trademark of Behaviour Interactive Inc.
