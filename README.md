
# 🏘️ Community Issue Tracker

A simple, **role-based web application** that allows local residents to report community issues, authorities to manage them, and administrators to verify authority accounts. Built for hackathon purposes with a focus on **speed, usability, and clarity**.  

[Live Repository](https://github.com/Alan21303/community-issue-tracker)

---

## 📌 Table of Contents
- [✨ Features](#-features)
- [🛡️ Roles & Permissions](#-roles--permissions)
- [🛠️ Tech Stack](#-tech-stack)
- [💻 Installation](#-installation)
- [🚀 Usage](#-usage)
- [📂 Project Structure](#-project-structure)
- [🔮 Future Enhancements](#-future-enhancements)
- [📄 License](#-license)

---

## ✨ Features

- **User Reports**: Submit issues with title, description, category, location, images/videos, and tags.
- **Role-Based Access**: Clear separation of Users, Authorities, and Admins.
- **Status Management**: Authorities can update report status (`Submitted → Reviewed → Resolved`).
- **Tags & Filtering**: Add custom tags, search/filter by tags, category, or status.
- **Admin Verification**: Admins approve authorities before they can access reports.
- **Dashboard**: View all reports, filter and sort dynamically.
- **Media Support**: Upload images or videos along with a report.

---

## 🛡️ Roles & Permissions

| Role      | Permissions                                                                 |
|-----------|-----------------------------------------------------------------------------|
| **User**      | Register/Login, create reports, view all reports, filter reports          |
| **Authority** | Register/Login (after admin verification), update report status, view reports |
| **Admin**     | Login, verify pending authorities, view all reports                       |

---

## 🛠️ Tech Stack

- **Frontend**: HTML, CSS, JavaScript
- **Backend**: ASP.NET Core Web API
- **Database**: PostgreSQL / SQL Server (via Entity Framework Core)
- **Authentication**: JWT Tokens
- **Media Storage**: Local `wwwroot/uploads` folder
- **Tools**: Visual Studio / VS Code

---

## 💻 Installation

### 1. Clone the repository
```bash
git clone https://github.com/Alan21303/community-issue-tracker.git
cd community-issue-tracker
````

### 2. Configure the Database

Update `appsettings.json` with your PostgreSQL connection string:

```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=communitytracker;Username=postgres;Password=yourpassword"
}
```

### 3. Run Migrations

```bash
dotnet ef database update
```

### 4. Seed Admin User (optional)

You can add an admin directly via code or SQL:

```csharp
new User {
    Name = "Super Admin",
    Email = "admin@tracker.com",
    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
    Role = "Admin",
    IsVerified = true
}
```

### 5. Run the Application

```bash
dotnet run
```

### 6. Access Frontend

Open browser and navigate to:
[http://localhost:5000/index.html](http://localhost:5000/index.html)

---

## 🚀 Usage

### User

* Register and submit new reports with images/videos.
* View all reports, filter by tags, category, or status.

### Authority

* Must be verified by an admin to log in.
* View submitted reports and update status (`Submitted → Reviewed → Resolved`).

### Admin

* Approve pending authorities.
* View and manage all reports.

---


---

## 🔮 Future Enhancements

* Notifications when report status changes
* Commenting system for users and authorities
* Map integration to visualize issues
* Like or vote system for important community issues
* Email verification for users/authorities

---

## 📄 License

This project is licensed under the **MIT License**.

```
```
