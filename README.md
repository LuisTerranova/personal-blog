# TrashTechHub

A personal blog and portfolio platform built with Blazor Server, running at [trashtechhub.com](https://trashtechhub.com).

## Architecture

Clean Architecture with 4 layers:

| Layer | Project | Responsibility |
| :--- | :--- | :--- |
| **Core** | `trash-tech-hub.core` | Domain entities, interfaces, DTOs, requests, exceptions |
| **Application** | `trash-tech-hub.application` | Business logic, AutoMapper profiles, Markdown rendering |
| **Infrastructure** | `trash-tech-hub.infrastructure` | EF Core / PostgreSQL, repository implementations, auth, file storage |
| **Web** | `trash-tech-hub.web` | Blazor Server UI (MudBlazor), admin panel, sitemap, Docker |

## Tech Stack

| Component | Technology |
| :--- | :--- |
| **Framework** | .NET 10 |
| **Frontend** | Blazor Server (Interactive Server rendering) |
| **UI Library** | MudBlazor 9.x |
| **Database** | PostgreSQL 16 + EF Core 9.x |
| **Auth** | Cookie authentication + BCrypt |
| **ORM Mapping** | AutoMapper |
| **Markdown** | Markdig |
| **Testing** | xUnit + EF Core InMemory |
| **CI/CD** | GitHub Actions (self-hosted on Raspberry Pi 5) |
| **Containerization** | Docker + docker-compose |

## Features

- **Blog** — Posts with categories, slugs, pagination, featured posts, Markdown body
- **Projects** — Portfolio projects with image uploads, repo links, slugs
- **Admin Panel** — Password-protected dashboard for managing posts, projects, and categories
- **Dark/Light Theme** — Persisted to localStorage, respects system preference
- **SEO** — Auto-generated sitemap at `/sitemap.xml`
- **Containerized** — Ready for production deployment via Docker

## Project Structure

```
src/
├── trash-tech-hub.core/           # Domain & interfaces
│   ├── Models/                    # Post, Category, Project, AdminUser
│   ├── DTOs/                      # PostDto, CategoryDto, ProjectDto
│   ├── Requests/                  # Posts/, Projects/, Categories/
│   ├── Services/                  # IPostService, IProjectService, etc.
│   ├── Repositories/              # IPostRepository, IUnitOfWork, etc.
│   └── Exceptions/
├── trash-tech-hub.application/    # Business logic
│   ├── Services/                  # PostService, ProjectService, etc.
│   └── Mappings/                  # AutoMapper profile
├── trash-tech-hub.infrastructure/ # Data access & infrastructure
│   ├── Data/                      # AppDbContext, migrations
│   ├── Repositories/              # EF Core implementations
│   └── Services/                  # AuthService, LocalFileStorageService
├── trash-tech-hub.web/            # Blazor Server app
│   ├── Components/                # Pages (Public, Admin), Layout, Themes
│   └── Common/                    # Startup extensions, MockData, ThemeState
└── trash-tech-hub.tests/          # xUnit tests
```

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- PostgreSQL 16

### Run Locally

```bash
cd src
# Set up connection string in appsettings.json or via env vars
dotnet run --project trash-tech-hub.web
```

The app applies migrations and seeds an admin user on first startup. Default credentials are configured via `AdminSettings:Email` and `AdminSettings:Password`.

### Docker

```bash
cd src
docker compose -f docker-compose-prod.yml up -d
```
