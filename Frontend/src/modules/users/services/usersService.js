import { request } from '@/core/api/httpClient';

const BASE_PATH = '/identity/users';

export const usersService = {
    /**
     * GET /identity/users
     * Requiere rol Admin.
     */
    list() {

        return request(BASE_PATH);
    },
};
