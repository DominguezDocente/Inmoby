<template>
    <section>
        <p>
            <RouterLink :to="{ name: 'properties' }">← Volver a propiedades</RouterLink>
        </p>

        <p v-if="notice" class="alert alert--success">{{ notice }}</p>

        <p v-if="loading" class="muted">Cargando inmueble...</p>
        <p v-else-if="error" class="alert alert--error">{{ error }}</p>

        <template v-else-if="property">
            <header class="page-header">
                <div>
                    <h1>{{ property.title }}</h1>
                    <p class="muted">
                        {{ property.propertyTypeName }} · {{ property.neighborhoodName }},
                        {{ property.cityName }}
                    </p>
                </div>

                <span
                    v-if="property.isAvailable !== undefined"
                    class="badge"
                    :class="property.isAvailable ? 'badge--ok' : 'badge--muted'"
                >
                    {{ property.isAvailable ? 'Disponible' : 'No disponible' }}
                </span>
            </header>

            <div class="actions detail-actions">
                <RouterLink
                    v-if="property.isAvailable !== false"
                    class="btn btn--primary"
                    :to="{ name: 'booking-create', query: { propertyId: property.id } }"
                >
                    Agendar visita
                </RouterLink>
            </div>

            <div class="detail-grid">
                <article class="card">
                    <h2>Descripción</h2>
                    <p>{{ property.description }}</p>

                    <h2>Características</h2>
                    <dl class="detail-list">
                        <div>
                            <dt>Precio</dt>
                            <dd>{{ formatCurrency(property.priceAmount, property.currencyCode) }}</dd>
                        </div>
                        <div>
                            <dt>Habitaciones</dt>
                            <dd>{{ property.bedrooms }}</dd>
                        </div>
                        <div>
                            <dt>Baños</dt>
                            <dd>{{ property.bathrooms }}</dd>
                        </div>
                        <div>
                            <dt>Parqueaderos</dt>
                            <dd>{{ property.parkingSpaces }}</dd>
                        </div>
                        <div>
                            <dt>Área</dt>
                            <dd>{{ property.area }} m²</dd>
                        </div>
                        <div>
                            <dt>Estrato</dt>
                            <dd>{{ property.stratum }}</dd>
                        </div>
                        <div>
                            <dt>Condición</dt>
                            <dd>{{ conditionLabel }}</dd>
                        </div>
                        <div>
                            <dt>Publicada</dt>
                            <dd>{{ formatDate(property.createdAt) }}</dd>
                        </div>
                    </dl>

                    <template v-if="property.amenities?.length">
                        <h2>Comodidades</h2>
                        <p>
                            <span
                                v-for="amenity in property.amenities"
                                :key="amenity"
                                class="badge badge--soft"
                            >
                                {{ amenity }}
                            </span>
                        </p>
                    </template>
                </article>

                <aside class="card">
                    <h2>Dirección</h2>
                    <p>{{ addressText }}</p>
                    <p v-if="property.address?.indications" class="muted">
                        {{ property.address.indications }}
                    </p>
                    <p v-if="property.address?.postalCode" class="muted">
                        Código postal: {{ property.address.postalCode }}
                    </p>

                    <h2>Imágenes</h2>
                    <p v-if="!property.images?.length" class="muted">Sin imágenes registradas.</p>
                    <ul v-else class="image-list">
                        <li v-for="image in property.images" :key="image.id">
                            <a :href="image.url" target="_blank" rel="noopener">
                                <img
                                    class="image-list__thumb"
                                    :src="image.url"
                                    :alt="image.description || property.title"
                                    loading="lazy"
                                />
                            </a>
                            <span>{{ image.description || 'Sin descripción' }}</span>
                        </li>
                    </ul>
                </aside>
            </div>
        </template>
    </section>
</template>

<script>
import { formatCurrency, formatDate } from '@/core/utils/format';
import { propertiesService } from '../services/propertiesService';
import { PROPERTY_CONDITIONS, ROAD_SUFFIXES, ROAD_TYPES } from '../constants/propertyEnums';

export default {
    name: 'PropertyDetailView',

    props: {

        id: {
            type: String,
            required: true,
        },
    },

    data() {

        return {
            property: null,
            loading: false,
            error: '',
        };
    },

    computed: {

        notice() {

            return this.$route.query.notice === 'created'
                ? 'Propiedad creada correctamente.'
                : '';
        },

        conditionLabel() {

            return PROPERTY_CONDITIONS[this.property?.condition] || 'Sin definir';
        },

        addressText() {

            const address = this.property?.address;

            if (!address) return '—';

            const main = [
                ROAD_TYPES[address.mainRoadType],
                address.mainRoadNumber,
                address.mainRoadLetter,
                ROAD_SUFFIXES[address.mainRoadSuffix],
            ]
                .filter(Boolean)
                .join(' ');

            const cross = [
                address.crossRoadNumber,
                address.crossRoadLetter,
                ROAD_SUFFIXES[address.crossRoadSuffix],
            ]
                .filter(Boolean)
                .join(' ');

            if (!cross) return main || '—';

            return `${main} # ${cross}${address.plate ? `-${address.plate}` : ''}`;
        },
    },

    watch: {

        id() {

            this.load();
        },
    },

    created() {

        this.load();
    },

    methods: {

        formatCurrency,
        formatDate,

        async load() {

            this.loading = true;
            this.error = '';
            this.property = null;

            try {

                this.property = await propertiesService.getById(this.id);
            } catch (error) {

                this.error = error.message;
            } finally {

                this.loading = false;
            }
        },
    },
};
</script>
