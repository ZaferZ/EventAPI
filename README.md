[EventAPI_README.md](https://github.com/user-attachments/files/21728354/EventAPI_README.md)
# 📅 Event API Documentation

## Overview
The **Event API** allows authenticated users to create, manage, and participate in events. It also provides admin-level endpoints for managing events across all users.

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
    "id": "integer",
    "title": "string",
    "description": "string",
    "location": "string",
    "capacity": "integer",
    "startDate": "string (ISO 8601)",
    "endDate": "string (ISO 8601)",
    "hobbyId": "integer"
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
    "id": 42,
    "title": "Hiking Trip",
    "description": "Exploring the mountains"
  }
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

### 4. Get Event by ID *(User/Admin)*  
**`GET /api/event/{id}`**  

#### 📌 Description  
Retrieves details of a single event.

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
**`PATCH /api/event/{id}/{participantId}`**  

#### 📌 Description  
Adds a participant to the event.

#### 🔹 Path Parameters  
| Name         | Type | Required | Description |
|--------------|------|----------|-------------|
| id           | int  | Yes      | Event ID |
| participantId| GUID | Yes      | Participant's user ID |

#### 🔹 Example curl  
```bash
curl -X PATCH https://api.example.com/api/event/42/PARTICIPANT_GUID   -H "Authorization: Bearer YOUR_JWT_TOKEN"
```

---

### 6. Create Event *(Authenticated)*  
**`POST /api/event`**  

#### 📌 Description  
Creates a new event for the authenticated user.

#### 🔹 Authentication  
- Any authenticated user

#### 🔹 Request Body Schema  
```json
{
  "title": "string",
  "description": "string",
  "startDate": "string (ISO 8601)",
  "endDate": "string (ISO 8601)",
  "location": "string",
  "capacity": "integer",
  "hobbyId": "integer"
}
```

#### 🔹 Example curl  
```bash
curl -X POST https://api.example.com/api/event   -H "Authorization: Bearer YOUR_JWT_TOKEN"   -H "Content-Type: application/json"   -d '{
    "title": "Hiking Trip",
    "description": "Exploring the mountains",
    "startDate": "2025-09-12T10:00:00Z",
    "endDate": "2025-09-12T18:00:00Z",
    "location": "Mountain Park",
    "capacity": 10,
    "hobbyId": 3
  }'
```

---

### 7. Update Event *(Authenticated)*  
**`PATCH /api/event/{id}`**  

#### 📌 Description  
Updates an existing event. Only the event creator can update.

#### 🔹 Path Parameters  
| Name | Type | Required | Description |
|------|------|----------|-------------|
| id   | int  | Yes      | Event ID |

#### 🔹 Request Body Schema  
```json
{
  "id": "integer",
  "title": "string",
  "description": "string",
  "capacity": "integer"
}
```

#### 🔹 Example curl  
```bash
curl -X PATCH https://api.example.com/api/event/42   -H "Authorization: Bearer YOUR_JWT_TOKEN"   -H "Content-Type: application/json"   -d '{
    "id": 42,
    "title": "Updated Hiking Trip",
    "description": "New route planned",
    "capacity": 15
  }'
```

---

### 8. Delete Event *(User/Admin)*  
**`DELETE /api/event/{id}`**  

#### 📌 Description  
Deletes an event. Only the creator or an admin can delete.

#### 🔹 Example curl  
```bash
curl -X DELETE https://api.example.com/api/event/42   -H "Authorization: Bearer YOUR_JWT_TOKEN"
```

---

### 9. Authentication Test *(Authenticated)*  
**`GET /api/event/auth-test`**  

#### 📌 Description  
Returns a success message if the user is authenticated.

#### 🔹 Example curl  
```bash
curl -X GET https://api.example.com/api/event/auth-test   -H "Authorization: Bearer YOUR_JWT_TOKEN"
```

---

### 10. Admin Authentication Test *(Admin Only)*  
**`GET /api/event/auth-admin`**  

#### 📌 Description  
Returns a success message if the user is an admin.

#### 🔹 Example curl  
```bash
curl -X GET https://api.example.com/api/event/auth-admin   -H "Authorization: Bearer YOUR_JWT_TOKEN"
```

---

## 🔒 Authentication Notes
- All protected routes require a JWT token:
```
Authorization: Bearer <token>
```
- Tokens are obtained after logging in through the authentication service.
