import { request } from '@/core/api/httpClient';

const BASE_PATH = '/identity/auth';

export const authService = {
    /**
     * POST /identity/auth/login
     * Devuelve { accessToken, tokenType, expiresAtUTC }.
     */
    login(credentials) {

        return request(`${BASE_PATH}/login`, {
            method: 'POST',
            body: credentials,
            withAuth: false,
            handleUnauthorized: false,
        });
    },
};
