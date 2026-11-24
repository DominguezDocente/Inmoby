<template>
    <section>
        <header class="page-header">
            <div>
                <h1>Propiedades</h1>
                <p class="muted">{{ totalCount }} inmueble(s) encontrado(s).</p>
            </div>

            <div class="actions">
                <RouterLink
                    v-if="auth.canManageProperties"
                    class="btn btn--primary"
                    :to="{ name: 'property-create' }"
                >
                    Crear propiedad
                </RouterLink>

                <button class="btn" type="button" :disabled="loading" @click="loadPage(pageNumber)">
                    Recargar
                </button>
            </div>
        </header>

        <p v-if="notice" class="alert alert--success">{{ notice }}</p>

        <form class="filters card" @submit.prevent="applyFilters">
            <label class="field">
                <span>Tipo de inmueble</span>
                <select v-model="filters.propertyTypeId" :disabled="loading">
                    <option value="">Todos</option>
                    <option v-for="type in propertyTypes" :key="type.id" :value="type.id">
                        {{ type.name }}
                    </option>
                </select>
            </label>

            <label class="field">
                <span>Ciudad</span>
                <select v-model="filters.cityId" :disabled="loading">
                    <option value="">Todas</option>
                    <option v-for="city in cities" :key="city.id" :value="city.id">
                        {{ city.name }}
                    </option>
                </select>
            </label>

            <label class="field">
                <span>Estrato</span>
                <select v-model="filters.stratum" :disabled="loading">
                    <option value="">Todos</option>
                    <option v-for="number in 6" :key="number" :value="number">{{ number }}</option>
                </select>
            </label>

            <div class="filters__actions">
                <button class="btn btn--primary" type="submit" :disabled="loading">Filtrar</button>
                <button class="btn" type="button" :disabled="loading" @click="clearFilters">
                    Limpiar
                </button>
            </div>
        </form>

        <p v-if="catalogsError" class="alert alert--warning">{{ catalogsError }}</p>
        <p v-if="error" class="alert alert--error">{{ error }}</p>
        <p v-if="loading" class="muted">Cargando propiedades...</p>

        <div v-else-if="items.length" class="grid grid--cards">
            <PropertyCard
                v-for="item in items"
                :key="item.id"
                :property="item"
                :linkable="auth.canViewPropertyDetails"
            />
        </div>

        <p v-else-if="!error" class="card empty">
            No hay propiedades para los filtros seleccionados.
        </p>

        <AppPagination
            :page="pageNumber"
            :total-pages="totalPages"
            :has-previous="hasPreviousPage"
            :has-next="hasNextPage"
            @change="loadPage"
        />
    </section>
</template>

<script>
import { mapActions, mapState } from 'pinia';

import AppPagination from '@/components/AppPagination.vue';
import { useAuthStore } from '@/modules/auth/stores/authStore';
import { locationsService } from '@/modules/locations/services/locationsService';
import { propertyTypesService } from '@/modules/propertyTypes/services/propertyTypesService';
import PropertyCard from '../components/PropertyCard.vue';
import { usePropertiesStore } from '../stores/propertiesStore';

export default {
    name: 'PropertiesView',
    components: { AppPagination, PropertyCard },

    data() {

        return {
            propertyTypes: [],
            cities: [],
            catalogsError: '',
            filters: {
                propertyTypeId: '',
                cityId: '',
                stratum: '',
            },
        };
    },

    computed: {

        ...mapState(usePropertiesStore, [
            'items',
            'totalCount',
            'pageNumber',
            'totalPages',
            'hasPreviousPage',
            'hasNextPage',
            'loading',
            'error',
        ]),

        auth() {

            return useAuthStore();
        },

        notice() {

            return this.$route.query.notice === 'created'
                ? 'Propiedad creada correctamente.'
                : '';
        },
    },

    created() {

        this.loadCatalogs();
        this.loadPage(1);
    },

    methods: {

        ...mapActions(usePropertiesStore, ['fetchList']),

        async loadCatalogs() {

            this.catalogsError = '';

            try {

                const [typesPage, citiesPage] = await Promise.all([
                    propertyTypesService.list({ pageSize: 50 }),
                    locationsService.cities.list({ pageSize: 50 }),
                ]);

                this.propertyTypes = typesPage.items;
                this.cities = citiesPage.items;
            } catch (error) {

                this.catalogsError = `No se pudieron cargar los catálogos: ${error.message}`;
            }
        },

        loadPage(pageNumber) {

            return this.fetchList({
                pageNumber,
                propertyTypeId: this.filters.propertyTypeId,
                cityId: this.filters.cityId,
                stratum: this.filters.stratum,
            });
        },

        applyFilters() {

            this.loadPage(1);
        },

        clearFilters() {

            this.filters = {
                propertyTypeId: '',
                cityId: '',
                stratum: '',
            };

            this.loadPage(1);
        },
    },
};
</script>
