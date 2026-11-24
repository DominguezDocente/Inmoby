<template>
    <section>
        <header class="page-header">
            <div>
                <h1>Usuarios</h1>
                <p class="muted">Solo administradores pueden consultar este listado.</p>
            </div>

            <button class="btn" type="button" :disabled="loading" @click="load">
                {{ loading ? 'Cargando...' : 'Recargar' }}
            </button>
        </header>

        <p v-if="error" class="alert alert--error">{{ error }}</p>

        <div v-else class="card table-wrap">
            <table class="table">
                <thead>
                    <tr>
                        <th>Nombre</th>
                        <th>Correo</th>
                        <th>Documento</th>
                        <th>Roles</th>
                    </tr>
                </thead>

                <tbody>
                    <tr v-if="!loading && users.length === 0">
                        <td class="empty" colspan="4">No hay usuarios registrados.</td>
                    </tr>

                    <tr v-for="user in users" :key="user.id">
                        <td>{{ user.firstName }} {{ user.lastName }}</td>
                        <td>{{ user.email }}</td>
                        <td>{{ user.document }}</td>
                        <td>
                            <span v-for="role in user.roles" :key="role" class="badge">
                                {{ role }}
                            </span>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </section>
</template>

<script>
import { usersService } from '../services/usersService';

export default {
    name: 'UsersView',

    data() {

        return {

            users: [],
            loading: false,
            error: '',
        };
    },

    created() {

        this.load();
    },

    methods: {

        async load() {

            this.loading = true;
            this.error = '';

            try {

                this.users = await usersService.list();
            } catch (error) {

                this.error = error.message;
            } finally {

                this.loading = false;
            }
        },
    },
};
</script>
