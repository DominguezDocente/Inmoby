<template>
    <section>
        <p>
            <RouterLink :to="{ name: 'properties' }">← Volver a propiedades</RouterLink>
        </p>

        <header class="page-header">
            <div>
                <h1>Crear propiedad</h1>
                <p class="muted">Registra un inmueble en el sistema.</p>
            </div>
        </header>

        <p v-if="!canCreate" class="alert alert--error">
            No tienes permisos para crear propiedades.
        </p>

        <p v-if="catalogsError" class="alert alert--warning">{{ catalogsError }}</p>

        <form v-else class="form" @submit.prevent="submit">
            <section class="card form-section">
                <h2>Información general</h2>

                <div class="form-grid">
                    <label class="field">
                        <span>Título</span>
                        <input
                            v-model.trim="form.title"
                            type="text"
                            minlength="4"
                            maxlength="256"
                            required
                            :disabled="submitting"
                        />
                    </label>

                    <label class="field">
                        <span>Precio</span>
                        <input
                            v-model.number="form.priceAmount"
                            type="number"
                            min="1"
                            step="0.01"
                            required
                            :disabled="submitting"
                        />
                    </label>

                    <label class="field">
                        <span>Moneda</span>
                        <select v-model="form.currencyCode" :disabled="submitting">
                            <option
                                v-for="currency in currencies"
                                :key="currency"
                                :value="currency"
                            >
                                {{ currency }}
                            </option>
                        </select>
                    </label>

                    <label class="field">
                        <span>ID del propietario</span>
                        <input
                            v-model.trim="form.ownerId"
                            type="text"
                            required
                            :disabled="submitting"
                        />
                        <small class="hint">Por defecto se usa tu usuario: {{ ownerEmail }}</small>
                    </label>

                    <label class="field">
                        <span>Tipo de inmueble</span>
                        <select
                            v-model="form.propertyTypeId"
                            required
                            :disabled="submitting || catalogsLoading"
                        >
                            <option value="">Selecciona un tipo</option>
                            <option v-for="type in propertyTypes" :key="type.id" :value="type.id">
                                {{ type.name }}
                            </option>
                        </select>
                    </label>

                    <label class="field">
                        <span>Barrio</span>
                        <select
                            v-model="form.neighborhoodId"
                            required
                            :disabled="submitting || catalogsLoading"
                        >
                            <option value="">Selecciona un barrio</option>
                            <option
                                v-for="neighborhood in neighborhoods"
                                :key="neighborhood.id"
                                :value="neighborhood.id"
                            >
                                {{ neighborhood.name }}
                            </option>
                        </select>
                    </label>
                </div>

                <label class="field">
                    <span>Descripción</span>
                    <textarea
                        v-model.trim="form.description"
                        minlength="32"
                        maxlength="4000"
                        rows="4"
                        required
                        :disabled="submitting"
                    ></textarea>
                </label>
            </section>

            <section class="card form-section">
                <h2>Características</h2>

                <div class="form-grid">
                    <label class="field">
                        <span>Habitaciones</span>
                        <input
                            v-model.number="form.bedrooms"
                            type="number"
                            min="1"
                            step="1"
                            required
                            :disabled="submitting"
                        />
                    </label>

                    <label class="field">
                        <span>Baños</span>
                        <input
                            v-model.number="form.bathrooms"
                            type="number"
                            min="1"
                            step="1"
                            required
                            :disabled="submitting"
                        />
                    </label>

                    <label class="field">
                        <span>Parqueaderos</span>
                        <input
                            v-model.number="form.parkingSpaces"
                            type="number"
                            min="0"
                            step="1"
                            required
                            :disabled="submitting"
                        />
                    </label>

                    <label class="field">
                        <span>Estrato</span>
                        <select v-model.number="form.stratum" :disabled="submitting">
                            <option v-for="number in 6" :key="number" :value="number">
                                {{ number }}
                            </option>
                        </select>
                    </label>

                    <label class="field">
                        <span>Área (m²)</span>
                        <input
                            v-model.number="form.area"
                            type="number"
                            min="20.01"
                            step="0.01"
                            required
                            :disabled="submitting"
                        />
                    </label>

                    <label class="field">
                        <span>Condición</span>
                        <select v-model.number="form.condition" :disabled="submitting">
                            <option
                                v-for="option in conditionOptions"
                                :key="option.value"
                                :value="option.value"
                            >
                                {{ option.label }}
                            </option>
                        </select>
                    </label>
                </div>

                <div class="field">
                    <span>Comodidades</span>
                    <div class="checkbox-grid">
                        <label
                            v-for="amenity in amenities"
                            :key="amenity.code"
                            class="checkbox"
                        >
                            <input
                                v-model="form.amenities"
                                type="checkbox"
                                :value="amenity.code"
                                :disabled="submitting"
                            />
                            {{ amenity.label }}
                        </label>
                    </div>
                </div>
            </section>

            <section class="card form-section">
                <h2>Dirección</h2>

                <div class="form-grid">
                    <label class="field">
                        <span>Tipo de vía principal</span>
                        <select
                            v-model.number="form.address.mainRoadType"
                            :disabled="submitting"
                        >
                            <option
                                v-for="option in roadTypeOptions"
                                :key="option.value"
                                :value="option.value"
                            >
                                {{ option.label }}
                            </option>
                        </select>
                    </label>

                    <label class="field">
                        <span>Número vía principal</span>
                        <input
                            v-model.trim="form.address.mainRoadNumber"
                            type="text"
                            maxlength="16"
                            required
                            :disabled="submitting"
                        />
                    </label>

                    <label class="field">
                        <span>Letra vía principal</span>
                        <input
                            v-model.trim="form.address.mainRoadLetter"
                            type="text"
                            maxlength="4"
                            :disabled="submitting"
                        />
                    </label>

                    <label class="field">
                        <span>Sufijo vía principal</span>
                        <select
                            v-model="form.address.mainRoadSuffix"
                            :disabled="submitting"
                        >
                            <option value="">Sin sufijo</option>
                            <option
                                v-for="option in roadSuffixOptions"
                                :key="option.value"
                                :value="option.value"
                            >
                                {{ option.label }}
                            </option>
                        </select>
                    </label>

                    <label class="field">
                        <span>Número vía transversal</span>
                        <input
                            v-model.trim="form.address.crossRoadNumber"
                            type="text"
                            maxlength="16"
                            required
                            :disabled="submitting"
                        />
                    </label>

                    <label class="field">
                        <span>Letra vía transversal</span>
                        <input
                            v-model.trim="form.address.crossRoadLetter"
                            type="text"
                            maxlength="4"
                            :disabled="submitting"
                        />
                    </label>

                    <label class="field">
                        <span>Sufijo vía transversal</span>
                        <select
                            v-model="form.address.crossRoadSuffix"
                            :disabled="submitting"
                        >
                            <option value="">Sin sufijo</option>
                            <option
                                v-for="option in roadSuffixOptions"
                                :key="option.value"
                                :value="option.value"
                            >
                                {{ option.label }}
                            </option>
                        </select>
                    </label>

                    <label class="field">
                        <span>Placa</span>
                        <input
                            v-model.trim="form.address.plate"
                            type="text"
                            maxlength="16"
                            :disabled="submitting"
                        />
                    </label>

                    <label class="field">
                        <span>Código postal</span>
                        <input
                            v-model.trim="form.address.postalCode"
                            type="text"
                            maxlength="8"
                            :disabled="submitting"
                        />
                    </label>

                    <label class="field">
                        <span>Latitud (opcional)</span>
                        <input
                            v-model.number="form.address.latitude"
                            type="number"
                            min="-90"
                            max="90"
                            step="any"
                            :disabled="submitting"
                        />
                    </label>

                    <label class="field">
                        <span>Longitud (opcional)</span>
                        <input
                            v-model.number="form.address.longitude"
                            type="number"
                            min="-180"
                            max="180"
                            step="any"
                            :disabled="submitting"
                        />
                    </label>
                </div>

                <label class="field">
                    <span>Indicaciones</span>
                    <textarea
                        v-model.trim="form.address.indications"
                        rows="3"
                        :disabled="submitting"
                    ></textarea>
                </label>
            </section>

            <p v-if="error" class="alert alert--error">{{ error }}</p>

            <div class="actions">
                <button
                    class="btn btn--primary"
                    type="submit"
                    :disabled="submitting || !canCreate"
                >
                    {{ submitting ? 'Creando...' : 'Crear propiedad' }}
                </button>

                <RouterLink class="btn" :to="{ name: 'properties' }">Cancelar</RouterLink>
            </div>
        </form>
    </section>
