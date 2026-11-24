<template>
    <article class="property-card">
        <div class="property-card__header">
            <span class="badge badge--soft">{{ property.propertyTypeName }}</span>
            <span class="badge" :class="property.isAvailable ? 'badge--ok' : 'badge--muted'">
                {{ property.isAvailable ? 'Disponible' : 'No disponible' }}
            </span>
        </div>

        <h2 class="property-card__title">
            <RouterLink v-if="linkable" :to="{ name: 'property-detail', params: { id: property.id } }">
                {{ property.title }}
            </RouterLink>
            <span v-else>{{ property.title }}</span>
        </h2>

        <p class="muted">{{ property.cityName }}</p>
        <p class="property-card__price">
            {{ formatCurrency(property.priceAmount, property.priceCurrencyCode) }}
        </p>

        <ul class="property-card__meta">
            <li>{{ property.bedrooms }} hab.</li>
            <li>{{ property.bathRooms }} baños</li>
            <li>{{ property.parkingSpaces }} parq.</li>
            <li>{{ property.area }} m²</li>
            <li>Estrato {{ property.stratum }}</li>
        </ul>

        <div v-if="linkable" class="property-card__actions">
            <RouterLink
                class="btn btn--sm"
                :to="{ name: 'property-detail', params: { id: property.id } }"
            >
                Ver propiedad
            </RouterLink>
        </div>
    </article>
</template>

<script>
import { formatCurrency } from '@/core/utils/format';

export default {
    name: 'PropertyCard',

    props: {

        property: {
            type: Object,
            required: true,
        },
        linkable: {
            type: Boolean,
            default: true,
        },
    },

    methods: {

        formatCurrency,
    },
};
</script>
