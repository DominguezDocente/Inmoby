<template>
    <section>
        <p>
            <RouterLink :to="{ name: 'bookings' }">← Volver a reservas</RouterLink>
        </p>

        <header class="page-header">
            <div>
                <h1>{{ isEdit ? 'Reprogramar reserva' : 'Nueva reserva' }}</h1>
                <p class="muted">
                    {{
                        isEdit
                            ? 'Actualiza la fecha, la duración o las notas de la visita.'
                            : 'Agenda una visita a un inmueble disponible.'
                    }}
                </p>
            </div>
        </header>

        <p v-if="initializing" class="muted">Cargando reserva...</p>
        <p v-else-if="initError" class="alert alert--error">{{ initError }}</p>

        <form v-else class="card form" @submit.prevent="submit">
            <label v-if="!isEdit" class="field">
                <span>Propiedad</span>
                <select v-model="form.propertyId" :disabled="submitting" required>
                    <option value="">Selecciona un inmueble</option>
                    <option
                        v-for="property in properties"
                        :key="property.id"
                        :value="property.id"
                        :disabled="!property.isAvailable"
                    >
                        {{ propertyLabel(property) }}
                    </option>
                </select>
            </label>

            <div v-else class="field">
                <span>Propiedad</span>
                <p>
                    <RouterLink
                        :to="{ name: 'property-detail', params: { id: propertyId } }"
                    >
                        {{ propertyTitle || propertyId }}
                    </RouterLink>
                </p>
            </div>

            <label class="field">
                <span>Fecha y hora</span>
                <input
                    v-model="form.scheduledAt"
                    type="datetime-local"
                    :min="minDateTime"
                    :disabled="submitting"
                    required
                />
            </label>

            <label class="field">
                <span>Duración (minutos)</span>
                <input
                    v-model.number="form.durationInMinutes"
                    type="number"
                    min="15"
                    max="120"
                    step="5"
                    :disabled="submitting"
                    required
                />
            </label>

            <label class="field">
                <span>Notas (opcional)</span>
                <textarea
                    v-model.trim="form.notes"
                    maxlength="500"
                    rows="4"
                    :disabled="submitting"
                ></textarea>
            </label>

            <p v-if="editBlocked" class="alert alert--warning">{{ editBlocked }}</p>
            <p v-if="error" class="alert alert--error">{{ error }}</p>

            <div class="actions">
                <button
                    class="btn btn--primary"
                    type="submit"
                    :disabled="submitting || Boolean(editBlocked)"
                >
                    {{ submitting ? 'Guardando...' : (isEdit ? 'Guardar cambios' : 'Crear reserva') }}
                </button>

                <RouterLink class="btn" :to="{ name: 'bookings' }">Cancelar</RouterLink>
            </div>
        </form>
    </section>
</template>

<script>
import { bookingsService } from '../services/bookingsService';
import { isConfirmed, isPending } from '../constants/bookingStatus';
import { minimumBookingDateTime, toDateTimeLocal } from '../utils/dateTime';
import { useAuthStore } from '@/modules/auth/stores/authStore';
import { propertiesService } from '@/modules/properties/services/propertiesService';