</template>

<script>
import { useAuthStore } from '@/modules/auth/stores/authStore';
import { locationsService } from '@/modules/locations/services/locationsService';
import { propertyTypesService } from '@/modules/propertyTypes/services/propertyTypesService';
import {
    AMENITIES,
    CURRENCIES,
    PROPERTY_CONDITIONS,
    ROAD_SUFFIXES,
    ROAD_TYPES,
} from '../constants/propertyEnums';
import { propertiesService } from '../services/propertiesService';
import { isGuid } from '../utils/validation';

export default {
    name: 'PropertyCreateView',

    data() {

        return {
            form: {
                ownerId: '',
                title: '',
                description: '',
                priceAmount: '',
                currencyCode: 'COP',
                propertyTypeId: '',
                neighborhoodId: '',
                bedrooms: 1,
                parkingSpaces: 0,
                bathrooms: 1,
                stratum: 3,
                area: '',
                condition: 1,
                amenities: [],
                address: {
                    mainRoadType: 1,
                    mainRoadNumber: '',
                    mainRoadLetter: '',
                    mainRoadSuffix: '',
                    crossRoadNumber: '',
                    crossRoadLetter: '',
                    crossRoadSuffix: '',
                    plate: '',
                    indications: '',
                    postalCode: '',
                    latitude: '',
                    longitude: '',
                },
            },
            submitting: false,
            error: '',
            propertyTypes: [],
            neighborhoods: [],
            catalogsLoading: false,
            catalogsError: '',
        };
    },

    computed: {

        auth() {

            return useAuthStore();
        },

        canCreate() {

            return this.auth.canManageProperties;
        },

        ownerEmail() {

            return this.auth.user?.email || '—';
        },

        currencies() {

            return CURRENCIES;
        },

        amenities() {

            return AMENITIES;
        },

        roadTypeOptions() {

            return Object.entries(ROAD_TYPES).map(([value, label]) => ({
                value: Number(value),
                label,
            }));
        },

        roadSuffixOptions() {

            return Object.entries(ROAD_SUFFIXES).map(([value, label]) => ({
                value: Number(value),
                label,
            }));
        },

        conditionOptions() {

            return Object.entries(PROPERTY_CONDITIONS).map(([value, label]) => ({
                value: Number(value),
                label,
            }));
        },
    },

    created() {

        this.form.ownerId = this.auth.user?.id || '';
        this.loadCatalogs();
    },

    methods: {

        async loadCatalogs() {

            this.catalogsLoading = true;
            this.catalogsError = '';

            try {

                const [typesPage, neighborhoodsPage] = await Promise.all([
                    propertyTypesService.list({ pageSize: 50 }),
                    locationsService.neighborhoods.list({ pageSize: 50 }),
                ]);

                this.propertyTypes = typesPage.items;
                this.neighborhoods = neighborhoodsPage.items;
            } catch (error) {

                this.catalogsError = `No se pudieron cargar los catálogos: ${error.message}`;
            } finally {

                this.catalogsLoading = false;
            }
        },

        validate() {

            const address = this.form.address;
            const title = this.form.title.trim();
            const description = this.form.description.trim();
            const priceAmount = Number(this.form.priceAmount);
            const bedrooms = Number(this.form.bedrooms);
            const bathrooms = Number(this.form.bathrooms);
            const parkingSpaces = Number(this.form.parkingSpaces);
            const stratum = Number(this.form.stratum);
            const area = Number(this.form.area);
            const condition = Number(this.form.condition);

            if (!this.canCreate) {

                this.error = 'No tienes permisos para crear propiedades.';
                return false;
            }

            if (!isGuid(this.form.ownerId)) {

                this.error = 'El ID del propietario no es un GUID válido.';
                return false;
            }

            if (title.length < 4) {

                this.error = 'El título debe tener entre 4 y 256 caracteres.';
                return false;
            }

            if (description.length < 32) {

                this.error = 'La descripción debe tener al menos 32 caracteres.';
                return false;
            }

            if (!Number.isFinite(priceAmount) || priceAmount <= 0) {

                this.error = 'El precio debe ser mayor a 0.';
                return false;
            }

            if (!CURRENCIES.includes(this.form.currencyCode)) {

                this.error = 'La moneda seleccionada no es válida.';
                return false;
            }

            if (!isGuid(this.form.propertyTypeId)) {

                this.error = 'El ID del tipo de inmueble no es un GUID válido.';
                return false;
            }

            if (!isGuid(this.form.neighborhoodId)) {

                this.error = 'El ID del barrio no es un GUID válido.';
                return false;
            }

            if (!address.mainRoadNumber.trim()) {

                this.error = 'El número de la vía principal es requerido.';
                return false;
            }

            if (!address.crossRoadNumber.trim()) {

                this.error = 'El número de la vía transversal es requerido.';
                return false;
            }

            if (!Number.isInteger(bedrooms) || bedrooms < 1) {

                this.error = 'Debe registrar al menos una habitación.';
                return false;
            }

            if (!Number.isInteger(bathrooms) || bathrooms < 1) {

                this.error = 'Debe registrar al menos un baño.';
                return false;
            }

            if (!Number.isInteger(parkingSpaces) || parkingSpaces < 0) {

                this.error = 'Los parqueaderos no pueden ser negativos.';
                return false;
            }

            if (!Number.isInteger(stratum) || stratum < 1 || stratum > 6) {

                this.error = 'El estrato debe estar entre 1 y 6.';
                return false;
            }

            if (!Number.isFinite(area) || area <= 20) {

                this.error = 'El área debe ser mayor a 20 m².';
                return false;
            }

            if (condition !== 1 && condition !== 2) {

                this.error = 'La condición de la propiedad no es válida.';
                return false;
            }

            const hasLatitude = address.latitude !== '' && address.latitude !== null;
            const hasLongitude = address.longitude !== '' && address.longitude !== null;

            if (hasLatitude !== hasLongitude) {

                this.error = 'La latitud y la longitud deben enviarse juntas.';
                return false;
            }

            if (hasLatitude) {

                const latitude = Number(address.latitude);
                const longitude = Number(address.longitude);

                if (!Number.isFinite(latitude) || latitude < -90 || latitude > 90) {

                    this.error = 'La latitud debe estar entre -90 y 90.';
                    return false;
                }

                if (!Number.isFinite(longitude) || longitude < -180 || longitude > 180) {

                    this.error = 'La longitud debe estar entre -180 y 180.';
                    return false;
                }
            }

            return true;
        },

        buildPayload() {

            const address = this.form.address;
            const hasCoordinates = address.latitude !== '' && address.latitude !== null;

            return {
                ownerId: this.form.ownerId.trim(),
                title: this.form.title.trim(),
                description: this.form.description.trim(),
                priceAmount: Number(this.form.priceAmount),
                currencyCode: this.form.currencyCode,
                propertyTypeId: this.form.propertyTypeId.trim(),
                neighborhoodId: this.form.neighborhoodId.trim(),
                address: {
                    mainRoadType: Number(address.mainRoadType),
                    mainRoadNumber: address.mainRoadNumber.trim(),
                    mainRoadLetter: address.mainRoadLetter.trim() || null,
                    mainRoadSuffix:
                        address.mainRoadSuffix === '' ? null : Number(address.mainRoadSuffix),
                    crossRoadNumber: address.crossRoadNumber.trim(),
                    crossRoadLetter: address.crossRoadLetter.trim() || null,
                    crossRoadSuffix:
                        address.crossRoadSuffix === '' ? null : Number(address.crossRoadSuffix),
                    plate: address.plate.trim() || null,
                    indications: address.indications.trim() || null,
                    postalCode: address.postalCode.trim() || null,
                    latitude: hasCoordinates ? Number(address.latitude) : null,
                    longitude: hasCoordinates ? Number(address.longitude) : null,
                },
                bedrooms: Number(this.form.bedrooms),
                parkingSpaces: Number(this.form.parkingSpaces),
                bathrooms: Number(this.form.bathrooms),
                stratum: Number(this.form.stratum),
                area: Number(this.form.area),
                condition: Number(this.form.condition),
                amenities: [...this.form.amenities],
            };
        },

        async submit() {

            this.error = '';

            if (!this.validate()) return;

            this.submitting = true;

            try {

                const id = await propertiesService.create(this.buildPayload());

                if (this.auth.canViewPropertyDetails) {

                    this.$router.push({
                        name: 'property-detail',
                        params: { id },
                        query: { notice: 'created' },
                    });
                } else {

                    this.$router.push({
                        name: 'properties',
                        query: { notice: 'created' },
                    });
                }
            } catch (error) {

                this.error = error.message;
            } finally {

                this.submitting = false;
            }
        },
    },
};
</script>
