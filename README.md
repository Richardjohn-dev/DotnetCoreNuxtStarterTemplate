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

## 🔧 Backend Setup

1. Set up user secrets:

    ```bash
    cd backend
    dotnet user-secrets init
    dotnet user-secrets set "Authentication:Google:ClientId" "your-google-client-id"
    dotnet user-secrets set "Authentication:Google:ClientSecret" "your-google-client-secret"
    dotnet user-secrets set "JWT:SecretKey" "your-secret-key"
    ```

2. Run the backend (replace with your actual run command if needed):

    ```bash
    dotnet run
    ```

---

## 🌐 Frontend Setup

1. Install dependencies:

    ```bash
    cd frontend
    npm install
    ```

2. Set up local HTTPS certificates:

    - Create a folder called `ssl` inside the `frontend` directory:

        ```bash
        mkdir ssl
        ```

    - Place your certificate files in the `ssl` folder:
        - `localhost+2.pem`
        - `localhost+2-key.pem`
     
          

    > ⚠️ If you don't have these files, you can generate them using [mkcert](https://github.com/FiloSottile/mkcert).  
    > Install it via [Chocolatey](https://chocolatey.org/) on Windows:
    >
    > ```bash
    > choco install mkcert
    > mkcert -install
    > mkcert -key-file ssl/localhost+2-key.pem -cert-file ssl/localhost+2.pem localhost 127.0.0.1 ::1
    > ```

3. Run the frontend:

    ```bash
    npm run dev
    ```


## Authentication Flow
- Registration creates a new user in the database
- Login issues JWT access token and refresh token stored in HTTP-only cookies
- Protected routes/API endpoints check the JWT token automatically
- Token refresh happens automatically when the access token expires
- Role-based authorization restricts access based on user roles

## License
[MIT License](LICENSE)
