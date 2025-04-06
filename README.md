<p align="center">
  <a href="https://dotnet.microsoft.com/" target="blank"><img src="https://upload.wikimedia.org/wikipedia/commons/e/ee/.NET_Core_Logo.svg" width="120" alt=".NET Logo" /></a>
</p>


# Sales System - Modular Monolith

## Technologies Used 🛠️
  - **Framework**: ASP.NET 9.0
  - **Database**: SQL Server
  - **Containerization**: Docker
  - **Mediator Library**: MediatR (for Commands, Queries, and Events)
  - **Event Storage**: EventStore (Event Sourcing)

## Architecture 🏛️
<p>The project follows a <strong>Modular Monolith</strong> architecture, split into four main modules, each with its own database schema and implementing the <strong>Hexagonal Architecture</strong>. Communication between modules is asynchronous and event-based, adhering to an <strong>EDA (Event-Driven Architecture)</strong> approach, ensuring modules remain unaware of each other. Concepts of <strong>Clean Architecture</strong> and <strong>DDD (Domain-Driven Design)</strong> are strictly applied, alongside <strong>CQRS</strong> and <strong>Event Sourcing</strong>. A <strong>Shared Kernel</strong> is used as a shared context to provide common models, utilities, and logic reused across the bounded contexts (modules).</p>

### Modules
  - **Sales**: Handles orders and the sales workflow.
  - **Payments**: Manages payment processing, integrating with Stripe via an additional <strong>ACL (Anti-Corruption Layer)</strong>.
  - **Register**: Oversees authentication and customer data management, generating JWTs and supporting RBAC on endpoints.
  - **Catalog**: Controls product inventory and availability.

<h3>Layer Structure</h3>
    <p>Each module consists of three core layers, with Payments having an extra one:</p>
    <ul>
        <li><strong>Application</strong>: Contains application logic, including Commands, Queries, and Handlers.</li>
        <li><strong>Domain</strong>: Houses entities, business rules, and domain logic.</li>
        <li><strong>Infrastructure</strong>: Manages data access, external services, and configurations.</li>
        <li><strong>ACL (Anti-Corruption Layer)</strong>: Exclusive to Payments, it shields the domain from Stripe integration.</li>
    </ul>

## Communication 📡
<p>Modules interact solely through events, leveraging the <strong>MediatR</strong> library to dispatch and handle <code>Commands</code>, <code>Queries</code>, and <code>Events</code>. The system employs <strong>Event Sourcing</strong>, storing state as a sequence of events in <strong>EventStore</strong>.</p>

## Interface 🌐
<p>A <strong>UI layer</strong>, implemented as an API, connects to all modules, serving as the entry point for clients. It exposes endpoints secured with <strong>JWT</strong>-based authentication and <strong>RBAC</strong> access control.</p>
