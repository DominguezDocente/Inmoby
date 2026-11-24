<template>
    <section class="standalone-page">
        <div class="card">
            <h1>Acceso denegado</h1>
            <p class="muted">
                Tu sesión es válida, pero no tienes el rol necesario para ver esta sección.
            </p>

            <p>
                Roles actuales:
                <strong>{{ rolesText }}</strong>
            </p>

            <div class="actions">
                <RouterLink class="btn btn--primary" :to="{ name: 'dashboard' }">
                    Volver al inicio
                </RouterLink>
            </div>
        </div>
    </section>
</template>

<script>
import { useAuthStore } from '@/modules/auth/stores/authStore';

export default {
    name: 'ForbiddenView',

    computed: {

        auth() {

            return useAuthStore();
        },

        rolesText() {

            return this.auth.roles.length ? this.auth.roles.join(' · ') : 'Sin roles';
        },
    },
};
</script>
