# Identity microservice

Servicio de ejemplo que usa ASP.NET Core Identity para persistir usuarios y roles en SQL Server, comprobar credenciales y emitir JWT.

## Configuración local

La conexión `IdentityConnection` apunta por defecto a `DOMINGUEZSERVER` y usa autenticación integrada de Windows. Si tu SQL Server requiere otro modo de autenticación, reemplaza la cadena con User Secrets o una variable de entorno, por ejemplo:

```powershell
dotnet user-secrets set "ConnectionStrings:IdentityConnection" "Server=localhost;Database=InmobyIdentityDb;Trusted_Connection=True;TrustServerCertificate=True" --project .\Identity.App
```

La clave JWT de desarrollo ya está en `appsettings.Development.json`; configura `Jwt__SigningKey` con un secreto propio de al menos 32 bytes fuera de desarrollo. En el arranque, `IdentitySeeder` crea las tablas de ASP.NET Core Identity, los roles `User`, `Admin` y `Properties Manager`, y dos cuentas base con credenciales definidas en el código del seeder: `admin@inmoby.local` / `Admin123` y `properties@inmoby.local` / `Properties123`. Las cuentas normales se registran con `POST /api/auth/register` y reciben el rol `User`. Identity aplica las reglas de contraseña configuradas en `Program.cs` y administra el hash, la normalización del email, roles y bloqueo por intentos fallidos. Las credenciales codificadas son únicamente para el entorno educativo local.

## Endpoints

- `POST /api/auth/register`: registra un usuario con `firstName`, `lastName`, `document`, `email` y `password`. El documento se almacena en mayúsculas y es único.
- `POST /api/auth/login`: recibe email y contraseña; valida el hash y devuelve el JWT con los roles actuales.
- `GET /api/auth/me`: devuelve la identidad autenticada.
- `GET /api/users`: lista usuarios y sus roles (requiere `Admin`).
- `PUT /api/users/{userId}/roles/{roleName}` y `DELETE /api/users/{userId}/roles/{roleName}`: asigna o retira roles (requiere `Admin`).
- `GET /api/roles`: lista los roles definidos por el seeder (requiere `Admin`). Los roles se asignan o retiran a usuarios mediante `PUT` y `DELETE /api/users/{userId}/roles/{roleName}`.

Todas las respuestas HTTP de la API usan el sobre `ApiResponse<T>` con `isSuccess`, `data`, `message` y `errors`, siguiendo el formato de `Properties`. La autenticación está detrás de `IAuthService`, la administración de usuarios detrás de `IUsersService` y la gestión de roles detrás de `IRolesService`; sus implementaciones se registran en el contenedor de dependencias.

Los contratos HTTP están en `DTOs/Requests` y `DTOs/Responses`. Los DTOs usan propiedades explícitas y atributos de validación para que el binding de ASP.NET Core valide los campos automáticamente.

Identity crea las tablas estándar (`AspNetUsers`, `AspNetRoles`, `AspNetUserRoles` y otras). Los roles van dentro del JWT al iniciar sesión; después de cambiar los roles de una cuenta, esa persona debe volver a iniciar sesión para recibir un token actualizado. Las migraciones se aplican al iniciar el servicio. Si la base ya fue creada con `EnsureCreated` por una versión anterior, usa una base vacía nueva o prepara una migración de adopción antes de iniciar esta versión.
