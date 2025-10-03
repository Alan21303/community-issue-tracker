Community Issue Tracker

A simple, role-based web application that allows local residents to report community issues, authorities to manage them, and administrators to verify authority accounts. Built for hackathon purposes with a focus on speed, usability, and clarity.

Live Repository: https://github.com/Alan21303/community-issue-tracker

Table of Contents

Features

Roles & Permissions

Tech Stack

Installation

Usage

Project Structure

Future Enhancements

License

Features

User Reports: Users can submit issues with title, description, category, location, images/videos, and tags.

Role-Based Access: Clear separation of Users, Authorities, and Admins.

Status Management: Authorities can update report status (Submitted → Reviewed → Resolved).

Tags & Filtering: Add custom tags, search/filter by tags, category, or status.

Admin Verification: Admins approve authorities before they can access reports.

Dashboard: View all reports, filter and sort dynamically.

Media Support: Upload images or videos along with a report.

Roles & Permissions
Role Permissions
User Register/Login, create reports, view all reports, filter reports
Authority Register/Login after admin verification, update report status, view reports
Admin Login, verify pending authorities, view all reports
Tech Stack

Frontend: HTML, CSS, JavaScript

Backend: ASP.NET Core Web API

Database: PostgreSQLSQL Server (via Entity Framework Core)

Authentication: JWT Tokens

Media Storage: Local wwwroot/uploads folder

Tools: Visual Studio / VS Code

Installation

Clone the repository

git clone https://github.com/Alan21303/community-issue-tracker.git
cd community-issue-tracker

Configure the Database

Update appsettings.json with your SQL Server connection string:

"ConnectionStrings": {
"DefaultConnection": "Host=localhost;Port=5432;Database=communitytracker;Username=postgres;Password=yourpassword"
}

Run Migrations

dotnet ef database update

Seed Admin User (optional via direct SQL or code):

new User {
Name = "Super Admin",
Email = "admin@tracker.com",
PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
Role = "Admin",
IsVerified = true
}

Run the Application

dotnet run

Access Frontend

Open browser and navigate to:

http://localhost:5000/index.html

Usage

Register a User or Authority

Authority must wait for Admin verification before logging in.

Login

Redirects to dashboard based on role.

User Dashboard

Submit new reports with images/videos

View all reports and filter by tags, category, or status

Authority Dashboard

View submitted reports

Update status (Submitted → Reviewed → Resolved)

Admin Dashboard

View pending authorities

Approve authority accounts for access
Future Enhancements

Notifications when report status changes

Commenting system for users and authorities

Map integration to visualize issues

Like or vote system for important community issues

Email verification for users/authorities
