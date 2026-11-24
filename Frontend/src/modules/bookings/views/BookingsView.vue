<template>
    <section>
        <header class="page-header">
            <div>
                <h1>Reservas</h1>
                <p class="muted">
                    {{ totalCount }} visita(s). Los administradores ven todas; los demás usuarios
                    solo las propias.
                </p>
            </div>

            <div class="actions">
                <RouterLink class="btn btn--primary" :to="{ name: 'booking-create' }">
                    Nueva reserva
                </RouterLink>

                <button class="btn" type="button" :disabled="loading" @click="loadPage(currentPage)">
                    Recargar
                </button>
            </div>
        </header>

        <p v-if="notice" class="alert alert--success">{{ notice }}</p>
        <p v-if="error" class="alert alert--error">{{ error }}</p>
        <p v-if="actionError" class="alert alert--error">{{ actionError }}</p>
        <p v-if="loading" class="muted">Cargando reservas...</p>

        <div v-else class="card table-wrap">
            <table class="table">
                <thead>
                    <tr>
                        <th>Fecha</th>
                        <th>Propiedad</th>
                        <th>Estado</th>
                        <th>Duración</th>
                        <th>Notas</th>
                        <th>Acciones</th>
                    </tr>
                </thead>

                <tbody>
                    <tr v-if="items.length === 0">
                        <td class="empty" colspan="6">No hay reservas registradas.</td>
                    </tr>

                    <tr v-for="booking in items" :key="booking.id">
                        <td>{{ formatDate(booking.scheduledAt) }}</td>
                        <td>
                            <RouterLink
                                v-if="auth.canViewPropertyDetails"
                                :to="{ name: 'property-detail', params: { id: booking.propertyId } }"
                            >
                                Ver propiedad
                            </RouterLink>
                            <span v-else class="muted">—</span>
                        </td>
                        <td>
                            <span class="badge" :class="bookingStatusBadge(booking.status)">
                                {{ bookingStatusLabel(booking.status) }}
                            </span>
                        </td>
                        <td>{{ booking.durationInMinutes }} min</td>
                        <td>{{ booking.notes || '—' }}</td>
                        <td>
                            <div class="table__actions">
                                <RouterLink
                                    v-if="isPending(booking.status) || isConfirmed(booking.status)"
                                    class="btn btn--sm"
                                    :to="{ name: 'booking-edit', params: { id: booking.id } }"
                                >
                                    Editar
                                </RouterLink>

                                <button
                                    v-if="isPending(booking.status)"
                                    class="btn btn--sm"
                                    type="button"
                                    :disabled="actionId === booking.id"
                                    @click="runAction(booking.id, 'confirm')"
                                >
                                    Confirmar
                                </button>

                                <button
                                    v-if="isConfirmed(booking.status)"
                                    class="btn btn--sm"
                                    type="button"
                                    :disabled="actionId === booking.id"
                                    @click="runAction(booking.id, 'complete')"
                                >
                                    Completar
                                </button>

                                <button
                                    v-if="isPending(booking.status) || isConfirmed(booking.status)"
                                    class="btn btn--sm btn--danger"
                                    type="button"
                                    :disabled="actionId === booking.id"
                                    @click="runAction(booking.id, 'cancel')"
                                >
                                    Cancelar
                                </button>
                            </div>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>

        <AppPagination
            :page="currentPage"
            :total-pages="totalPages"
            :has-previous="hasPrevious"
            :has-next="hasNext"
            @change="loadPage"
        />
    </section>
</template>

<script>
import { mapActions, mapState } from 'pinia';

import AppPagination from '@/components/AppPagination.vue';
import { useAuthStore } from '@/modules/auth/stores/authStore';
import { formatDate } from '@/core/utils/format';
import { bookingsService } from '../services/bookingsService';
import { useBookingsStore } from '../stores/bookingsStore';
import {
    bookingStatusBadge,
    bookingStatusLabel,
    isConfirmed,
    isPending,
} from '../constants/bookingStatus';

export default {
    name: 'BookingsView',
    components: { AppPagination },

    data() {

        return {
            actionId: null,
            actionError: '',
        };
    },

    computed: {

        ...mapState(useBookingsStore, [
            'items',
            'totalCount',
            'currentPage',
            'totalPages',
            'hasPrevious',
            'hasNext',
            'loading',
            'error',
        ]),

        auth() {

            return useAuthStore();
        },

        notice() {

            const notice = this.$route.query.notice;

            if (notice === 'created') return 'Reserva creada correctamente.';
            if (notice === 'updated') return 'Reserva actualizada correctamente.';

            return '';
        },
    },

    created() {

        this.loadPage(1);
    },

    methods: {

        ...mapActions(useBookingsStore, ['fetchList']),

        formatDate,
        bookingStatusLabel,
        bookingStatusBadge,
        isPending,
        isConfirmed,

        loadPage(page) {

            return this.fetchList({ page });
        },

        async runAction(id, action) {

            let payload = {};

            if (action === 'cancel') {

                const reason = window.prompt('Motivo de la cancelación (opcional):');

                if (reason === null) return;

                payload = {
                    reason: reason.trim() || null,
                };
            }

            this.actionId = id;
            this.actionError = '';

            try {

                if (action === 'confirm') await bookingsService.confirm(id);
                if (action === 'complete') await bookingsService.complete(id);
                if (action === 'cancel') await bookingsService.cancel(id, payload);

                await this.loadPage(this.currentPage);
            } catch (error) {

                this.actionError = error.message;
            } finally {

                this.actionId = null;
            }
        },
    },
};
</script>
