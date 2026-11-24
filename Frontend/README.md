# Inmoby Frontend

Frontend sencillo en **Vue 3 + Options API** (JavaScript, sin TypeScript) para consumir los microservicios de Inmoby a través del **API Gateway** de la rama `master`.

Incluye:

- Vue Router con rutas protegidas y autorización por roles.
- Pinia para sesión, propiedades y reservas.
- Cliente HTTP basado en **Axios**, con interceptores para JWT y manejo centralizado de errores/401.
- Organización por módulos de dominio.

## Requisitos

- Node.js 20.19 o superior.
- API Gateway, Identity, Properties y Booking en ejecución.

## Puesta en marcha

```bash
cd Frontend
npm install
npm run dev
```

La aplicación queda en `http://localhost:5173` (si el puerto está ocupado, Vite elige el siguiente).

Credenciales sembradas por Identity:

| Usuario | Contraseña | Roles |
| --- | --- | --- |
| `admin@inmoby.local` | `1234*` | Admin, Properties Manager |
| `propertymanager@inmoby.local` | `1234` | Properties Manager |

## Configuración

Variables en `.env.development` (ver `.env.example`):

| Variable | Descripción |
| --- | --- |
| `VITE_API_BASE_URL` | URL base del gateway. Vacío usa el mismo origen y el proxy de Vite. |
| `VITE_GATEWAY_URL` | Destino del proxy de desarrollo. Por defecto `https://localhost:7777`. |

En desarrollo, Vite proxya `/identity`, `/properties` y `/bookings` hacia el gateway. Las rutas del SPA usan nombres en español (`/propiedades`, `/reservas`, `/usuarios`) para no chocar con esos prefijos.

En producción hay dos opciones:

1. Servir el SPA detrás del mismo dominio y reversar `/identity`, `/properties` y `/bookings` al gateway, con fallback a `index.html` para las rutas del SPA.
2. Definir `VITE_API_BASE_URL` con la URL del gateway y habilitar CORS en el gateway.

## Rutas del gateway (rama `master`)

En `master` los destinos del gateway incluyen `/api/`, por lo que la ruta pública no lleva un `/api` adicional:

| Servicio | Ruta consumida por el frontend |
| --- | --- |
| Login | `POST /identity/auth/login` |
| Usuarios | `GET /identity/users` |
| Propiedades | `GET/POST/PUT /properties/properties` |
| Detalle propiedad | `GET /properties/properties/{id}` |
| Tipos de inmueble | `/properties/propertytypes` |
| Países | `/properties/locations/countries` |
| Departamentos/estados | `/properties/locations/states` |
| Ciudades | `/properties/locations/cities` |
| Barrios | `/properties/locations/neighborhoods` |
| Reservas | `/bookings/bookings` |
| Confirmar/completar/cancelar reserva | `POST /bookings/bookings/{id}/{confirm|complete|cancel}` |
| Editar reserva | `PUT /bookings/bookings/{id}` |

> La ruta `/bookings` se agregó en `Microservices/ApiGateway/ApiGateway.App/appsettings.json` para exponer el microservicio de Booking, usando el mismo estilo de destino que Identity y Properties.

## Flujo de autenticación

1. `LoginForm` llama a `authService.login()` → `POST /identity/auth/login`.
2. El token se guarda en `localStorage` mediante `core/auth/tokenStorage.js`.
3. `authStore` decodifica los claims del JWT (`sub`, `name`, `fisrtName`, `lastName`, `document`, `role`) para exponer usuario, roles y expiración.
4. El router bloquea rutas con `meta.requiresAuth` y valida `meta.roles`.
5. Cualquier respuesta `401` dispara el cierre de sesión global y redirige a `/login`.
6. Una respuesta `403` se muestra como error de permisos.

## Estructura

```text
src/
  core/
    api/httpClient.js          # Axios + interceptores JWT + desempaquetado de ApiResponse/Result
    auth/token.js              # decodificación de JWT y roles
    auth/tokenStorage.js
    utils/format.js
  components/                  # navbar y paginación
  layouts/                     # layouts público y autenticado
  router/index.js              # compone rutas de módulos, layouts y guards
  modules/
    auth/
      routes.js
      components/LoginForm.vue
      services/authService.js
      stores/authStore.js
      views/LoginView.vue
    users/
      routes.js
      services/usersService.js
      views/UsersView.vue
    properties/
      routes.js
      components/PropertyCard.vue
      constants/propertyEnums.js
      services/propertiesService.js
      stores/propertiesStore.js
      utils/validation.js
      views/PropertiesView.vue
      views/PropertyCreateView.vue
      views/PropertyDetailView.vue
    propertyTypes/
      services/propertyTypesService.js
    locations/
      services/locationsService.js
    bookings/
      routes.js
      constants/bookingStatus.js
      services/bookingsService.js
      stores/bookingsStore.js
      utils/dateTime.js
      views/BookingsView.vue
      views/BookingFormView.vue
  views/                       # dashboard y páginas de error
```

Cada módulo mantiene sus rutas, servicios, stores, componentes y vistas. `src/router/index.js` solo compone las rutas de los módulos dentro de los layouts, define las rutas globales (dashboard, 403 y 404) y aplica los guards.

## Funcionalidades principales

- Listado de propiedades con filtros por tipo de inmueble, ciudad y estrato.
- Crear propiedades con catálogos reales de `propertytypes` y `neighborhoods`. La ruta `/propiedades/nueva` valida los roles `Admin` y `Properties Manager`.
- Ver el detalle de una propiedad con su galería de imágenes.
- Crear reservas desde el listado o desde el detalle de una propiedad.
- Reprogramar reservas pendientes o confirmadas.
- Confirmar, completar y cancelar reservas (con motivo opcional).

> La creación de propiedades se valida en el frontend. El endpoint `POST /properties/properties` de `master` solo exige autenticación; si se necesita una restricción definitiva debe agregarse también en el backend.
>
> El detalle de propiedad de `master` solo permite los roles `Admin` y `User`. Por eso el frontend oculta el detalle a `Properties Manager` y, si ese rol crea una propiedad, lo redirige al listado.
>
> `master` no expone endpoints para asignar imágenes a una propiedad ni un servicio Media. El frontend solo muestra la galería que ya venga en el detalle.

## Particularidades de los contratos

- Properties e Identity responden con el sobre `{ isSuccess, data, message, errors }`.
- Booking responde con `{ isSuccess, data, message, errors }`.
- Booking serializa los estados como números (`1` pendiente, `2` confirmada, `3` cancelada, `4` completada) y usa `durationInMinutes`.
- Al crear una reserva, `master` exige `userId` en el DTO; se envía el usuario autenticado.
- En Properties, `amenities` es opcional y el backend acepta `null`, `[]` o una lista de códigos.

## Notas

- El JWT se guarda en `localStorage` para simplificar el ejemplo. En producción conviene evaluar cookies `HttpOnly` o un backend-for-frontend.
- El backend de Identity devuelve `500` ante credenciales inválidas (lanza excepción en lugar de responder `401`); el formulario muestra un mensaje genérico para ambos casos.
- Si el gateway no puede alcanzar los microservicios por HTTPS, revisa la confianza del certificado de desarrollo (`dotnet dev-certs https --trust`) y que los destinos del gateway coincidan con los puertos de los proyectos.
