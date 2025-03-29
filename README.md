<p align="center">
  <a href="https://dotnet.microsoft.com/" target="blank"><img src="https://upload.wikimedia.org/wikipedia/commons/e/ee/.NET_Core_Logo.svg" width="120" alt=".NET Logo" /></a>
</p>


# Sales System

  <p>This is a robust and modern sales system designed for scalability, maintainability, and clear separation of concerns.</p>

  <h2>Technologies Used 🛠️</h2>
    <ul>
        <li><strong>Framework</strong>: ASP.NET 9.0</li>
        <li><strong>Database</strong>: SQL Server</li>
        <li><strong>Containerization</strong>: Docker</li>
        <li><strong>Mediation Library</strong>: MediatR (for Commands, Queries, and Events)</li>
        <li><strong>Event Storage</strong>: EventStore (Event Sourcing)</li>
    </ul>

  <h2>Architecture 🏛️</h2>
    <p>The project follows a <strong>Modular Monolith</strong> architecture, split into five main modules, each with its own database schema and implementing the <strong>Hexagonal Architecture</strong>. Communication between modules is asynchronous and event-based, adhering to an <strong>EDA (Event-Driven Architecture)</strong> approach, ensuring modules remain unaware of each other. Concepts of <strong>Clean Architecture</strong> and <strong>DDD (Domain-Driven Design)</strong> are strictly applied, alongside <strong>CQRS</strong> and <strong>Event Sourcing</strong>.</p>

  <h3>Modules</h3>
    <ul>
        <li><strong>Sales</strong>: Handles orders and the sales workflow.</li>
        <li><strong>Payments</strong>: Manages payment processing, integrating with Stripe via an additional <strong>ACL (Anti-Corruption Layer)</strong>.</li>
        <li><strong>Register</strong>: Oversees authentication and customer data management, generating JWTs and supporting RBAC on endpoints.</li>
        <li><strong>Invoices</strong>: Manages order-related invoices.</li>
        <li><strong>Catalog</strong>: Controls product inventory and availability.</li>
    </ul>

   <h3>Layer Structure</h3>
    <p>Each module consists of three core layers, with Payments having an extra one:</p>
    <ul>
        <li><strong>Application</strong>: Contains application logic, including Commands, Queries, and Handlers.</li>
        <li><strong>Domain</strong>: Houses entities, business rules, and domain logic.</li>
        <li><strong>Infrastructure</strong>: Manages data access, external services, and configurations.</li>
        <li><strong>ACL (Anti-Corruption Layer)</strong>: Exclusive to Payments, it shields the domain from Stripe integration.</li>
    </ul>

  <h2>Communication 📡</h2>
    <p>Modules interact solely through events, leveraging the <strong>MediatR</strong> library to dispatch and handle <code>Commands</code>, <code>Queries</code>, and <code>Events</code>. The system employs <strong>Event Sourcing</strong>, storing state as a sequence of events in <strong>EventStore</strong>.</p>

   <h2>Interface 🌐</h2>
    <p>A <strong>UI layer</strong>, implemented as an API, connects to all modules, serving as the entry point for clients. It exposes endpoints secured with <strong>JWT</strong>-based authentication and <strong>RBAC</strong> access control.</p>
