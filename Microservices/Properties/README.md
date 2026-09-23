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

Edita la cadena de conexión en `Properties.Api/appsettings.json` (clave `MyConnection`):

```json
"ConnectionStrings": {
  "MyConnection": "Server=.\\DOMINGUEZSERVER;Database=InmobyPropertiesDb;User Id=...;Password=...;encrypt=false"
}
```

## Migraciones EF Core

Desde la raíz del microservicio (`Microservices/Properties`). Hay **dos formas** de hacerlo: con el script auxiliar o con `dotnet ef` directo.

### Forma 1 — Script auxiliar

Bash (Git Bash / WSL / Linux / macOS):

```
./scripts/ef.sh add AddIndexes
./scripts/ef.sh update
./scripts/ef.sh update InnitialSquema
./scripts/ef.sh list
./scripts/ef.sh remove
./scripts/ef.sh script
./scripts/ef.sh script migrations.sql
```

CMD (Windows):

```
scripts\ef.cmd add AddIndexes
scripts\ef.cmd update
scripts\ef.cmd update InnitialSquema
scripts\ef.cmd list
scripts\ef.cmd remove
scripts\ef.cmd script
scripts\ef.cmd script migrations.sql
```

### Forma 2 — Comandos `dotnet ef` directos

Crear una migración:

```
dotnet ef migrations add <NombreMigracion> --project Properties.Persistence/Properties.Persistence.csproj --startup-project Properties.Api/Properties.Api.csproj --output-dir Migrations
```

Ejemplo:

```
dotnet ef migrations add AddPropertyAmenities --project Properties.Persistence/Properties.Persistence.csproj --startup-project Properties.Api/Properties.Api.csproj --output-dir Migrations
```

Aplicar migraciones:

```
dotnet ef database update --project Properties.Persistence/Properties.Persistence.csproj --startup-project Properties.Api/Properties.Api.csproj
```

Aplicar hasta una migración específica:

```
dotnet ef database update <NombreMigracion> --project Properties.Persistence/Properties.Persistence.csproj --startup-project Properties.Api/Properties.Api.csproj
```

Listar migraciones:

```
dotnet ef migrations list --project Properties.Persistence/Properties.Persistence.csproj --startup-project Properties.Api/Properties.Api.csproj
```

Eliminar la última migración (sin aplicarla):

```
dotnet ef migrations remove --project Properties.Persistence/Properties.Persistence.csproj --startup-project Properties.Api/Properties.Api.csproj
```

Generar script SQL:

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
