# KillerDex 2.0 - API Documentation

Complete REST API documentation for Dead by Daylight game data.

## Base URL

```
https://localhost:5001/api
```

## Authentication

Write operations require an API Key in the request header:

```
X-Api-Key: your-api-key
```

| Operation | Authentication |
|-----------|----------------|
| GET | Not required |
| POST | Required |
| PUT | Required |
| DELETE | Required |

## Response Formats

### Success Response
```json
{
  "id": "guid",
  "slug": "entity-slug",
  "name": "Entity Name",
  ...
}
```

### Error Responses

| Status Code | Description |
|-------------|-------------|
| 200 | Success |
| 201 | Created |
| 204 | No Content (Delete success) |
| 400 | Bad Request |
| 401 | Unauthorized (Missing/Invalid API Key) |
| 404 | Not Found |

---

## Killers

### Get All Killers
```http
GET /api/killers
```

**Response:** `200 OK`
```json
[
  {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "slug": "the-trapper",
    "name": "The Trapper",
    "imageUrl": "https://example.com/trapper.png",
    "powerName": "Bear Trap"
  }
]
```

### Get Killer by ID
```http
GET /api/killers/{id}
```

**Parameters:**
| Name | Type | Description |
|------|------|-------------|
| id | guid | Killer ID |

