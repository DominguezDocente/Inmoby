import axios from 'axios';

import { API_BASE_URL } from '@/core/config';
import { tokenStorage } from '@/core/auth/tokenStorage';

export class ApiError extends Error {
    constructor(message, { status = 0, errors = [] } = {}) {

        super(message);
        this.name = 'ApiError';
        this.status = status;
        this.errors = errors;
    }
}

let unauthorizedHandler = null;

/**
 * La app registra aquí el cierre de sesión global.
 * Se evita importar el router o Pinia desde este módulo para no crear ciclos.
 */
export function setUnauthorizedHandler(handler) {

    unauthorizedHandler = handler;
}

const api = axios.create({
    baseURL: API_BASE_URL,
    timeout: 20000,
});

// `withAuth` no debe llamarse `auth`: Axios reserva `config.auth` para Basic Auth
// y sobrescribiría el header Authorization con un valor Basic.
api.interceptors.request.use((config) => {

    const token = tokenStorage.get();

    if (token && config.withAuth !== false) {

        config.headers.Authorization = `Bearer ${token}`;
    }

    return config;
});

api.interceptors.response.use(
    (response) => response,
    (error) => {

        if (error.response?.status === 401 && error.config?.handleUnauthorized !== false) {

            unauthorizedHandler?.();
        }

        return Promise.reject(error);
    }
);

function sanitizeParams(query) {

    if (!query) return undefined;

    const params = {};

    Object.entries(query).forEach(([key, value]) => {

        if (value === undefined || value === null || value === '') return;

        params[key] = value;
    });

    return params;
}

function normalizeErrors(payload) {

    const errors = payload?.errors;

    if (!errors) return [];

    if (Array.isArray(errors)) {

        return errors.map((error) => {

            if (typeof error === 'string') return error;

            return error?.description ?? error?.message ?? String(error);
        });
    }

    if (typeof errors === 'object') {

        return Object.values(errors).flat().map(String);
    }

    return [String(errors)];
}

function defaultMessageFor(status) {

    switch (status) {

        case 400:
            return 'La solicitud no es válida.';
        case 403:
            return 'No tienes permisos para realizar esta acción.';
        case 404:
            return 'El recurso solicitado no existe.';
        case 500:
            return 'El servidor devolvió un error inesperado.';
        default:
            return `Error ${status}`;
    }
}

function unwrapPayload(payload, status) {

    if (payload && typeof payload === 'object' && 'isSuccess' in payload) {

        if (!payload.isSuccess) {

            const errors = normalizeErrors(payload);

            throw new ApiError(payload.message || errors[0] || 'La operación no se pudo completar.', {
                status,
                errors,
            });
        }

        // Identity, Properties y Booking responden con `data`.
        return payload.data;
    }

    return payload;
}

/**
 * Petición HTTP al API Gateway mediante Axios.
 *
 * - El interceptor de request agrega `Authorization: Bearer <token>`.
 * - El interceptor de response dispara el cierre de sesión ante un 401.
 * - Desenvuelve el sobre `{ isSuccess, data | result, message, errors }`.
 * - Lanza ApiError con status y errores normalizados cuando algo falla.
 */
export async function request(
    path,
    { method = 'GET', body, query, withAuth = true, handleUnauthorized = true } = {}
) {

    try {

        const response = await api.request({
            url: path,
            method,
            data: body,
            params: sanitizeParams(query),
            withAuth,
            handleUnauthorized,
        });

        if (response.status === 204) return null;

        return unwrapPayload(response.data, response.status);
    } catch (error) {

        if (error instanceof ApiError) throw error;

        const status = error.response?.status ?? 0;
        const payload = error.response?.data;

        if (status === 0) {

            throw new ApiError(
                'No se pudo conectar con el servidor. Verifica que el API Gateway esté en ejecución.'
            );
        }

        if (status === 401) {

            throw new ApiError('Tu sesión expiró o no estás autenticado.', { status });
        }

        const errors = normalizeErrors(payload);
        const message =
            payload?.message || payload?.title || errors[0] || defaultMessageFor(status);

        throw new ApiError(message, { status, errors });
    }
}
