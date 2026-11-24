<template>
    <header class="navbar">
        <div class="navbar__inner">
            <RouterLink class="navbar__brand" :to="{ name: 'dashboard' }">Inmoby</RouterLink>

            <nav class="navbar__links">
                <RouterLink :to="{ name: 'properties' }">Propiedades</RouterLink>
                <RouterLink :to="{ name: 'bookings' }">Reservas</RouterLink>
                <RouterLink v-if="auth.isAdmin" :to="{ name: 'users' }">Usuarios</RouterLink>
            </nav>

            <div class="navbar__user">
                <div class="navbar__identity">
                    <span class="navbar__email">{{ user?.email }}</span>
                    <span class="navbar__roles">{{ rolesText }}</span>
                </div>

                <button class="btn btn--ghost btn--sm" type="button" @click="logout">
                    Salir
                </button>
            </div>
        </div>
    </header>
</template>

<script>
import { useAuthStore } from '@/modules/auth/stores/authStore';

export default {
    name: 'AppNavbar',

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
    },

    methods: {

        logout() {

            this.auth.logout();
            this.$router.push({ name: 'login' });
        },
    },
};
</script>
