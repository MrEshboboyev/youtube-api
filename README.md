# 🎥 YouTube Content Creator API – Manage YouTube Creators Effortlessly 🚀  

![.NET 10](https://img.shields.io/badge/.NET%2010-blue?style=for-the-badge)  
![YouTube API](https://img.shields.io/badge/YouTube%20API-%F0%9F%8E%A5-red?style=for-the-badge)  
![Docker](https://img.shields.io/badge/Docker-%F0%9F%90%A6-blue?style=for-the-badge)  
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-%E2%9C%85-green?style=for-the-badge)  
![Automated Testing](https://img.shields.io/badge/Automated%20Testing-%F0%9F%94%A5-orange?style=for-the-badge)  

Welcome to the **YouTube Content Creator API**! This **powerful and scalable API** allows you to efficiently **manage YouTube creators**, track their content, and organize metadata—all while leveraging the latest **.NET 10** features.  

> **Why Use This API?**  
> - 🎯 **Automate YouTube Creator Management**  
> - 🚀 **High Performance & Scalable**  
> - 🔒 **Secure with Robust Error Handling**  
> - 🛠 **Optimized for Modern Web Development**  

---

## **🌟 Features**  

✅ **Retrieve All YouTube Content Creators** – Fetch a list of **all registered creators**.  
✅ **Get Creator Details by ID** – Retrieve **detailed creator metadata** by ID.  
✅ **Create New Content Creator** – Add **new creators** using their **YouTube channel ID**.  
✅ **Secure & Error-Handled API** – Ensures **valid input and prevents duplicates**.  
✅ **Docker-Ready** – Deploy effortlessly with **containerization**.  
✅ **Database-Powered** – Uses **PostgreSQL with Entity Framework Core** for **reliable data storage**.  
✅ **Automated Testing** – Implements **xUnit** for **unit and integration testing**.  

---

## **📂 Project Structure**  

📌 **src/Youtube.Api** – Main API logic and request handling.  
📌 **src/Application** – Business logic and validation.  
📌 **src/Infrastructure** – Database operations using **EF Core & PostgreSQL**.  
📌 **tests/Youtube.Api.Tests** – Unit and integration test cases.  
📌 **docker-compose.yml** – Sets up **PostgreSQL and API** in Docker.  

---

## **🚀 Getting Started**  

### **📌 Prerequisites**  
✅ [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)  
✅ [Docker](https://www.docker.com/get-started)  
✅ [PostgreSQL](https://www.postgresql.org/download/)  

### **Step 1: Clone the Repository**  
```bash
git clone https://github.com/yourusername/YouTubeContentCreatorAPI.git
cd YouTubeContentCreatorAPI
```

### **Step 2: Set Up the Database (Docker)**  
```bash
docker-compose up -d
```

### **Step 3: Run the API Locally**  
```bash
dotnet run --project src/Youtube.Api
```

---

## **🌍 API Endpoints**  

### **📋 Get All Content Creators**  
| Method | Endpoint | Description |
|--------|---------|-------------|
| **GET**  | `/api/content-creators` | Retrieves all content creators |

### **📌 Get Content Creator by ID**  
| Method | Endpoint | Description |
|--------|---------|-------------|
| **GET**  | `/api/content-creators/{id}` | Fetches creator details by ID |

### **➕ Create a New Content Creator**  
| Method | Endpoint | Description |
|--------|---------|-------------|
| **POST** | `/api/content-creators` | Adds a new creator |

### **Example Request (cURL)**  
```bash
curl -X GET "http://localhost:5000/api/content-creators" -H "accept: application/json"
```

---

## **🐳 Running with Docker**  

### **Step 1: Build & Run with Docker Compose**  
```bash
docker-compose up --build
```

### **Step 2: Access API & Documentation**  
🔹 **Swagger UI** – [http://localhost:5000/swagger](http://localhost:5000/swagger)  

> **🚀 The API is fully Dockerized, making deployment simple and scalable.**  

---

## **🛠 Configuration**  

Set up environment variables in **docker-compose.yml**:  

```yaml
services:
  youtube.api:
    environment:
      - ConnectionStrings__PostgresConnection=your-postgres-connection-string
```

---

## **🧪 Testing**  

### **Run Automated Tests**  
```bash
dotnet test
```

### **Manual API Testing**  
📌 **Use Postman or Swagger UI** to:  
✅ **Retrieve creators** → `/api/content-creators`  
✅ **Fetch creator by ID** → `/api/content-creators/{id}`  
✅ **Create a new creator** → `/api/content-creators (POST)`  

---

## **🎯 Why Use This Project?**  

✅ **Effortless YouTube Creator Management** – Automates **creator data handling**.  
✅ **Secure & Reliable API** – Prevents **errors & duplicate data**.  
✅ **Docker & Cloud Ready** – Deploy on **AWS, Azure, or Kubernetes**.  
✅ **Modern Development Stack** – Built with **.NET 10, PostgreSQL, and EF Core**.  

---

## **📜 License**  

This project is licensed under the **MIT License**. See [LICENSE](LICENSE) for details.  

---

## **📞 Contact**  

For feedback, contributions, or inquiries:  
📧 **Email**: [mreshboboyev@gmail.com](mailto:mreshboboyev@gmail.com)  
💻 **GitHub**: [MrEshboboyev](https://github.com/MrEshboboyev/youtube-api)  

---

🚀 **Effortlessly manage YouTube creators with YouTube Content Creator API!** Clone the repo & get started now! 
