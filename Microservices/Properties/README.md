# Properties Microservice

Microservicio de gestión de inmuebles. Las migraciones de EF Core viven en **Properties.Persistence** y la conexión se configura en **Properties.Api** (presenter).

## Requisitos

- [.NET SDK 10](https://dotnet.microsoft.com/download)
- SQL Server o LocalDB
- [EF Core CLI](https://learn.microsoft.com/en-us/ef/core/cli/dotnet)

Instalar la herramienta global de EF (una sola vez):

```
dotnet tool install --global dotnet-ef
```

Actualizar si ya la tienes instalada:

```
dotnet tool update --global dotnet-ef
```

## Connection string

Edita la cadena de conexión en:

- `Properties.Api/appsettings.json`
- `Properties.Api/appsettings.Development.json`

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=PropertiesDb;Trusted_Connection=True;TrustServerCertificate=True"
}
```

## Migraciones EF Core

Ejecuta los comandos desde la raíz del microservicio (`Microservices/Properties`).

### Crear una migración

```
dotnet ef migrations add <NombreMigracion> --project Properties.Persistence/Properties.Persistence.csproj --startup-project Properties.Api/Properties.Api.csproj --output-dir Migrations
```

Ejemplo:

```
dotnet ef migrations add AddPropertyAmenities --project Properties.Persistence/Properties.Persistence.csproj --startup-project Properties.Api/Properties.Api.csproj --output-dir Migrations
```

### Aplicar migraciones a la base de datos

```
dotnet ef database update --project Properties.Persistence/Properties.Persistence.csproj --startup-project Properties.Api/Properties.Api.csproj
```

Aplicar hasta una migración específica:

```
dotnet ef database update <NombreMigracion> --project Properties.Persistence/Properties.Persistence.csproj --startup-project Properties.Api/Properties.Api.csproj
```

### Listar migraciones

```
dotnet ef migrations list --project Properties.Persistence/Properties.Persistence.csproj --startup-project Properties.Api/Properties.Api.csproj
```

### Eliminar la última migración (sin aplicarla)

```
dotnet ef migrations remove --project Properties.Persistence/Properties.Persistence.csproj --startup-project Properties.Api/Properties.Api.csproj
```

### Generar script SQL

```
dotnet ef migrations script --project Properties.Persistence/Properties.Persistence.csproj --startup-project Properties.Api/Properties.Api.csproj --output migrations.sql
```

## Ejecutar la API

```
dotnet run --project Properties.Api/Properties.Api.csproj
```

## Estructura del proyecto

| Proyecto | Rol |
|---|---|
| `Properties.Domain` | Entidades, value objects, reglas de negocio |
| `Properties.Application` | Casos de uso, contratos, mediator |
| `Properties.Persistence` | EF Core, repositorios, migraciones |
| `Properties.Api` | Presenter: configuración, conexión, endpoints |
| `Properties.Tests` | Pruebas |
