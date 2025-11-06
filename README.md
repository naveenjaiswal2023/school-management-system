# School Management System - System Architecture Document

**Document Status:** Approved  
**Version:** 1.0  
**Date:** October 25, 2025  
**Author:** Senior .NET Development Team

---

## Table of Contents
1. [Executive Summary](#executive-summary)
2. [System Overview](#system-overview)
3. [Architecture Principles](#architecture-principles)
4. [System Architecture](#system-architecture)
5. [Microservices Architecture](#microservices-architecture)
6. [Clean Architecture Implementation](#clean-architecture-implementation)
7. [Technology Stack](#technology-stack)
8. [Data Architecture](#data-architecture)
9. [Security Architecture](#security-architecture)
10. [Deployment Architecture](#deployment-architecture)

---

## Executive Summary

The School Management System is an enterprise-grade distributed application built using microservices architecture, implementing Clean Architecture, CQRS (Command Query Responsibility Segregation), and Domain-Driven Design (DDD) patterns. The system is designed to handle comprehensive school operations including student management, attendance tracking, examination management, fee collection, HRMS, and notifications.

### Key Architectural Decisions
- **Microservices Architecture** for scalability and independent deployment
- **Clean Architecture** for maintainability and testability
- **CQRS Pattern** for separation of read/write operations
- **API Gateway Pattern** for centralized routing
- **Event-Driven Communication** for loose coupling
- **JWT-Based Authentication** for secure access control
- **Role-Based Access Control (RBAC)** with menu-level permissions

---

## System Overview

### Business Context
The system manages the complete lifecycle of school operations across multiple domains:
- **Student Management**: Enrollment, academic records, biometric integration
- **Attendance Management**: Real-time tracking with biometric device integration
- **Examination Management**: Exam scheduling, result processing, analytics
- **Fee Management**: Fee structure, payment processing, receipts
- **HRMS**: Employee onboarding, payroll, leave management, performance reviews
- **Notification Service**: Multi-channel notifications (Email, SMS, Push)
- **User Management**: Authentication, authorization, role management

### Key Features
- Dynamic menu and role-based permission system
- Biometric device integration for attendance
- Offline attendance synchronization
- Real-time notifications
- Comprehensive reporting and analytics
- Multi-tenant support capability

---

## Architecture Principles

### 1. Separation of Concerns
Each layer has distinct responsibilities:
- **Domain Layer**: Business logic and domain models
- **Application Layer**: Use cases, commands, queries, DTOs
- **Infrastructure Layer**: External integrations, background services
- **Persistence Layer**: Data access, repositories, EF Core
- **API Layer**: Controllers, middleware, HTTP handling

### 2. Dependency Rule
Dependencies point inward toward the Domain:
```
API → Application → Domain ← Infrastructure ← Persistence
```

### 3. Technology Independence
- Core business logic is independent of frameworks
- Infrastructure dependencies are abstracted through interfaces
- Easy to swap implementations (database, notification provider, etc.)

### 4. Testability
- Each layer can be tested independently
- Business logic isolated from infrastructure concerns
- Use of interfaces enables mocking for unit tests

---

## System Architecture

### High-Level Architecture Diagram

```
┌─────────────────────────────────────────────────────────────────┐
│                         API Gateway                              │
│                  (Future Enhancement)                            │
└────────────┬────────────────────────────────────────────────────┘
             │
             ├──────────────┬──────────────┬──────────────┬────────────┐
             │              │              │              │            │
    ┌────────▼───────┐ ┌───▼──────┐ ┌────▼──────┐ ┌────▼──────┐ ┌──▼───────┐
    │ SchoolMgmt     │ │ Student  │ │ Attendance│ │Examination│ │   Fee    │
    │   Service      │ │  Service │ │  Service  │ │  Service  │ │ Service  │
    │  (Core API)    │ │          │ │           │ │           │ │          │
    └───────┬────────┘ └────┬─────┘ └─────┬─────┘ └─────┬─────┘ └────┬─────┘
            │               │              │             │             │
    ┌───────▼───────┐ ┌────▼─────┐  ┌────▼──────┐ ┌────▼──────┐ ┌───▼──────┐
    │     HRMS      │ │   User   │  │Notification│ │Background │ │          │
    │   Service     │ │Management│  │  Service   │ │ Services  │ │   DB     │
    │               │ │  Service │  │            │ │           │ │          │
    └───────────────┘ └──────────┘  └────────────┘ └───────────┘ └──────────┘
```

---

## Microservices Architecture

### Service Catalog

| Service Name | Responsibility | Port | Technology |
|--------------|----------------|------|------------|
| **SchoolManagement.API** | Core service: Role management, Menu permissions, Authorization, Student/Employee CRUD | 5000 | ASP.NET Core 8.0 |
| **StudentManagement.API** | Student-specific operations, Academic records | 5001 | ASP.NET Core 8.0 |
| **AttendanceService.API** | Real-time attendance tracking, Biometric integration | 5002 | ASP.NET Core 8.0 |
| **ExaminationService.API** | Exam management, Result processing | 5003 | ASP.NET Core 8.0 |
| **FeeManagement.API** | Fee structure, Payment processing | 5004 | ASP.NET Core 8.0 |
| **HRMSService.API** | Employee management, Payroll, Leave | 5005 | ASP.NET Core 8.0 |
| **UserManagement.API** | User authentication, Profile management | 5006 | ASP.NET Core 8.0 |
| **NotificationService.API** | Email, SMS, Push notifications | 5007 | ASP.NET Core 8.0 |

### Service Communication Patterns

#### Synchronous Communication
- **HTTP/REST**: For immediate request-response scenarios
- **Use Cases**: 
  - User authentication requests
  - Real-time data queries
  - CRUD operations
  - Permission verification

#### Asynchronous Communication (Future)
- **Message Queue/Event Bus**: For eventual consistency scenarios
- **Planned Use Cases**:
  - Attendance marked → Notify parents
  - Fee payment received → Update student records
  - Employee onboarded → Create user account

## Clean Architecture Implementation

### Layer Structure

┌────────────────────────────────────────────────────────────┐
│                       API Layer                             │
│  Controllers, Middleware, Authorization, Startup           │
└─────────────────────────┬──────────────────────────────────┘
                          │
┌─────────────────────────▼──────────────────────────────────┐
│                  Application Layer                          │
│  Commands, Queries, Handlers (CQRS), DTOs, Validators      │
└─────────────────────────┬──────────────────────────────────┘
                          │
┌─────────────────────────▼──────────────────────────────────┐
│                    Domain Layer (Core)                      │
│  Entities, Value Objects, Enums, Domain Services,          │
│  Business Rules, Domain Exceptions                          │
└─────────────────────────┬──────────────────────────────────┘
                          │
         ┌────────────────┴────────────────┐
         │                                  │
┌────────▼────────────┐         ┌──────────▼───────────────┐
│ Infrastructure Layer│         │   Persistence Layer      │
│  Background Services│         │  DbContext, Repositories │
│  External Services  │         │  Migrations, UnitOfWork  │
│  Notification, Email│         │  Entity Configurations   │
└─────────────────────┘         └──────────────────────────┘
```

### Domain Layer (SchoolManagement.Domain)

**Purpose**: Contains pure business logic, independent of any framework

**Components**:
- **Entities**: Core business objects (Student, Employee, Attendance, etc.)
- **Value Objects**: Immutable objects (Address, BiometricInfo, Salary)
- **Enums**: Business constants (AttendanceStatus, EmployeeStatus, etc.)
- **Domain Services**: Business operations spanning multiple entities
- **Exceptions**: Domain-specific exceptions
- **Interfaces**: Contracts for domain services

**Key Entities**:
```csharp
BaseEntity (Id, CreatedAt, UpdatedAt, IsDeleted)
├── Student
├── Employee
├── Attendance / EmployeeAttendance
├── Class / Section / Department
├── User / Role / Permission
├── Menu / RoleMenuPermission
├── FeePayment / ExamResult
└── LeaveApplication / PayrollRecord
```

### Application Layer (SchoolManagement.Application)

**Purpose**: Implements use cases using CQRS pattern

**Structure**:
Application/
├── [Feature]/
│   ├── Commands/
│   │   ├── CreateXCommand.cs
│   │   ├── CreateXResponse.cs
│   │   ├── UpdateXCommand.cs
│   │   └── DeleteXCommand.cs
│   ├── Queries/
│   │   ├── GetXByIdQuery.cs
│   │   └── GetAllXQuery.cs
│   └── Handlers/
│       ├── Commands/
│       │   └── CreateXCommandHandler.cs
│       └── Queries/
│           └── GetXQueryHandler.cs
├── DTOs/
├── Validators/
├── Interfaces/
└── Services/

**Key Features Implemented**:
- **Students**: Create, Update, Enroll Biometric, Query by Class
- **Employees**: Create, Update, Onboard, Query by Department
- **Attendance**: Mark Attendance (Biometric/Manual), Query Statistics
- **Menus**: CRUD operations, Hierarchy management
- **Roles**: CRUD operations, Permission assignment
- **Role Permissions**: Assign, Update, Revoke menu permissions

**CQRS Benefits**:
- **Commands**: Change state, validated, transactional
- **Queries**: Read-only, optimized for specific views, no validation needed
- **Independent Scaling**: Can scale reads and writes separately
- **Clear Responsibility**: Each handler has single responsibility

### Infrastructure Layer (SchoolManagement.Infrastructure)

**Purpose**: Technical implementation details

**Components**:
1. **Background Services**:
   - `AttendanceSyncService`: Syncs offline attendance records
   - `NotificationProcessorService`: Processes notification queue

2. **External Services**:
   - `BiometricVerificationService`: Integrates with biometric devices
   - `AttendanceCalculationService`: Business calculations
   - `NotificationService`: Email, SMS, Push notifications

3. **Configuration**:
   - Settings classes for all external integrations
   - Device configurations
   - Security settings

4. **Data Seeding**:
   - `MenuDataSeeder`: Seeds default menu structure

### Persistence Layer (SchoolManagement.Persistence)

**Purpose**: Data access implementation

**Components**:
- **DbContext**: `SchoolManagementDbContext` with all entity configurations
- **Repositories**: Implementation of repository interfaces
  - `StudentRepository`
  - `EmployeeRepository`
  - `AttendanceRepository`
  - `MenuRepository`
  - `RoleRepository`
  - `PermissionRepository`
  - `RoleMenuPermissionRepository`
- **Unit of Work**: Transaction management across repositories

**Repository Pattern Benefits**:
- Abstraction over data access
- Testability (can mock repositories)
- Centralized query logic
- Transaction management through UnitOfWork

### API Layer (SchoolManagement.API)

**Purpose**: HTTP interface, routing, middleware

**Components**:
1. **Controllers**: RESTful endpoints
   - `StudentsController`
   - `EmployeesController`
   - `AttendanceController`
   - `MenusController`
   - `RolesController`
   - `RolePermissionsController`
   - `UserRolesController`

2. **Authorization**:
   - `MenuPermissionAttribute`: Custom authorization attribute
   - `MenuPermissionHandler`: Permission verification logic

3. **Middleware**:
   - `GlobalExceptionHandlerMiddleware`: Centralized exception handling

4. **Helpers**:
   - `MenuHelper`: Menu hierarchy and breadcrumb generation
   - DTOs for API responses

## Technology Stack

### Backend
| Component | Technology | Version | Purpose |
|-----------|-----------|---------|---------|
| Framework | .NET Core | 8.0 | API development |
| ORM | Entity Framework Core | 8.0 | Data access |
| Mediator | MediatR | 12.x | CQRS implementation |
| Validation | FluentValidation | 11.x | Input validation |
| Authentication | JWT Bearer | 8.0 | Token-based auth |
| Logging | Serilog | 3.x | Structured logging |
| API Documentation | Swagger/OpenAPI | 6.5 | API documentation |

### Frontend (Separate Repository)
| Component | Technology | Purpose |
|-----------|-----------|---------|
| Framework | React | 18.x | UI framework |
| Language | TypeScript | 5.x | Type safety |
| State Management | Context API | Built-in | Global state |
| Routing | React Router | 6.x | Navigation |
| HTTP Client | Axios | 1.x | API calls |

### Database
| Type | Technology | Purpose |
|------|-----------|---------|
| Primary DB | SQL Server | 2022 | Relational data |
| Future: Cache | Redis | N/A | Performance optimization |
| Future: Queue | RabbitMQ/Azure Service Bus | N/A | Message queuing |

### DevOps & Infrastructure
| Component | Technology | Purpose |
|-----------|-----------|---------|
| Version Control | Git | Source control |
| Container | Docker | Containerization |
| Orchestration | Docker Compose | Local development |
| CI/CD | Azure DevOps / GitHub Actions | Automation |
| Hosting | Azure App Service | Cloud hosting |
| Monitoring | Application Insights | APM |

---

## Data Architecture

### Database Strategy
- **Single Database**: Currently using single SQL Server database
- **Schema Separation**: Tables organized by domain (Student_, Employee_, etc.)
- **Future: Database per Service**: Can migrate to microservices pattern

### Entity Relationships

```
User ──┐
       ├──> UserRole ──> Role ──> RoleMenuPermission ──> Menu
       │                   └────> RolePermission ──> Permission
       │
Student ──> Class ──> Section
       └──> Attendance
       └──> ExamResult
       └──> FeePayment
       └──> StudentParent

Employee ──> Department
        └──> Designation
        └──> EmployeeAttendance
        └──> LeaveApplication
        └──> PayrollRecord ──> Allowance / Deduction
        └──> PerformanceReview
```

### Data Access Patterns

1. **Repository Pattern**: All data access through repositories
2. **Unit of Work**: Transaction consistency across repositories
3. **Query Objects**: Complex queries encapsulated in query classes
4. **Specification Pattern**: Reusable query criteria (planned)

---

## Security Architecture

### Authentication
- **JWT Token-Based**: Stateless authentication
- **Token Lifetime**: Configurable (default 60 minutes)
- **Refresh Token**: For seamless user experience (planned)

### Authorization
- **Multi-Level Authorization**:
  1. **API Level**: JWT token validation
  2. **Role Level**: User must have assigned role
  3. **Menu Level**: User must have permission for specific menu
  4. **Action Level**: Read/Write/Delete permissions per menu

### Menu-Based Permission System

```
User → UserRole → Role → RoleMenuPermission → Menu → Permissions
                                                 ├─ CanRead
                                                 ├─ CanWrite
                                                 └─ CanDelete
```

**Implementation**:
```csharp
[MenuPermission("Students", "Write")]
public async Task<IActionResult> CreateStudent(CreateStudentCommand command)
```

### Security Best Practices
- Passwords hashed using BCrypt/PBKDF2
- SQL injection prevention via parameterized queries (EF Core)
- Input validation using FluentValidation
- HTTPS enforcement
- CORS policy configuration
- API rate limiting (planned)

---

## Deployment Architecture

### Current Deployment (Development)
```
Local Development Environment
├── Multiple API Projects (Ports 5000-5007)
├── SQL Server (Local/Docker)
└── React App (Port 3000)
```

### Planned Production Deployment

```
Azure Cloud Infrastructure
├── Azure App Service (Web Apps)
│   ├── SchoolManagement.API
│   ├── AttendanceService.API
│   ├── ExaminationService.API
│   └── [Other Services]
├── Azure SQL Database
├── Azure Application Gateway (API Gateway)
├── Azure Key Vault (Secrets Management)
├── Azure Cache for Redis
├── Azure Service Bus (Event Bus)
└── Azure Application Insights (Monitoring)
```

### Container Deployment Strategy

**Docker Compose** (for local/staging):
```yaml
services:
  schoolmanagement-api:
    build: ./SchoolManagement.API
    ports: ["5000:80"]
  attendance-api:
    build: ./AttendanceService.API
    ports: ["5002:80"]
  sql-server:
    image: mcr.microsoft.com/mssql/server:2022
  # ... other services
```

**Kubernetes** (future production):
- Pods for each microservice
- Services for internal communication
- Ingress controller for external access
- ConfigMaps and Secrets for configuration

---

## Non-Functional Requirements

### Performance
- API response time < 500ms (95th percentile)
- Support 1000+ concurrent users
- Database query optimization with indexes
- Caching strategy for frequently accessed data

### Scalability
- Horizontal scaling of individual microservices
- Stateless API design
- Load balancing across instances
- Database read replicas (future)

### Availability
- 99.9% uptime SLA
- Health check endpoints for all services
- Automated failover mechanisms
- Regular backup strategy (daily)

### Monitoring & Observability
- Structured logging with Serilog
- Application Performance Monitoring (APM)
- Distributed tracing (future)
- Real-time alerting for critical failures

---

## Future Enhancements

### Phase 1 (Next 3 Months)
- [ ] API Gateway implementation (Ocelot/YARP)
- [ ] Event-driven architecture with message bus
- [ ] Refresh token implementation
- [ ] Enhanced logging and monitoring

### Phase 2 (6 Months)
- [ ] Database per microservice
- [ ] Redis caching layer
- [ ] Advanced search with Elasticsearch
- [ ] Mobile app development

### Phase 3 (12 Months)
- [ ] AI/ML integration for predictions
- [ ] Advanced analytics dashboard
- [ ] Multi-tenant support
- [ ] Kubernetes orchestration

---

## Appendix

### A. Design Patterns Used
1. **CQRS** - Command Query Responsibility Segregation
2. **Repository Pattern** - Data access abstraction
3. **Unit of Work** - Transaction management
4. **Mediator Pattern** - MediatR for loose coupling
5. **Factory Pattern** - Object creation
6. **Strategy Pattern** - Notification providers
7. **Dependency Injection** - IoC container

### B. Coding Standards
- Follow C# coding conventions
- Async/await for I/O operations
- Proper exception handling
- XML documentation for public APIs
- Unit test coverage > 80%

### C. API Versioning Strategy
- URL-based versioning: `/api/v1/students`
- Maintain backward compatibility for 2 versions
- Deprecation notice 6 months in advance

---

**Document Approval**

| Role | Name | Date | Signature |
|------|------|------|-----------|
| Technical Architect | [Your Name] | 2025-10-25 | _________ |
| Project Manager | [Name] | 2025-10-25 | _________ |
| Development Lead | [Name] | 2025-10-25 | _________ |

---

*End of System Architecture Document*
