# Gemini Banana REST API Integration Specification

This document details the REST API endpoints, communication protocols, request payloads, response schemas, and image processing details utilized by the `BananaBridge` plugin to connect to the Google GenAI Gateway.

---

## 1. REST Endpoint Reference

The client communicates directly with the Google Generative Language REST Gateway (version `v1beta`).

* **HTTP Method:** `POST`
* **URL Format:** `https://generativelanguage.googleapis.com/v1beta/{modelName}:generateContent?key={apiKey}`
* **Default Model (`modelName`):** `models/gemini-2.5-flash-image`
* **Request Content-Type:** `application/json`

---

## 2. Request Payload Schema

The request payload conforms to the Google GenAI multi-turn conversational API contract, customized for image-to-image and inpainting tasks. 

### 2.1 JSON Schema Structure

```json
{
  "contents": [
    {
      "role": "user | model",
      "parts": [
        {
          "text": "Instruction or prompt string (optional)"
        },
        {
          "inlineData": {
            "mimeType": "image/png",
            "data": "Base64EncodedImageDataString (optional)"
          }
        }
      ]
    }
  ],
  "generationConfig": {
    "temperature": 0.4,
    "responseMimeType": "image/png"
  }
}
```

### 2.2 Parameters Breakdown

| Field | Type | Description |
| :--- | :--- | :--- |
| `contents` | Array | Chronological list of chat turns representing session history. |
| `role` | String | Must be `"user"` for instructions/uploads or `"model"` for generated outputs. |
| `parts` | Array | Interactive segments containing text prompts or inline media data. |
| `inlineData` | Object | Binary media payload structure. |
| `inlineData.mimeType` | String | Set strictly to `"image/png"`. |
| `inlineData.data` | String | Standard Base64 string of the image without headers. |
| `generationConfig` | Object | Optional tuning configurations. |
| `generationConfig.temperature` | Float | Controls creativity vs coherence. Defaults to `0.4` for editing precision. |
| `generationConfig.responseMimeType`| String | Tells the model to return binary image data (`"image/png"`) instead of text. |

---

## 3. Conversational State & Multi-Turn Schema

To maintain conversational state across multiple requests (e.g., "Add a dog", "make it run", "change the background"), the plugin preserves and transmits the full list of previous chat turns.

### 3.1 Initial Turn (Turn 1)
User uploads the source crop, the selection mask, and the prompt instruction:
```json
{
  "role": "user",
  "parts": [
    { "text": "Make the background look starry" },
    { "inlineData": { "mimeType": "image/png", "data": "<SourceImageBase64>" } },
    { "inlineData": { "mimeType": "image/png", "data": "<MaskImageBase64>" } }
  ]
}
```

### 3.2 Model Response (Turn 2)
The API returns the edited image:
```json
{
  "role": "model",
  "parts": [
    { "text": "[Generated Image Result]" }
  ]
}
```

### 3.3 Second Turn (Turn 3)
User requests a refinement. The request body contains the complete history (Turn 1 & Turn 2) followed by the new instruction:
```json
{
  "role": "user",
  "parts": [
    { "text": "Add some nebula clouds in the corners" },
    { "inlineData": { "mimeType": "image/png", "data": "<LatestImageBase64>" } },
    { "inlineData": { "mimeType": "image/png", "data": "<MaskImageBase64>" } }
  ]
}
```

---

## 4. Response Parsing Strategy

The API client supports two distinct output formats to handle different response configurations from the Gemini API gateway.

### 4.1 Format A: Binary Inline Response
If the API returns the image as binary data directly inside the part structure:
```json
{
  "candidates": [
    {
      "content": {
        "parts": [
          {
            "inlineData": {
              "mimeType": "image/png",
              "data": "iVBORw0KGgoAAAANSUhEUgAA..."
            }
          }
        ]
      }
    }
  ]
}
```
The parser directly retrieves `data` and converts the base64 string back into a `System.Drawing.Bitmap`.

### 4.2 Format B: Base64 Text Response
If the API returns the image wrapped inside a text response block:
```json
{
  "candidates": [
    {
      "content": {
        "parts": [
          {
            "text": "```\ndata:image/png;base64,iVBORw0KGgoAAAANSUhEUgAA...\n```"
          }
        ]
      }
    }
  ]
}
```
The client employs a sanitization utility (`CleanBase64`) to extract the raw data:
1. Strips out markdown code blocks (```` ``` ````).
2. Strips out Data URI scheme prefixes (e.g., `data:image/png;base64,`).
3. Strips all carriage returns (`\r`), line feeds (`\n`), and spaces.
4. Converts the resulting clean base64 string back to a `Bitmap`.

