import { request } from '@/core/api/httpClient';

const BASE_PATH = '/properties/properties';

export const propertiesService = {
    /**
     * GET /properties/properties?pageNumber=&pageSize=&propertyTypeId=&cityId=&stratum=
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

    update(payload) {

        return request(BASE_PATH, { method: 'PUT', body: payload });
    },
};
