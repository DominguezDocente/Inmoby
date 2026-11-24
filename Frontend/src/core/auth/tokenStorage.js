const TOKEN_KEY = 'inmoby.auth.token';

export const tokenStorage = {
    get() {

        try {

            return window.localStorage.getItem(TOKEN_KEY);
        } catch {

            return null;
        }
    },

    set(token) {

        try {

            window.localStorage.setItem(TOKEN_KEY, token);
        } catch {

            // Almacenamiento no disponible (modo privado, permisos, etc.).
        }
    },

    clear() {

        try {

            window.localStorage.removeItem(TOKEN_KEY);
        } catch {

            // Almacenamiento no disponible.
        }
    },
};
