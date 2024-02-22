# Diffing Service

This project implements a simple HTTP-based diffing service that allows users to compare two sets of base64 encoded binary data. The service exposes two endpoints for providing the left and right data respectively, and a third endpoint for retrieving the comparison results.

## ENDPOINTS
### 1. Set Left Data
- **Endpoint:** `<host>/v1/diff/<ID>/left`
- **Method:** `PUT`
- **Payload:** JSON containing base64 encoded binary data
  ```json
  {
    "data": "AAAAAA=="
  }

### 2. Set Right Data
- **Endpoint:** `<host>/v1/diff/<ID>/right`
- **Method:** `PUT`
- **Payload:** JSON containing base64 encoded binary data
  ```json
  {
    "data": "AAAAAA=="
  }

### 3. Get Diff Results
- **Endpoint:** `<host>/v1/diff/<ID>`
- **Method:** `GET`
- **Response:** JSON containing the comparison results
  ```json
  {
    "diffResultType": "Equals"
  }

