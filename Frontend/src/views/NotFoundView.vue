<template>
    <section class="standalone-page">
        <div class="card">
            <h1>Página no encontrada</h1>
            <p class="muted">La ruta solicitada no existe en el frontend.</p>

            <div class="actions">
                <RouterLink
                    class="btn btn--primary"
                    :to="{ name: authenticated ? 'dashboard' : 'login' }"
                >
                    {{ authenticated ? 'Ir al inicio' : 'Iniciar sesión' }}
                </RouterLink>
            </div>
        </div>
    </section>
</template>

<script>
import { useAuthStore } from '@/modules/auth/stores/authStore';

export default {
    name: 'NotFoundView',

    computed: {

        authenticated() {

            return useAuthStore().isAuthenticated;
        },
    },
};
</script>
