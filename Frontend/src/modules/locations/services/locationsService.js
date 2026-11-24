import { request } from '@/core/api/httpClient';

const BASE_PATH = '/properties/locations';

function createLocationResource(resource) {

    const path = `${BASE_PATH}/${resource}`;

    return {
        list(params = {}) {

            return request(path, { query: params });
        },

        getById(id) {

            return request(`${path}/${id}`);
        },

        create(payload) {

            return request(path, { method: 'POST', body: payload });
        },

        update(id, payload) {

            return request(`${path}/${id}`, { method: 'PUT', body: payload });
        },

        remove(id) {

            return request(`${path}/${id}`, { method: 'DELETE' });
        },
    };
}

/**
 * Catálogos de ubicación del microservicio de Properties.
 * Rutas de master:
 *   /properties/locations/countries
 *   /properties/locations/states
 *   /properties/locations/cities
 *   /properties/locations/neighborhoods
 */
export const locationsService = {
    countries: createLocationResource('countries'),
    states: createLocationResource('states'),
    cities: createLocationResource('cities'),
    neighborhoods: createLocationResource('neighborhoods'),
};
