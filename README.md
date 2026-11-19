# NZParadiseAPI-main

NZParadise is a full\-stack property listing and rental application built with **React, TypeScript, ASP.NET Core Web API, Entity Framework Core, SQL Server, and Azure Blob Storage**.

The project demonstrates end\-to\-end application development, including authentication, property management, image storage, location search, wishlists, messaging, and cloud\-ready architecture.

## Features

### Property Management

- Create property listings
- Edit existing properties
- Delete property listings
- Search and browse available properties
- View detailed property information
- Manage property availability and status
- Support multiple property types and property features

### Property Images

- Upload property images
- Associate multiple images with a property
- Remove existing property images
- Store image metadata in SQL Server
- Store image files using Blob Storage
- Use Azurite as the local Azure Blob Storage emulator

### Location & Maps

- Google Places location autocomplete
- Places API (New) integration
- New Zealand location filtering
- Google Maps property preview
- Google Maps Embed support

### Authentication & Users

- User registration
- User sign\-in
- JWT\-based authentication
- Protected routes
- User profile management
- User dashboard

### User Features

- Property wishlists
- Property enquiries
- User messaging
- My Listings management

---

# Technology Stack

## Frontend

- React
- TypeScript
- Material UI
- React Router
- React Hook Form
- Google Maps JavaScript API
- Google Places API (New)
- Maps Static API
- Maps Embed API

## Backend

- C\#
- ASP.NET Core Web API
- Entity Framework Core
- REST APIs
- JWT authentication
- Repository and service layers

## Data & Storage

- Microsoft SQL Server
- Entity Framework Core
- Azure Blob Storage
- Azurite for local Blob Storage development

## Development & Deployment

- Git
- GitHub
- Visual Studio Code
- Swagger / OpenAPI
- Azure\-ready configuration

---

# Application Architecture

                     NZParadise

                         \|

              \+\-\-\-\-\-\-\-\-\-\-\+\-\-\-\-\-\-\-\-\-\-\+

              \|                     \|

        React \+ TypeScript      ASP.NET Core

           Frontend               Web API

              \|                     \|

              \| REST / JSON         \|

              \+\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\->\|

                                    \|

                         \+\-\-\-\-\-\-\-\-\-\-\+\-\-\-\-\-\-\-\-\-\-\+

                         \|                     \|

                    SQL Server          Blob Storage

                    EF Core              Images

                                               \|

                                            Azurite

                                      (Local Development)

External Services

       \|

       \+\-\- Google Maps JavaScript API

       \+\-\- Places API (New)

       \+\-\- Maps Static API

       \+\-\- Maps Embed API

---

# Project Structure

The application is split into frontend and backend repositories.

## Frontend

src/

├── components/

├── config/

├── pages/

├── services/

├── theme/

├── types/

├── App.tsx

└── index.tsx

The frontend is responsible for:

- User interface
- Form validation
- Property management UI
- Authentication state
- Property search
- Location autocomplete
- Image selection
- API communication

## Backend

The ASP.NET Core API is responsible for:

- Authentication
- Authorization
- Business logic
- Property management
- Wishlist operations
- Messaging
- SQL Server persistence
- Image upload and deletion
- Blob Storage integration
- Audit logging

---

# Local Development Architecture

During local development:

React Client

