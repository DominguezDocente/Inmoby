<template>
    <section>
        <header class="page-header">
            <div>
                <h1>Hola, {{ user?.firstName || user?.email || 'usuario' }}</h1>
                <p class="muted">Sesión iniciada contra el API Gateway con JWT.</p>
            </div>
        </header>

        <div class="grid grid--3">
            <article class="card stat">
                <span class="stat__label">Usuario</span>
                <strong>{{ user?.email || '—' }}</strong>
            </article>

            <article class="card stat">
                <span class="stat__label">Roles</span>
                <strong>{{ rolesText }}</strong>
            </article>

            <article class="card stat">
                <span class="stat__label">Token expira</span>
                <strong>{{ expiresText }}</strong>
            </article>
        </div>

        <div class="grid grid--2">
            <RouterLink class="card link-card" :to="{ name: 'properties' }">
                <h2>Propiedades</h2>
                <p>Consulta el listado paginado y el detalle de cada inmueble.</p>
            </RouterLink>

            <RouterLink class="card link-card" :to="{ name: 'bookings' }">
                <h2>Reservas</h2>
                <p>Revisa las visitas agendadas y gestiona su estado.</p>
            </RouterLink>

            <RouterLink v-if="auth.isAdmin" class="card link-card" :to="{ name: 'users' }">
                <h2>Usuarios</h2>
                <p>Listado de usuarios del sistema. Disponible solo para el rol Admin.</p>
            </RouterLink>
        </div>
    </section>
</template>

<script>
import { formatDate } from '@/core/utils/format';
import { useAuthStore } from '@/modules/auth/stores/authStore';

export default {
    name: 'DashboardView',

    computed: {

        auth() {

            return useAuthStore();
        },

        user() {

            return this.auth.user;
        },

        rolesText() {

            return this.auth.roles.length ? this.auth.roles.join(' · ') : 'Sin roles';
        },

        expiresText() {

            return formatDate(this.auth.expiresAt);
        },
    },
};
</script>
