# Booking microservice

Microservicio de visitas a inmuebles de Inmoby.

## Enfoque

Este servicio sigue el estilo minimalista de `PrivateBlog.Web`: un solo proyecto Web API con separación por carpetas.

```
Booking.Api
├── Controllers   -> endpoints HTTP
├── Core          -> Response, paginación, extensiones y validaciones
├── Data          -> DataContext, entidad Booking y migraciones
├── DTOs          -> contratos de entrada/salida
└── Services      -> lógica de negocio y acceso a datos
```

No usa Clean Architecture + DDD por proyectos. Esa responsabilidad queda reservada para `Properties`, según el microcurrículo.

## Entidad

`Booking` representa una visita agendada:

- `PropertyId`: referencia externa al microservicio Properties.
- `UserId`: referencia externa al microservicio Identity.
- `ScheduledAt`: fecha y hora UTC de la visita.
- `DurationMinutes`: duración en minutos.
- `Status`: `Pending`, `Confirmed`, `Cancelled`, `Completed`.
- `Notes`, `CancelledAt`, `CancelReason`, `CreatedAt`, `UpdatedAt`.

No hay llaves foráneas entre servicios: cada microservicio es dueño de sus datos.

## Validación del inmueble y del usuario

- **Usuario:** se valida con el JWT emitido por `Identity`. El `UserId` se obtiene del claim `NameIdentifier`/`sub`; no se consulta Identity en cada reserva.
- **Inmueble:** antes de crear o reprogramar una visita, Booking consulta síncronamente:

```http
GET {Services:Properties:BaseUrl}api/Properties/{propertyId}
```

Reenvía el header `Authorization` del usuario y valida:

- `404` → el inmueble no existe.
- `200` con `isAvailable = false` → el inmueble no está disponible.
- `5xx`, timeout o error de red → no se puede validar; se devuelve `503`.

La URL se configura en:

```json
"Services": {
  "Properties": {
    "BaseUrl": "http://localhost:5296/"
  }
}
```

En local se usa HTTP para evitar problemas con el certificado de desarrollo. En un ambiente real debe usarse HTTPS o service discovery.

## Endpoints

| Método | Ruta | Descripción |
|---|---|---|
| GET | `/api/Bookings` | Lista paginada. Un usuario ve sus visitas; `Admin` ve todas. |
| GET | `/api/Bookings/{id}` | Detalle de una visita. |
| POST | `/api/Bookings` | Agenda una visita para el usuario autenticado. |
| PUT | `/api/Bookings/{id}` | Reprograma una visita pendiente o confirmada. |
| POST | `/api/Bookings/{id}/confirm` | Confirma una visita pendiente. |
| POST | `/api/Bookings/{id}/cancel` | Cancela una visita. |
| POST | `/api/Bookings/{id}/complete` | Marca una visita confirmada como completada. |
| DELETE | `/api/Bookings/{id}` | Elimina una visita. Solo rol `Admin`. |

Filtros de listado: `page`, `recordsPerPage`, `filter`, `status`, `propertyId`, `userId` (solo Admin), `from`, `to`.

## Reglas de negocio

- La fecha debe ser futura (mínimo 5 minutos de anticipación).
- La duración debe estar entre 15 y 240 minutos.
- No se permite cruzar dos visitas activas para el mismo inmueble.
- No se permite cruzar dos visitas activas para el mismo usuario.
- Un usuario solo puede ver y modificar sus propias visitas, salvo `Admin`.
- Las transiciones de estado son idempotentes:
  - `Pending -> Confirmed`
  - `Pending/Confirmed -> Cancelled`
  - `Confirmed -> Completed`

## Ejecución

```powershell
dotnet run --project Microservices\Booking\Booking.Api\Booking.Api.csproj
```

Puertos:

- HTTP: `5300`
- HTTPS: `7002`

La base de datos se migra automáticamente al iniciar. Para crear una migración:

```powershell
Microservices\Booking\ef.cmd add InitialCreate
```

## Integración con el gateway

El gateway expone el servicio en:

```
https://localhost:7777/booking/api/Bookings
```

## Pendientes didácticos

- Agregar resiliencia al cliente HTTP (reintentos, circuit breaker).
- Publicar eventos como `BookingCreated`, `BookingConfirmed` o `BookingCancelled`.
- Agregar pruebas de reglas de negocio y pruebas de integración.
- Mover la cadena de conexión y la clave JWT a variables de entorno o user-secrets.

