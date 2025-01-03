# AspNetCoreIdentityAuthorizationCustom

This project demonstrates how to implement custom authorization policies in ASP.NET Core using the `IAuthorizationPolicyProvider`.

The goal is to provide a solution for scenarios where policies cannot be registered statically or when policies based on dynamic parameters are required.

## Key Features

- **Custom Policy Provider:**
  Implements a custom `IAuthorizationPolicyProvider` that dynamically generates authorization policies based on specific requirements.

- **Custom Authorization Attribute:**
  Defines an authorization attribute that accepts parameters, enabling the application of policies based on dynamic conditions directly on controllers or actions.

- **Custom Requirement Handlers:**
  Includes handlers that evaluate the policy requirements at runtime, determining whether a user meets the defined criteria.

## Project Structure

- **Controllers/**: 
  Contains MVC controllers that demonstrate the application of custom authorization policies.

- **Data/**: 
  Includes classes related to data access and Entity Framework Core configuration.

- **Identity/**: 
  Houses implementations related to identity and authorization, including the custom policy provider and requirement handlers.

- **Migrations/**: 
  Contains Entity Framework Core migrations to configure the database schema.

## Prerequisites

- ASP.NET Core 6.0 or later.
- Entity Framework Core for data access.

## How to Run the Project

1. **Clone the repository:**
   ```bash
   git clone https://github.com/JohnSalazar/AspNetCoreIdentityAuthorizationCustom.git
   ```

2. **Navigate to the project directory:**
   ```bash
   cd AspNetCoreIdentityAuthorizationCustom
   ```

3. **Restore dependencies:**
   ```bash
   dotnet restore
   ```

4. **Update the database:**
   ```bash
   dotnet ef database update
   ```

5. **Run the application:**
   ```bash
   dotnet run
   ```

## References

- [Custom Authorization Policy Providers in ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/security/authorization/iauthorizationpolicyprovider)
- [How to Create a Custom Authorize Attribute in ASP.NET Core](https://code-maze.com/custom-authorize-attribute-aspnetcore/)

---

This project serves as a practical guide for developers looking to implement more flexible and dynamic authorization solutions in their ASP.NET Core applications.

---

This project was developed by [JohnSalazar](https://github.com/JohnSalazar) <img alt="Brazil" src="https://github.com/user-attachments/assets/6340ab49-4afe-43cb-acce-53ab1e2f64c2" width="20" height="14"/> under the [MIT license](LICENSE).
