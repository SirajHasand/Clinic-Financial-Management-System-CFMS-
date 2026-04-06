# 🚀 🔷 Project Plan: Financial Medical Management System

## 🎯 Goal

Build a **Clean Architecture backend system** using:

- .NET (10.0.102)
- PostgreSQL

### Covers:
- Patients
- Doctors
- Drugs (Inventory)
- Sales
- Expenses
- Loans (customers & external)
- Financial tracking

---

## 🧱 STEP 1: Define Modules (VERY IMPORTANT)

We split the system into modules (features):

### 👨‍⚕️ Core Modules
- Patients  
- Doctors  
- Drugs (Inventory)  
- Suppliers  

### 💰 Financial Modules
- Sales (Daily selling)  
- Expenses (Daily costs)  
- Customer Loans (Debt)  
- Payments (incoming/outgoing)  
- Reports (profit, loss, daily summary)  

---

## 🏗️ STEP 2: Clean Architecture Structure

We will follow **Clean Architecture** .

### 📂 Project Structure

```bash
src/
├── Domain
├── Application
├── Infrastructure
├── WebAPI
```

### 🔹 1. Domain Layer (Core Logic)
- Entities (Patient, Drug, Sale, etc.)
- Enums
- Interfaces (Repositories)

> ❗ NO database, NO external libraries

---

### 🔹 2. Application Layer
- Use Cases (Services)
- DTOs
- Interfaces (IService)
- Validation

---

### 🔹 3. Infrastructure Layer
- Database (PostgreSQL)
- Entity Framework Core
- Repository Implementations

---

### 🔹 4. WebAPI Layer
- Controllers
- API Endpoints
- Authentication (later)

---

## 🗄️ STEP 3: Database Design (PostgreSQL)

We will design tables like:

- Patients  
- Doctors  
- Drugs  
- Sales  
- SaleItems  
- Expenses  
- Loans  
- Payments  

> 👉 Don’t worry — we will design this step by step.

---

## ⚙️ STEP 4: Technologies

- .NET Web API  
- Entity Framework Core  
- PostgreSQL (via Npgsql)  
- Swagger (API testing)  

---

## 🪜 STEP 5: Development Steps (IMPORTANT ORDER)

We will NOT jump randomly. Follow this order:

### ✅ Phase 1: Setup
- Create solution  
- Add projects (Domain, Application, Infrastructure, API)  
- Setup Clean Architecture references  

---

### ✅ Phase 2: Core Entities
Create entities:
- Patient  
- Drug  
- Doctor  

---

### ✅ Phase 3: Database
- Setup PostgreSQL connection  
- Configure EF Core  
- Run migrations  

---

### ✅ Phase 4: Features (One by One)

Build modules step by step:

- Patients API  
- Drugs API  
- Doctors API  
- Sales  
- Expenses  
- Loans  

---

### ✅ Phase 5: Financial Logic
- Profit calculation  
- Loan tracking  
- Daily reports  

---

### ✅ Phase 6: Advanced
- Authentication (JWT)  
- Role-based access (Admin, Staff)  
- Logging  
- Validation (FluentValidation)  

---


