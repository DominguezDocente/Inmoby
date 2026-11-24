import { request } from '@/core/api/httpClient';

const BASE_PATH = '/bookings/bookings';

export const bookingsService = {
    /**
     * GET /bookings/bookings?page=&recordsPerPage=&from=&to=&filter=
     * El backend decide el alcance según el JWT: Admin ve todo, el resto solo lo propio.
     */
    list(params = {}) {

        return request(BASE_PATH, { query: params });
    },

    getById(id) {

        return request(`${BASE_PATH}/${id}`);
    },

    create(payload) {

        return request(BASE_PATH, { method: 'POST', body: payload });
    },

    update(id, payload) {

        return request(`${BASE_PATH}/${id}`, { method: 'PUT', body: payload });
    },

    confirm(id) {

        return request(`${BASE_PATH}/${id}/confirm`, { method: 'POST' });
    },

    cancel(id, payload = {}) {

        return request(`${BASE_PATH}/${id}/cancel`, { method: 'POST', body: payload });
    },

    complete(id) {

        return request(`${BASE_PATH}/${id}/complete`, { method: 'POST' });
    },
};
