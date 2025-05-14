# ASP.NET Core + Nuxt 3 Authentication Template

## Overview
A modern authentication template built with ASP.NET Core backend and Nuxt 3 frontend. Features secure JWT authentication with HTTP-only cookies, role-based authorization, and Google OAuth integration.

## Features
- 🔒 **Secure Authentication**: JWT tokens stored in HTTP-only cookies
- 🔄 **Refresh Token Rotation**: Automatic token refresh with secure storage
- 👤 **Role-Based Authorization**: Three-tier user roles (Basic, Premium, Admin)
- 🌐 **Google OAuth**: Social login integration
- ⚡ **Modern Stack**: ASP.NET Core 8 + Entity Framework Core + Nuxt 3 + Vue 3 + Pinia
- 💅 **Polished UI**: Responsive design using Vuetify 3

## Architecture
- **Backend**: ASP.NET Core with Identity Framework, JWT authentication, and role-based policies
- **Frontend**: Nuxt 3 with Vue 3 components, Pinia state management, and Vuetify UI library
- **Database**: SQL Server for user data and refresh token storage

## Getting Started

### Prerequisites
- .NET 8 SDK
- Node.js v18+ and npm
- SQL Server (or SQL Server Express)
- Google OAuth credentials (for social login)

### Backend Setup
1. Set up user secrets for the backend:
cd backend
dotnet user-secrets init
dotnet user-secrets set "Authentication:Google:ClientId" "your-google-client-id"
dotnet user-secrets set "Authentication:Google:ClientSecret" "your-google-client-secret"
dotnet user-secrets set "JWT:SecretKey" "your-secret-key"
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "your-connection-string"

2. Run the backend:
### Frontend Setup
1. Install dependencies:
cd frontend
npm install
2. Run the frontend:
npm run dev


## Authentication Flow
- Registration creates a new user in the database
- Login issues JWT access token and refresh token stored in HTTP-only cookies
- Protected routes/API endpoints check the JWT token automatically
- Token refresh happens automatically when the access token expires
- Role-based authorization restricts access based on user roles

## License
[MIT License](LICENSE)
