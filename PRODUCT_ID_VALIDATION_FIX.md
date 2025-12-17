# Product ID Validation - Testing Guide

## ? Fixed: Invalid Product ID Handling

The `GET /api/product/{id}` endpoint now properly validates product IDs and returns clear error messages.

---

## ?? Test Scenarios

### **Test 1: Valid Product ID (Success)**

**Request:**
```http
GET http://localhost:5000/api/product/1
X-API-Key: MySecretApiKey123!@#
```

**Expected Response (200 OK):**
```json
{
  "id": 1,
  "name": "Laptop",
  "productType": "Electronics",
  "colours": ["Black", "Silver"]
}
```

---

### **Test 2: Product ID = 0 (Invalid)**

**Request:**
```http
GET http://localhost:5000/api/product/0
X-API-Key: MySecretApiKey123!@#
```

**Expected Response (400 Bad Request):**
```json
{
  "message": "Invalid product ID. Product ID must be a positive number greater than 0. Received: 0",
  "statusCode": 400,
  "details": null,
  "errors": null,
  "traceId": "0HMVBBBBB01-00000001",
  "timestamp": "2025-01-16T12:00:00Z"
}
```

---

### **Test 3: Negative Product ID (Invalid)**

**Request:**
```http
GET http://localhost:5000/api/product/-5
X-API-Key: MySecretApiKey123!@#
```

**Expected Response (400 Bad Request):**
```json
{
  "message": "Invalid product ID. Product ID must be a positive number greater than 0. Received: -5",
  "statusCode": 400,
  "traceId": "0HMVBBBBB01-00000002",
  "timestamp": "2025-01-16T12:00:00Z"
}
```

---

### **Test 4: Text Instead of Number (Invalid)**

**Request:**
```http
GET http://localhost:5000/api/product/abc
X-API-Key: MySecretApiKey123!@#
```

**Expected Response (400 Bad Request):**
```json
{
  "message": "Invalid product ID format. 'abc' is not a valid number. Please provide a valid positive integer.",
  "statusCode": 400,
  "traceId": "0HMVBBBBB01-00000003",
  "timestamp": "2025-01-16T12:00:00Z"
}
```

---

### **Test 5: Special Characters (Invalid)**

**Request:**
```http
GET http://localhost:5000/api/product/@#$
X-API-Key: MySecretApiKey123!@#
```

**Expected Response (400 Bad Request):**
```json
{
  "message": "Invalid product ID format. '@#$' is not a valid number. Please provide a valid positive integer.",
  "statusCode": 400,
  "traceId": "0HMVBBBBB01-00000004",
  "timestamp": "2025-01-16T12:00:00Z"
}
```

---

### **Test 6: Decimal Number (Invalid)**

**Request:**
```http
GET http://localhost:5000/api/product/1.5
X-API-Key: MySecretApiKey123!@#
```

**Expected Response (400 Bad Request):**
```json
{
  "message": "Invalid product ID format. '1.5' is not a valid number. Please provide a valid positive integer.",
  "statusCode": 400,
  "traceId": "0HMVBBBBB01-00000005",
  "timestamp": "2025-01-16T12:00:00Z"
}
```

---

### **Test 7: Very Large Number (Invalid)**

**Request:**
```http
GET http://localhost:5000/api/product/999999999999999
X-API-Key: MySecretApiKey123!@#
```

**Expected Response (400 Bad Request):**
```json
{
  "message": "Invalid product ID format. '999999999999999' is not a valid number. Please provide a valid positive integer.",
  "statusCode": 400,
  "traceId": "0HMVBBBBB01-00000006",
  "timestamp": "2025-01-16T12:00:00Z"
}
```

---

### **Test 8: Product Not Found (Valid ID, Not Exists)**

**Request:**
```http
GET http://localhost:5000/api/product/9999
X-API-Key: MySecretApiKey123!@#
```