export default {
    name: 'BookingFormView',

    props: {

        id: {
            type: String,
            default: '',
        },
    },

    data() {

        return {

            form: {
                propertyId: '',
                scheduledAt: '',
                durationInMinutes: 30,
                notes: '',
            },
            properties: [],
            propertyId: '',
            propertyTitle: '',
            booking: null,
            initializing: false,
            initError: '',
            submitting: false,
            error: '',
        };
    },

    computed: {

        auth() {

            return useAuthStore();
        },

        isEdit() {

            return Boolean(this.id);
        },

        minDateTime() {

            return minimumBookingDateTime(5);
        },

        editBlocked() {

            if (!this.isEdit || !this.booking) return '';

            if (isPending(this.booking.status) || isConfirmed(this.booking.status)) return '';

            return 'Solo se pueden reprogramar reservas pendientes o confirmadas.';
        },
    },

    created() {

        this.initialize();
    },

    methods: {

        propertyLabel(property) {

            const place = property.cityName ? ` · ${property.cityName}` : '';

            return `${property.title}${place}${property.isAvailable ? '' : ' (no disponible)'}`;
        },

        async initialize() {

            this.initializing = true;
            this.initError = '';

            try {

                if (this.isEdit) {

                    await this.loadBooking();
                } else {

                    await this.loadProperties();
                }
            } catch (error) {

                this.initError = error.message;
            } finally {

                this.initializing = false;
            }
        },

        async loadProperties() {

            const page = await propertiesService.list({ pageSize: 50 });

            this.properties = page.items;

            const preselectedId =
                typeof this.$route.query.propertyId === 'string'
                    ? this.$route.query.propertyId
                    : '';

            if (!preselectedId) return;

            this.form.propertyId = preselectedId;

            if (this.properties.some((property) => property.id === preselectedId)) return;

            try {

                const property = await propertiesService.getById(preselectedId);

                this.properties = [property, ...this.properties];
            } catch {

                // Si el inmueble preseleccionado no se puede cargar, se conserva el resto del listado.
            }
        },

        async loadBooking() {

            const booking = await bookingsService.getById(this.id);

            this.booking = booking;
            this.propertyId = booking.propertyId;
            this.form.scheduledAt = toDateTimeLocal(booking.scheduledAt);
            this.form.durationInMinutes = booking.durationInMinutes;
            this.form.notes = booking.notes || '';

            try {

                const property = await propertiesService.getById(booking.propertyId);

                this.propertyTitle = property.title;
            } catch {

                this.propertyTitle = '';
            }
        },

        async submit() {

            this.error = '';

            if (this.editBlocked) {

                this.error = this.editBlocked;
                return;
            }

            if (!this.isEdit && !this.form.propertyId) {

                this.error = 'Selecciona una propiedad.';
                return;
            }

            if (!this.isEdit) {

                const selectedProperty = this.properties.find(
                    (property) => property.id === this.form.propertyId
                );

                if (selectedProperty && !selectedProperty.isAvailable) {

                    this.error = 'La propiedad no está disponible para visitas.';
                    return;
                }
            }

            if (!this.form.scheduledAt) {

                this.error = 'Selecciona la fecha y la hora de la visita.';
                return;
            }

            const scheduledAt = new Date(this.form.scheduledAt);

            if (Number.isNaN(scheduledAt.getTime())) {

                this.error = 'La fecha y la hora no son válidas.';
                return;
            }

            if (scheduledAt.getTime() <= Date.now() + 5 * 60 * 1000) {

                this.error = 'La visita debe agendarse con al menos 5 minutos de anticipación.';
                return;
            }

            const durationInMinutes = Number(this.form.durationInMinutes);

            if (
                !Number.isFinite(durationInMinutes) ||
                durationInMinutes < 15 ||
                durationInMinutes > 120
            ) {

                this.error = 'La duración debe estar entre 15 y 120 minutos.';
                return;
            }

            this.submitting = true;

            try {

                const payload = {
                    scheduledAt: scheduledAt.toISOString(),
                    durationInMinutes,
                    notes: this.form.notes || null,
                };

                if (this.isEdit) {

                    await bookingsService.update(this.id, payload);
                } else {

                    await bookingsService.create({
                        propertyId: this.form.propertyId,
                        userId: this.auth.user?.id || '',
                        ...payload,
                    });
                }

                this.$router.push({
                    name: 'bookings',
                    query: {
                        notice: this.isEdit ? 'updated' : 'created',
                    },
                });
            } catch (error) {

                this.error = error.message;
            } finally {

                this.submitting = false;
            }
        },
    },
};
</script>
