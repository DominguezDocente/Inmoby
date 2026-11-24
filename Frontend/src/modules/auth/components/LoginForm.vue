<template>
    <form class="form" @submit.prevent="submit">
        <label class="field">
            <span>Correo electrónico</span>
            <input
                v-model.trim="form.email"
                type="email"
                autocomplete="username"
                placeholder="admin@inmoby.local"
                required
                :disabled="loading"
            />
        </label>

        <label class="field">
            <span>Contraseña</span>
            <input
                v-model="form.password"
                type="password"
                autocomplete="current-password"
                required
                :disabled="loading"
            />
        </label>

        <p v-if="error" class="alert alert--error">{{ error }}</p>

        <button class="btn btn--primary btn--block" type="submit" :disabled="loading">
            {{ loading ? 'Ingresando...' : 'Ingresar' }}
        </button>

        <p class="hint">
            Usuario de prueba: <code>admin@inmoby.local</code> / <code>1234*</code>
        </p>
    </form>
</template>

<script>
import { useAuthStore } from '../stores/authStore';

export default {
    name: 'LoginForm',

    data() {

        return {

            form: {
                email: '',
                password: '',
            },
            loading: false,
            error: '',
        };
    },

    methods: {

        async submit() {

            this.error = '';
            this.loading = true;

            try {

                const auth = useAuthStore();

                await auth.login(this.form);

                const redirect = this.$route.query.redirect;
                const target =
                    typeof redirect === 'string' &&
                    redirect.startsWith('/') &&
                    !redirect.startsWith('//')
                        ? redirect
                        : '/';

                this.$router.replace(target);
            } catch (error) {

                this.error =
                    error.status === 401 || error.status >= 500
                        ? 'Credenciales inválidas o el servicio de identidad no está disponible.'
                        : error.message;
            } finally {

                this.loading = false;
            }
        },
    },
};
</script>
