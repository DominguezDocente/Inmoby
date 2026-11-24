import { request } from '@/core/api/httpClient';

const BASE_PATH = '/properties/propertytypes';

export const propertyTypesService = {
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

    remove(id) {

        return request(`${BASE_PATH}/${id}`, { method: 'DELETE' });
    },
};