**Response:** `200 OK`
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "slug": "the-trapper",
  "name": "The Trapper",
  "realName": "Evan MacMillan",
  "overview": "A killer who uses bear traps...",
  "backstory": "Evan MacMillan grew up...",
  "imageUrl": "https://example.com/trapper.png",
  "gameVersion": "1.0.0",
  "power": {
    "name": "Bear Trap",
    "description": "A deadly trap that..."
  },
  "movementSpeed": 4.6,
  "terrorRadius": 32,
  "height": "Tall",
  "chapter": {
    "id": "guid",
    "slug": "base-game",
    "name": "Base Game",
    "number": 0
  },
  "createdAt": "2024-01-01T00:00:00Z",
  "updatedAt": "2024-01-01T00:00:00Z"
}
```

### Get Killer by Slug
```http
GET /api/killers/{slug}
```

**Parameters:**
| Name | Type | Description |
|------|------|-------------|
| slug | string | Killer slug (e.g., "the-trapper") |

### Get Killer's Perks
```http
GET /api/killers/{id}/perks
```

**Response:** `200 OK`
```json
[
  {
    "id": "guid",
    "slug": "unnerving-presence",
    "name": "Unnerving Presence",
    "imageUrl": "https://example.com/perk.png",
    "role": "Killer"
  }
]
```

### Get Killer's Addons
```http
GET /api/killers/{id}/addons
```

**Response:** `200 OK`
```json
[
  {
    "id": "guid",
    "slug": "trapper-bag",
    "name": "Trapper Bag",
    "imageUrl": "https://example.com/addon.png",
    "rarity": "Uncommon"
  }
]
```

### Create Killer
```http
POST /api/killers
```
**Authentication:** Required

**Request Body:**
```json
{
  "name": "The Killer",
  "realName": "John Doe",
  "overview": "A deadly killer...",
  "backstory": "The story of...",
  "power": {
    "name": "Power Name",
    "description": "Power description..."
  },
  "movementSpeed": 4.6,
  "terrorRadius": 32,
  "height": "Average",
  "chapterId": "guid",
  "imageUrl": "https://example.com/image.png",
  "gameVersion": "1.0.0"
}
```

**Response:** `201 Created`

### Update Killer
```http
PUT /api/killers/{id}
```
**Authentication:** Required

**Request Body:** (all fields optional)
```json
{
  "name": "Updated Name",
  "realName": "Updated Real Name",
  "overview": "Updated overview...",
  "backstory": "Updated backstory...",
  "power": {
    "name": "Updated Power",
    "description": "Updated description..."
  },
  "movementSpeed": 4.4,
  "terrorRadius": 24,
  "height": "Short",
  "imageUrl": "https://example.com/new-image.png",
  "gameVersion": "2.0.0"
}
```

**Response:** `200 OK`

### Delete Killer
```http
DELETE /api/killers/{id}
```
**Authentication:** Required

**Response:** `204 No Content`

---

## Survivors

### Get All Survivors
```http
GET /api/survivors
```

**Response:** `200 OK`
```json
[
  {
    "id": "guid",
    "slug": "dwight-fairfield",
    "name": "Dwight Fairfield",
    "imageUrl": "https://example.com/dwight.png"
  }
]
```

### Get Survivor by ID
```http
GET /api/survivors/{id}
```

**Response:** `200 OK`
```json
{
  "id": "guid",
  "slug": "dwight-fairfield",
  "name": "Dwight Fairfield",
  "overview": "A nervous leader...",
  "backstory": "Dwight was always...",
  "imageUrl": "https://example.com/dwight.png",
  "gameVersion": "1.0.0",
  "chapter": {
    "id": "guid",
    "slug": "base-game",
    "name": "Base Game",
    "number": 0
  },
  "createdAt": "2024-01-01T00:00:00Z",
  "updatedAt": "2024-01-01T00:00:00Z"
}
```

### Get Survivor by Slug
```http
GET /api/survivors/{slug}
```

### Get Survivor's Perks
```http
GET /api/survivors/{id}/perks
```

### Create Survivor
```http
POST /api/survivors
```
**Authentication:** Required

**Request Body:**
```json
{
  "name": "New Survivor",
  "overview": "A survivor overview...",
  "backstory": "The survivor's story...",
  "chapterId": "guid",
  "imageUrl": "https://example.com/image.png",
  "gameVersion": "1.0.0"
}
```

### Update Survivor
```http
PUT /api/survivors/{id}
```
**Authentication:** Required

### Delete Survivor
```http
DELETE /api/survivors/{id}
```
**Authentication:** Required

---

## Chapters

### Get All Chapters
```http
GET /api/chapters
```

**Response:** `200 OK`
```json
[
  {
    "id": "guid",
    "slug": "chapter-1-the-last-breath",
    "name": "Chapter 1: The Last Breath",
    "number": 1
  }
]
```

### Get Chapter by ID
```http
GET /api/chapters/{id}
```

**Response:** `200 OK`
```json
{
  "id": "guid",
  "slug": "chapter-1-the-last-breath",
  "name": "Chapter 1: The Last Breath",
  "number": 1,
  "releaseDate": "2016-08-18",
  "imageUrl": "https://example.com/chapter.png",
  "gameVersion": "1.1.0",
  "killers": [...],
  "survivors": [...],
  "createdAt": "2024-01-01T00:00:00Z",
  "updatedAt": "2024-01-01T00:00:00Z"
}
```

### Get Chapter by Slug
```http
GET /api/chapters/{slug}
```

### Get Chapter by Number
```http
GET /api/chapters/number/{number}
```

### Create Chapter
```http
POST /api/chapters
```
**Authentication:** Required

**Request Body:**
```json
{
  "name": "Chapter X: Name",
  "number": 30,
  "releaseDate": "2024-06-01",
  "imageUrl": "https://example.com/image.png",
  "gameVersion": "8.0.0"
}
```

### Update Chapter
```http
PUT /api/chapters/{id}
```
**Authentication:** Required

### Delete Chapter
```http
DELETE /api/chapters/{id}
```
**Authentication:** Required

---

## Perks

### Get All Perks
```http
GET /api/perks
```

**Query Parameters:**
| Name | Type | Description |
|------|------|-------------|
| role | string | Filter by role: `Killer` or `Survivor` |

**Example:**
```http
GET /api/perks?role=Killer
```

**Response:** `200 OK`
```json
[
  {
    "id": "guid",
    "slug": "hex-ruin",
    "name": "Hex: Ruin",
    "imageUrl": "https://example.com/ruin.png",
    "role": "Killer"
  }
]
```

### Get Perk by ID
```http
GET /api/perks/{id}
```

**Response:** `200 OK`
```json
{
  "id": "guid",
  "slug": "hex-ruin",
  "name": "Hex: Ruin",
  "description": "A Hex that affects generator repair...",
  "imageUrl": "https://example.com/ruin.png",
  "gameVersion": "1.0.0",
  "role": "Killer",
  "owner": {
    "id": "guid",
    "slug": "the-hag",
    "name": "The Hag",
    "type": "Killer"
  },
  "createdAt": "2024-01-01T00:00:00Z",
  "updatedAt": "2024-01-01T00:00:00Z"
}
```

### Get Perk by Slug
```http
GET /api/perks/{slug}
```

### Create Perk
```http
POST /api/perks
```
**Authentication:** Required

**Request Body:**
```json
{
  "name": "New Perk",
  "description": "Perk description...",
  "role": "Killer",
  "killerId": "guid",
  "imageUrl": "https://example.com/image.png",
  "gameVersion": "1.0.0"
}
```

> **Note:** Set either `killerId` OR `survivorId`, not both.

### Update Perk
```http
PUT /api/perks/{id}
```
**Authentication:** Required

### Delete Perk
```http
DELETE /api/perks/{id}
```
**Authentication:** Required

---

## Killer Addons

### Get All Killer Addons
```http
GET /api/killer-addons
```

**Query Parameters:**
| Name | Type | Description |
|------|------|-------------|
| killerId | guid | Filter by killer ID |

**Example:**
```http
GET /api/killer-addons?killerId=3fa85f64-5717-4562-b3fc-2c963f66afa6
```

### Get Killer Addon by ID
```http
GET /api/killer-addons/{id}
```

**Response:** `200 OK`
```json
{
  "id": "guid",
  "slug": "trapper-bag",
  "name": "Trapper Bag",
  "description": "A leather bag...",
  "imageUrl": "https://example.com/addon.png",
  "gameVersion": "1.0.0",
  "rarity": "Uncommon",
  "killer": {
    "id": "guid",
    "slug": "the-trapper",
    "name": "The Trapper",
    "imageUrl": "...",
    "powerName": "Bear Trap"
  },
  "createdAt": "2024-01-01T00:00:00Z",
  "updatedAt": "2024-01-01T00:00:00Z"
}
```

### Get Killer Addon by Slug
```http
GET /api/killer-addons/{slug}
```

### Create Killer Addon
```http
POST /api/killer-addons
```
**Authentication:** Required

**Request Body:**
```json
{
  "name": "New Addon",
  "description": "Addon description...",
  "rarity": "Rare",
  "killerId": "guid",
  "imageUrl": "https://example.com/image.png",
  "gameVersion": "1.0.0"
}
```

### Update Killer Addon
```http
PUT /api/killer-addons/{id}
```
**Authentication:** Required

### Delete Killer Addon
```http
DELETE /api/killer-addons/{id}
```
**Authentication:** Required

---

## Survivor Addons

### Get All Survivor Addons
```http
GET /api/survivor-addons
```

**Query Parameters:**
| Name | Type | Description |
|------|------|-------------|
| itemType | string | Filter by item type: `Medkit`, `Flashlight`, `Toolbox`, `Map`, `Key` |

**Example:**
```http
GET /api/survivor-addons?itemType=Medkit
```

### Get Survivor Addon by ID
```http
GET /api/survivor-addons/{id}
```

**Response:** `200 OK`
```json
{
  "id": "guid",
  "slug": "rubber-gloves",
  "name": "Rubber Gloves",
  "description": "Thick rubber gloves...",
  "imageUrl": "https://example.com/addon.png",
  "gameVersion": "1.0.0",
  "rarity": "Common",
  "itemType": "Medkit",
  "createdAt": "2024-01-01T00:00:00Z",
  "updatedAt": "2024-01-01T00:00:00Z"
}
```

### Get Survivor Addon by Slug
```http
GET /api/survivor-addons/{slug}
```

### Create Survivor Addon
```http
POST /api/survivor-addons
```
**Authentication:** Required

**Request Body:**
```json
{
  "name": "New Addon",
  "description": "Addon description...",
  "rarity": "Uncommon",
  "itemType": "Flashlight",
  "imageUrl": "https://example.com/image.png",
  "gameVersion": "1.0.0"
}
```

### Update Survivor Addon
```http
PUT /api/survivor-addons/{id}
```
**Authentication:** Required

### Delete Survivor Addon
```http
DELETE /api/survivor-addons/{id}
```
**Authentication:** Required

---

## Items

### Get All Items
```http
GET /api/items
```

**Query Parameters:**
| Name | Type | Description |
|------|------|-------------|
| type | string | Filter by type: `Medkit`, `Flashlight`, `Toolbox`, `Map`, `Key` |

### Get Item by ID
```http
GET /api/items/{id}
```

**Response:** `200 OK`
```json
{
  "id": "guid",
  "slug": "camping-aid-kit",
  "name": "Camping Aid Kit",
  "description": "A rudimentary first aid kit...",
  "imageUrl": "https://example.com/item.png",
  "gameVersion": "1.0.0",
  "type": "Medkit",
  "rarity": "Common",
  "createdAt": "2024-01-01T00:00:00Z",
  "updatedAt": "2024-01-01T00:00:00Z"
}
```

### Get Item by Slug
```http
GET /api/items/{slug}
```

### Get Item Type Addons
```http
GET /api/items/type/{itemType}/addons
```

Returns all survivor addons for a specific item type.

### Create Item
```http
POST /api/items
```
**Authentication:** Required

**Request Body:**
```json
{
  "name": "New Item",
  "description": "Item description...",
  "type": "Medkit",
  "rarity": "Rare",
  "imageUrl": "https://example.com/image.png",
  "gameVersion": "1.0.0"
}
```

### Update Item
```http
PUT /api/items/{id}
```
**Authentication:** Required

### Delete Item
```http
DELETE /api/items/{id}
```
**Authentication:** Required

---

## Offerings

### Get All Offerings
```http
GET /api/offerings
```

**Query Parameters:**
| Name | Type | Description |
|------|------|-------------|
| role | string | Filter by role: `Killer`, `Survivor`, or `All` |

### Get Offering by ID
```http
GET /api/offerings/{id}
```

**Response:** `200 OK`
```json
{
  "id": "guid",
  "slug": "bloody-party-streamers",
  "name": "Bloody Party Streamers",
  "description": "Grants 100% bonus Bloodpoints...",
  "imageUrl": "https://example.com/offering.png",
  "gameVersion": "1.0.0",
  "rarity": "Rare",
  "role": "All",
  "createdAt": "2024-01-01T00:00:00Z",
  "updatedAt": "2024-01-01T00:00:00Z"
}
```

### Get Offering by Slug
```http
GET /api/offerings/{slug}
```

### Create Offering
```http
POST /api/offerings
```
**Authentication:** Required

**Request Body:**
```json
{
  "name": "New Offering",
  "description": "Offering description...",
  "rarity": "Uncommon",
  "role": "Killer",
  "imageUrl": "https://example.com/image.png",
  "gameVersion": "1.0.0"
}
```

### Update Offering
```http
PUT /api/offerings/{id}
```
**Authentication:** Required

### Delete Offering
```http
DELETE /api/offerings/{id}
```
**Authentication:** Required

---

## Realms

### Get All Realms
```http
GET /api/realms
```

**Response:** `200 OK`
```json
[
  {
    "id": "guid",
    "slug": "the-macmillan-estate",
    "name": "The MacMillan Estate",
    "imageUrl": "https://example.com/realm.png"
  }
]
```

### Get Realm by ID
```http
GET /api/realms/{id}
```

**Response:** `200 OK`
```json
{
  "id": "guid",
  "slug": "the-macmillan-estate",
  "name": "The MacMillan Estate",
  "description": "A dark industrial complex...",
  "imageUrl": "https://example.com/realm.png",
  "gameVersion": "1.0.0",
  "killer": {
    "id": "guid",
    "slug": "the-trapper",
    "name": "The Trapper",
    "imageUrl": "...",
    "powerName": "Bear Trap"
  },
  "maps": [
    {
      "id": "guid",
      "slug": "coal-tower",
      "name": "Coal Tower",
      "imageUrl": "..."
    }
  ],
  "createdAt": "2024-01-01T00:00:00Z",
  "updatedAt": "2024-01-01T00:00:00Z"
}
```

### Get Realm by Slug
```http
GET /api/realms/{slug}
```

### Create Realm
```http
POST /api/realms
```
**Authentication:** Required

**Request Body:**
```json
{
  "name": "New Realm",
  "description": "Realm description...",
  "killerId": "guid",
  "imageUrl": "https://example.com/image.png",
  "gameVersion": "1.0.0"
}
```

> **Note:** `killerId` is optional. Realms without a killer are shared/generic.

### Update Realm
```http
PUT /api/realms/{id}
```
**Authentication:** Required

### Delete Realm
```http
DELETE /api/realms/{id}
```
**Authentication:** Required

---

## Maps

### Get All Maps
```http
GET /api/maps
```

**Query Parameters:**
| Name | Type | Description |
|------|------|-------------|
| realmId | guid | Filter by realm ID |

### Get Map by ID
```http
GET /api/maps/{id}
```

**Response:** `200 OK`
```json
{
  "id": "guid",
  "slug": "coal-tower",
  "name": "Coal Tower",
  "description": "A dark tower...",
  "imageUrl": "https://example.com/map.png",
  "gameVersion": "1.0.0",
  "realm": {
    "id": "guid",
    "slug": "the-macmillan-estate",
    "name": "The MacMillan Estate",
    "imageUrl": "..."
  },
  "createdAt": "2024-01-01T00:00:00Z",
  "updatedAt": "2024-01-01T00:00:00Z"
}
```

### Get Map by Slug
```http
GET /api/maps/{slug}
```

### Create Map
```http
POST /api/maps
```
**Authentication:** Required

**Request Body:**
```json
{
  "name": "New Map",
  "description": "Map description...",
  "realmId": "guid",
  "imageUrl": "https://example.com/image.png",
  "gameVersion": "1.0.0"
}
```

### Update Map
```http
PUT /api/maps/{id}
```
**Authentication:** Required

### Delete Map
```http
DELETE /api/maps/{id}
```
**Authentication:** Required

---

## Status Effects

### Get All Status Effects
```http
GET /api/status-effects
```

**Query Parameters:**
| Name | Type | Description |
|------|------|-------------|
| type | string | Filter by type: `Buff` or `Debuff` |

### Get Status Effect by ID
```http
GET /api/status-effects/{id}
```

**Response:** `200 OK`
```json
{
  "id": "guid",
  "slug": "exhausted",
  "name": "Exhausted",
  "description": "You are exhausted and cannot...",
  "imageUrl": "https://example.com/effect.png",
  "gameVersion": "1.0.0",
  "type": "Debuff",
  "appliesTo": "Survivor",
  "createdAt": "2024-01-01T00:00:00Z",
  "updatedAt": "2024-01-01T00:00:00Z"
}
```

### Get Status Effect by Slug
```http
GET /api/status-effects/{slug}
```

### Create Status Effect
```http
POST /api/status-effects
```
**Authentication:** Required

**Request Body:**
```json
{
  "name": "New Status Effect",
  "description": "Effect description...",
  "type": "Buff",
  "appliesTo": "Killer",
  "imageUrl": "https://example.com/image.png",
  "gameVersion": "1.0.0"
}
```

### Update Status Effect
```http
PUT /api/status-effects/{id}
```
**Authentication:** Required

### Delete Status Effect
```http
DELETE /api/status-effects/{id}
```
**Authentication:** Required

---

## Enum Values Reference

### Role
| Value | Description |
|-------|-------------|
| `Killer` | Killer-specific |
| `Survivor` | Survivor-specific |
| `All` | Both roles (Offerings, Status Effects only) |

### Rarity
| Value | Description |
|-------|-------------|
| `Common` | Brown |
| `Uncommon` | Yellow |
| `Rare` | Green |
| `VeryRare` | Purple |
| `UltraRare` | Red/Pink |
| `Event` | Special event items |

### ItemType
| Value | Description |
|-------|-------------|
| `Medkit` | Healing items |
| `Flashlight` | Light items |
| `Toolbox` | Repair/sabotage items |
| `Map` | Tracking items |
| `Key` | Aura reading items |

### KillerHeight
| Value | Description |
|-------|-------------|
| `Short` | 4.4 m/s base speed |
| `Average` | Standard height |
| `Tall` | Above average height |

### StatusEffectType
| Value | Description |
|-------|-------------|
| `Buff` | Positive effect |
| `Debuff` | Negative effect |

---

## Examples

### cURL Examples

**Get all killers:**
```bash
curl https://localhost:5001/api/killers
```

**Get killer by slug:**
```bash
curl https://localhost:5001/api/killers/the-trapper
```

**Create a new killer (requires API Key):**
```bash
curl -X POST https://localhost:5001/api/killers \
  -H "Content-Type: application/json" \
  -H "X-Api-Key: your-api-key" \
  -d '{
    "name": "The Example",
    "realName": "John Doe",
    "overview": "A killer for testing purposes.",
    "backstory": "Once upon a time...",
    "power": {
      "name": "Test Power",
      "description": "A power for testing."
    },
    "movementSpeed": 4.6,
    "terrorRadius": 32,
    "height": "Average",
    "gameVersion": "1.0.0"
  }'