**Expected Response (404 Not Found):**
```json
{
  "message": "Product with ID 9999 was not found.",
  "statusCode": 404,
  "traceId": "0HMVBBBBB01-00000007",
  "timestamp": "2025-01-16T12:00:00Z"
}
```

---

## ?? What Changed

### **Before:**
- ID = 0 ? Returned 404 or unexpected error
- ID = "abc" ? ASP.NET Core 400 with generic message
- No clear validation messages

### **After:**
- ? ID = 0 ? Clear message: "Product ID must be positive"
- ? ID = "abc" ? Clear message: "'abc' is not a valid number"
- ? Negative IDs ? Clear validation message
- ? Non-numeric input ? User-friendly error
- ? Consistent ErrorResponse format

---

## ?? Implementation Details

### **Route Change:**
```csharp
// Before: [HttpGet("{id:int:min(1)}")]
// After:  [HttpGet("{id}")]
```

### **Parameter Change:**
```csharp
// Before: public async Task<ActionResult> GetById([FromRoute] int id)
// After:  public async Task<ActionResult> GetById([FromRoute] string id)
```

### **Validation Logic:**
```csharp
// 1. Check if string is valid integer
if (!int.TryParse(id, out int productId))
{
    return BadRequest("Invalid format...");
}

// 2. Check if number is positive
if (productId <= 0)
{
    return BadRequest("Must be greater than 0...");
}

// 3. Check if product exists
var product = await _handler.GetProductById(productId);
if (product == null)
{
    return NotFound("Product not found...");
}
```

---

## ?? Error Response Structure

All errors now return consistent `ErrorResponse`:

```csharp
public class ErrorResponse
{
    public string Message { get; set; }        // User-friendly message
    public int StatusCode { get; set; }         // HTTP status code
    public string? Details { get; set; }        // Technical details (dev only)
    public IDictionary<string, string[]>? Errors { get; set; }  // Field errors
    public string? TraceId { get; set; }        // For debugging
    public DateTime Timestamp { get; set; }     // When error occurred
}
```

---

## ?? Postman Testing

### **Collection Setup:**

1. **Create Request:** "Get Product - Invalid ID 0"
   - URL: `{{base_url}}/api/product/0`
   - Method: GET
   - Headers: `X-API-Key: {{api_key}}`

2. **Create Request:** "Get Product - Text ID"
   - URL: `{{base_url}}/api/product/abc`
   - Method: GET
   - Headers: `X-API-Key: {{api_key}}`

3. **Create Request:** "Get Product - Negative ID"
   - URL: `{{base_url}}/api/product/-1`
   - Method: GET
   - Headers: `X-API-Key: {{api_key}}`

---

## ? Validation Flow

```
Request: GET /api/product/{id}
    ?
[Is id a valid integer?] ? No ? 400: "Invalid format"
    ? Yes
[Is id > 0?] ? No ? 400: "Must be positive"
    ? Yes
[Does product exist?] ? No ? 404: "Not found"
    ? Yes
Return 200 OK with product details
```

---

## ?? Benefits

1. ? **Clear Error Messages** - Users know exactly what's wrong
2. ? **Consistent Format** - All errors use ErrorResponse
3. ? **Proper Status Codes** - 400 for validation, 404 for not found
4. ? **Trace IDs** - Easy debugging with correlation
5. ? **User-Friendly** - Non-technical users understand the error

---

## ?? Quick cURL Tests

### **Test ID = 0:**
```bash
curl -H "X-API-Key: MySecretApiKey123!@#" http://localhost:5000/api/product/0
```

### **Test Text ID:**
```bash
curl -H "X-API-Key: MySecretApiKey123!@#" http://localhost:5000/api/product/abc
```

### **Test Negative ID:**
```bash
curl -H "X-API-Key: MySecretApiKey123!@#" http://localhost:5000/api/product/-5
```

### **Test Valid ID:**
```bash
curl -H "X-API-Key: MySecretApiKey123!@#" http://localhost:5000/api/product/1
```

---

**Status:** ? Fixed  
**All Invalid IDs Now Return Proper Validation Messages!** ??
