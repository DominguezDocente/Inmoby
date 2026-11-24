function base64UrlDecode(value) {

    const base64 = value.replace(/-/g, '+').replace(/_/g, '/');
    const padded = base64.padEnd(base64.length + ((4 - (base64.length % 4)) % 4), '=');
    const binary = window.atob(padded);
    const bytes = Uint8Array.from(binary, (char) => char.charCodeAt(0));

    return new TextDecoder('utf-8').decode(bytes);
}

/**
 * Decodifica el payload de un JWT sin validar la firma.
 * La validación real siempre la hacen los microservicios.
 */
export function decodeToken(token) {

    if (!token) return null;

    const parts = token.split('.');

    if (parts.length < 2) return null;

    try {

        return JSON.parse(base64UrlDecode(parts[1]));
    } catch {

        return null;
    }
}

const ROLE_CLAIM_KEYS = [
    'role',
    'roles',
    'http://schemas.microsoft.com/ws/2008/06/identity/claims/role',
];

export function getTokenRoles(token) {

    const payload = decodeToken(token);

    if (!payload) return [];

    const roles = ROLE_CLAIM_KEYS.flatMap((key) => {

        if (!payload[key]) return [];

        return Array.isArray(payload[key]) ? payload[key] : [payload[key]];
    });

    return [...new Set(roles)];
}

export function getTokenExpiration(token) {

    const payload = decodeToken(token);

    if (!payload?.exp) return null;

    return new Date(payload.exp * 1000);
}

export function isTokenExpired(token, clockSkewSeconds = 30) {

    const payload = decodeToken(token);

    if (!payload?.exp) return true;

    return Date.now() >= (payload.exp - clockSkewSeconds) * 1000;
}

export function getUserFromToken(token) {

    const payload = decodeToken(token);

    if (!payload) return null;

    return {
        id:
            payload.sub ||
            payload.nameid ||
            payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'] ||
            null,
        email:
            payload.name ||
            payload.unique_name ||
            payload.email ||
            payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'] ||
            '',
        firstName: payload.fisrtName || payload.firstName || '',
        lastName: payload.lastName || '',
        document: payload.document || '',
        roles: getTokenRoles(token),
        expiresAt: getTokenExpiration(token),
    };
}