[http://localhost:3000](http://localhost:3000)

        \|

        v

ASP.NET Core API

[http://localhost:99](http://localhost:99)

        \|

        \+\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-> SQL Server

        \|

        \+\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-\-> Azurite Blob Storage

                              127.0.0.1:10000

---

# Frontend Setup

## Prerequisites

Install:

- Node.js
- npm
- Git

Clone the frontend repository and install dependencies:

npm install

## Environment Configuration

Create a local .env file in the frontend project root:

REACT\_APP\_API\_URL=[http://localhost:99](http://localhost:99)

REACT\_APP\_GOOGLE\_MAPS\_API\_KEY=YOUR\_GOOGLE\_MAPS\_API\_KEY

Never commit .env, API keys, storage credentials, or other secrets to Git.

Start the frontend:

npm start

The application should be available at:

[http://localhost:3000](http://localhost:3000)

---

# Backend Setup

The backend is provided by the separate **NZParadise ASP.NET Core Web API** project.

For local development, configure the API to use SQL Server and Azurite.

Example database configuration:

Server=localhost

Database=NZParadiseDBDev

Trusted Connection=True

Start the API using:

dotnet run

Swagger can be used locally to inspect and test API endpoints.

---

# Local Blob Storage

NZParadise uses **Azurite** to emulate Azure Blob Storage during local development.

Example development configuration:

"AzureBlob": \{

  "ConnectionString": "UseDevelopmentStorage=true",

  "ContainerName": "nzparadise\-images"

\}

The default local Blob endpoint is:

[http://127.0.0.1:10000](http://127.0.0.1:10000)

Property images are stored in the:

nzparadise\-images

container.

For local browser\-based image testing, individual blobs can be configured with public read access through the local development storage environment.

---

# Google Maps Configuration

NZParadise integrates with Google Maps Platform for property locations.

The frontend uses:

- Maps JavaScript API
- Places API (New)
- Maps Static API
- Maps Embed API

For local development, configure the Google Maps browser key to allow:

[http://localhost:3000/](http://localhost:3000/)\*

The API key should be restricted to only the APIs required by this application.

The project uses the newer Google Places autocomplete APIs rather than relying on the legacy Places autocomplete implementation.

---

# Database

The application currently includes entities for:

Users

Properties

PropertyTypes

PropertyFeatures

Attachments

Wishlists

Messages

AuditLog

Relationships between property, attachment, message, wishlist, and user data are managed through SQL Server and Entity Framework Core.

---

# Security

Current security considerations include:

- JWT authentication
- Protected API endpoints
- Protected frontend routes
- Environment\-based configuration
- Google API key restrictions
- Secrets excluded from Git
- Server\-side storage credentials
- CORS configuration
- Property ownership checks

Sensitive values should never be committed to the repository.

---

# Engineering Roadmap

NZParadise is being developed beyond functional CRUD features toward a more production\-oriented engineering architecture.

## Testing

- [ ] Unit tests for business services
- [ ] Repository tests
- [ ] API integration tests
- [ ] Authentication and authorization tests
- [ ] React component tests
- [ ] End\-to\-end tests

## Backend Architecture

- [ ] Introduce service interfaces
- [ ] Improve dependency injection
- [ ] Reduce manual service construction
- [ ] Strengthen separation between API, application, and infrastructure layers
- [ ] Centralize exception handling
- [ ] Add consistent API validation
- [ ] Improve error\-response standards

## Security

- [ ] Strengthen resource ownership authorization
- [ ] Validate uploaded file types
- [ ] Enforce upload size limits
- [ ] Review JWT expiration and refresh strategy
- [ ] Move production secrets to secure cloud configuration
- [ ] Add security checks to CI/CD

## DevOps

- [ ] Automated frontend build
- [ ] Automated backend build
- [ ] Automated unit tests
- [ ] Automated integration tests
- [ ] Pull\-request validation
- [ ] Staging deployment
- [ ] Production deployment
- [ ] Deployment health validation

## Azure

- [ ] Deploy React frontend
- [ ] Deploy ASP.NET Core API
- [ ] Configure Azure SQL
- [ ] Configure Azure Blob Storage
- [ ] Configure production CORS
- [ ] Configure production secrets
- [ ] Add managed cloud identity where appropriate

## Observability

- [ ] Structured application logging
- [ ] Correlation IDs
- [ ] Centralized exception logging
- [ ] Health\-check endpoints
- [ ] Application Insights
- [ ] Performance monitoring

## Advanced Engineering

- [ ] Background notification processing
- [ ] Queue\-based asynchronous processing
- [ ] Event\-driven messaging
- [ ] Application caching
- [ ] Performance testing
- [ ] Load testing
- [ ] Resilience and retry policies

---

# Engineering Goals

NZParadise is not only a feature\-development project. The long\-term engineering goals are to demonstrate:

- Maintainable C\# and TypeScript
- Clear separation of concerns
- API design
- Relational database design
- Authentication and authorization
- Secure configuration management
- Cloud storage integration
- Automated testing
- CI/CD
- Cloud deployment
- Observability
- Resilience
- Technical decision\-making

The aim is to evolve the application from a working full\-stack system into a production\-style software engineering project.

---

# Repository Security

Local development artifacts should remain excluded from source control.

Examples include:

.env

node\_modules/

build/

\_\_blobstorage\_\_/

\_\_queuestorage\_\_/

\_\_azurite\_db\_\*.json

Never commit:

- Google Maps API keys
- Azure Storage access keys
- database credentials
- JWT signing secrets
- production connection strings

---

# Status

**Active development**

The current focus is:

1. Completing and stabilizing core application functionality
2. Migrating Google location functionality to Places API (New)
3. Improving image\-storage handling
4. Strengthening backend architecture
5. Adding automated testing
6. Implementing CI/CD
7. Deploying the complete system to Azure
8. Adding production observability

---

# Related Project

The NZParadise solution also includes a separate **ASP.NET Core Web API backend repository** containing the backend services, Entity Framework Core data access, SQL Server integration, authentication, and Blob Storage integration.

---


Software Engineer focused on C\#, .NET, React, TypeScript, SQL, cloud architecture, and modern software engineering practices.

