<<<<<<< HEAD
# Bank Management System

A Windows Forms application for managing bank operations including customer accounts, cash transactions, and employee management.

## Features

- **Customer Account Management**: Create and manage customer accounts
- **Cash In/Out Transactions**: Handle deposits and withdrawals
- **Transaction History**: View complete transaction records
- **Employee Management**: Manage bank employees
- **Secure Login**: Authentication system for users

## Technologies Used

- C# .NET Framework 4.8
- Windows Forms
- MySQL Database (MySql.Data 9.3.0)
- iTextSharp for PDF generation

## Prerequisites

- Visual Studio 2017 or later
- .NET Framework 4.8
- MySQL Server
- NuGet Package Manager

## Database Setup

1. Import the `bank_system.sql` file into your MySQL database
2. Update the database connection string in your configuration

## Installation

1. Clone this repository
2. Open `BankManagementSystem.sln` in Visual Studio
3. Restore NuGet packages
4. Build the solution
5. Run the application

## NuGet Packages

- MySql.Data 9.3.0
- iTextSharp 5.5.13.4
- Google.Protobuf 3.30.0
- BouncyCastle.Cryptography 2.5.1
- And other dependencies

## License

This project is for educational purposes.
=======
# 🏦 Bank Management System

A **Bank Management System** built with **C# and .NET (Visual Studio)** to manage core banking operations such as customer accounts, transactions, and basic administration. This project is suitable for learning, academic use, or as a foundation for a larger banking application.

---

## ✨ Features

* 👤 Customer account management (create, update, delete)
* 💰 Deposit and withdrawal operations
* 🔁 Fund transfer between accounts
* 📄 Transaction history tracking
* 🔐 Basic validation and error handling
* 🧩 Clean project structure using Visual Studio Solution

---

## 🛠️ Technologies Used

* **Language:** C#
* **Framework:** .NET
* **IDE:** Visual Studio 2022+
* **Project Type:** Console / Desktop Application (C#)

---

## 📂 Project Structure

```
BankManagementSystem.sln
│
├── .vs/
├── packages/
├── bank_system.sql          # database script
│
└── BankManagementSystem/
    ├── Program.cs
    ├── Models/
    ├── Services/
    ├── BankManagementSystem.csproj
```

---

## 🚀 Getting Started

### Prerequisites

* Windows OS
* Visual Studio 2022 or later
* .NET SDK installed

### Installation

1. **Clone the repository**

   ```bash
   git clone https://github.com/patmolsakda/BankManagementSystem.git
   ```

2. **Open the solution**

   * Double-click `BankManagementSystem.sln`
   * Or open via **Visual Studio → Open a project or solution**

3. **Build & Run**

   * Press `Ctrl + Shift + B` to build
   * Press `F5` to run the application

---

## 🧪 Example Usage

* Create a new bank account
* Deposit money into the account
* Withdraw funds
* Transfer money between two accounts
* View transaction history

---

## 🔑 Demo Login (For Testing Only)

Use the following default credentials to access the system:

* **Username:** admin
* **Password:** 1234

> ⚠️ These credentials are for demo/testing purposes only. Please change them before using the system in production.

---

## 🗄️ Database

* The project uses a **local database file** for storing users, accounts, and transactions.
* Database file is included in the project directory (e.g. `.mdf` / `.db`).
* Make sure the database file path is correctly configured in the application before running.

---

## 📌 Future Improvements

* Add authentication & role-based access
* Integrate database (SQL Server / MySQL)
* Add GUI (WinForms / WPF)
* Implement logging and reporting
* Unit testing

---

## 🤝 Contributing

Contributions are welcome!

1. Fork the project
2. Create a new branch (`feature/new-feature`)
3. Commit your changes
4. Open a Pull Request

---

## 📄 License

This project is licensed under the **MIT License**.

---

## 👤 Author

GitHub: https://github.com/patmolsakda/BankManagementSystem.git

---

⭐ If you like this project, feel free to give it a star on GitHub!
>>>>>>> 03f1a739c549e0477ea8eb488150ff46d1ec346a