```

**Update a killer:**
```bash
curl -X PUT https://localhost:5001/api/killers/3fa85f64-5717-4562-b3fc-2c963f66afa6 \
  -H "Content-Type: application/json" \
  -H "X-Api-Key: your-api-key" \
  -d '{
    "name": "The Updated Example"
  }'
```

**Delete a killer:**
```bash
curl -X DELETE https://localhost:5001/api/killers/3fa85f64-5717-4562-b3fc-2c963f66afa6 \
  -H "X-Api-Key: your-api-key"
```

### JavaScript/Fetch Examples

**Get all survivors:**
```javascript
const response = await fetch('https://localhost:5001/api/survivors');
const survivors = await response.json();
```

**Create a new perk:**
```javascript
const response = await fetch('https://localhost:5001/api/perks', {
  method: 'POST',
  headers: {
    'Content-Type': 'application/json',
    'X-Api-Key': 'your-api-key'
  },
  body: JSON.stringify({
    name: 'New Perk',
    description: 'Perk description...',
    role: 'Survivor',
    survivorId: 'survivor-guid-here',
    gameVersion: '1.0.0'
  })
});
const perk = await response.json();
```

### C# HttpClient Examples

**Get all items:**
```csharp
using var client = new HttpClient();
var items = await client.GetFromJsonAsync<List<ItemSummaryDto>>(
    "https://localhost:5001/api/items"
);
```

**Create a new offering:**
```csharp
using var client = new HttpClient();
client.DefaultRequestHeaders.Add("X-Api-Key", "your-api-key");

var request = new CreateOfferingRequest(
    Name: "New Offering",
    Description: "Description...",
    Rarity: "Rare",
    Role: "All",
    ImageUrl: null,
    GameVersion: "1.0.0"
);

var response = await client.PostAsJsonAsync(
    "https://localhost:5001/api/offerings",
    request
);
var offering = await response.Content.ReadFromJsonAsync<OfferingDto>();
```
