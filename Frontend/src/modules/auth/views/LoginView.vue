<template>
    <div>
        <h1>Iniciar sesión</h1>
        <p class="muted">Accede con tu cuenta de Inmoby.</p>

        <p v-if="expired" class="alert alert--warning">
            Tu sesión expiró. Vuelve a iniciar sesión.
        </p>

        <LoginForm />
    </div>
</template>

<script>
import LoginForm from '../components/LoginForm.vue';

export default {
    name: 'LoginView',
    components: { LoginForm },

    computed: {

        expired() {

            return this.$route.query.expired === '1';
        },
    },
};
</script>
