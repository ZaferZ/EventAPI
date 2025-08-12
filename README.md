# 📅 Event API Documentation

## Overview
The **Event API** allows authenticated users to create, manage, and participate in events. It is a part of a EVEN

**Base URL:**
```
/api/event
```

**Authentication:**
- JWT Bearer Token required for most endpoints.
- Certain endpoints are restricted to specific roles (`user`, `admin`).

---

## **Endpoints**

---

### 1. Get All Events  
**`GET /api/event`**  

#### 📌 Description  
Retrieves all events in the system. **Public endpoint** — no authentication required.

#### 🔹 Request  
No parameters.

#### 🔹 Response Schema  
```json
[
  {
    "title": "string",
    "description": "string",
    "ownerId": "Guid",
    "hobbyId": "integer",
    "startDate": "DateTime",
    "endDate": "DateTime",
    "location": "string",
    "capacity": "integer",
    "participantIds": "List<Guid>",
        "status": "integer"
  }
]
```

#### 🔹 Example curl  
```bash
curl -X GET https://api.example.com/api/event
```

---

### 2. Get Events by User ID *(Admin Only)*  
**`GET /api/event/user/{id}`**  

#### 📌 Description  
Retrieves all events created by a specific user.

#### 🔹 Authentication  
- **Role:** `admin`

#### 🔹 Path Parameters  
| Name | Type | Required | Description |
|------|------|----------|-------------|
| id   | GUID | Yes      | The user's unique ID |

#### 🔹 Response Example  
```json
[
 {
        "title": "Painting Workshop",
        "description": "Learn to paint with watercolors.",
        "ownerId": "7e61f925-b7d6-4e69-bbc2-a6695e2e424f",
        "hobbyId": 2,
        "startDate": "2024-08-15T14:00:00",
        "endDate": "2024-08-15T17:00:00",
        "location": "Art Studio",
        "capacity": 15,
        "participantIds": null,
        "status": 0
    },
]
```

#### 🔹 Example curl  
```bash
curl -X GET https://api.example.com/api/event/user/USER_GUID   -H "Authorization: Bearer YOUR_JWT_TOKEN"
```

---

### 3. Get My Events *(User Only)*  
**`GET /api/event/myevents`**  

#### 📌 Description  
Retrieves all events created by the currently logged-in user.

#### 🔹 Authentication  
- **Role:** `user`

#### 🔹 Example curl  
```bash
curl -X GET https://api.example.com/api/event/myevents   -H "Authorization: Bearer YOUR_JWT_TOKEN"
```

---

### 4. Get Event by ID *(Admin Only)*  
**`GET /api/event/{id}`**  

#### 📌 Description  
Retrieves details of a single event of the user int the params.

#### 🔹 Path Parameters  
| Name | Type | Required | Description |
|------|------|----------|-------------|
| id   | int  | Yes      | Event ID |

#### 🔹 Example curl  
```bash
curl -X GET https://api.example.com/api/event/42   -H "Authorization: Bearer YOUR_JWT_TOKEN"
```

---

### 5. Add Participant to Event *(Authenticated)*  
**`PATCH /api/event/add/{id}/{participantId}`**  

#### 📌 Description  
Adds a participant to the event. 
Participants can be added to event only by the admin and the owner of the event.

#### 🔹 Path Parameters  
| Name         | Type | Required | Description |
|--------------|------|----------|-------------|
| id           | int  | Yes      | Event ID |
| participantId| GUID | Yes      | Participant's user ID |

#### 🔹 Example curl  
```bash
curl -X PATCH https://api.example.com/api/event/add/42/PARTICIPANT_GUID   -H "Authorization: Bearer YOUR_JWT_TOKEN"
```

---

### 6. Remove Participant to Event *(Authenticated)*  
**`PATCH /api/event/remove/{id}/{participantId}`**  

#### 📌 Description  
Removes a participant to the event. 
Participants can be removed from event only by the admin and the owner of the event.

#### 🔹 Path Parameters  
| Name         | Type | Required | Description |
|--------------|------|----------|-------------|
| id           | int  | Yes      | Event ID |
| participantId| GUID | Yes      | Participant's user ID |

#### 🔹 Example curl  
```bash
curl -X PATCH https://api.example.com/api/event/remove/42/PARTICIPANT_GUID   -H "Authorization: Bearer YOUR_JWT_TOKEN"
```

---

### 7. Create Event *(Authenticated)*  
**`POST /api/event`**  

#### 📌 Description  
Creates a new event for the authenticated user.

#### 🔹 Authentication  
- Any authenticated user

#### 🔹 Request Body Schema
```json
[
    {
    "title": "Hiking Trip",
    "description": "Exploring the mountains",
    "hobbyId": 1,
    "startDate": "2026-12-21T17:14:30",
    "endDate": "2026-12-22T17:14:30",
    "location": "Mountain Park",
    "capacity": 30
    }
]
```

#### 🔹 Example curl  
```bash
curl -X POST https://api.example.com/api/event   -H "Authorization: Bearer YOUR_JWT_TOKEN"   -H "Content-Type: application/json"   -d '{
    "title": "Hiking Trip",
    "description": "Exploring the mountains",
    "hobbyId": 1,
    "startDate": "2025-09-12T10:00:00Z",
    "endDate": "2025-09-12T18:00:00Z",
    "location": "Mountain Park",
    "capacity": 10,
  }'
```

---

### 8. Update Event *(Authenticated)*  
**`PATCH /api/event/{id}`**  

#### 📌 Description  
Updates an existing event. Only the event creator and an admin can update. 

#### 🔹 Path Parameters  
| Name | Type | Required | Description |
|------|------|----------|-------------|
| id   | int  | Yes      | Event ID |

#### 🔹 Request Body Schema  
```json
[
  {
    "id":"integer",
    "title": "string",
    "description": "string",
    "ownerId": "Guid",
    "hobbyId": "integer",
    "startDate": "DateTime",
    "endDate": "DateTime",
    "location": "string",
    "capacity": "integer",
    "status": "integer"
}
]
```

#### 🔹 Example curl  
```bash
curl -X PATCH https://api.example.com/api/event/42   -H "Authorization: Bearer YOUR_JWT_TOKEN"   -H "Content-Type: application/json"   -d '{
    "id":23,
    "title": ""Hiking Trip",
    "description": "Exploring the mountains",
    "ownerId": "7E61F925-B7D6-4E69-BBC2-A6695E2E424F",
    "hobbyId": 2,
    "startDate": "2026-12-21T17:14:30",
    "endDate": "2026-12-22T17:14:30",
    "location": "Mountain Park",
    "capacity": 10,
    "participantIds": ["7E61F925-B7D6-4E69-BBC2-A6695E2E424F"],
    "status": 3
  }'
```

---

### 9. Delete Event *(User/Admin)*  
**`DELETE /api/event/{id}`**  


#### 📌 Description  
Deletes an event. Only the creator or an admin can delete.

#### 🔹 Path Parameters  
| Name | Type | Required | Description |
|------|------|----------|-------------|
| id   | int  | Yes      | Event ID |

#### 🔹 Example curl  
```bash
curl -X DELETE https://api.example.com/api/event/42   -H "Authorization: Bearer YOUR_JWT_TOKEN"
```

---

## 🔒 Authentication Notes
- All protected routes require a JWT token:
```
Authorization: Bearer <token>
```
- Tokens are obtained after logging in through the authentication service.
