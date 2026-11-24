import { defineStore } from 'pinia';

import {
    getTokenExpiration,
    getTokenRoles,
    getUserFromToken,
    isTokenExpired,
} from '@/core/auth/token';

import { tokenStorage } from '@/core/auth/tokenStorage';
import { ROLES } from '@/core/constants/roles';
import { authService } from '../services/authService';

export const useAuthStore = defineStore('auth', {

    state: () => ({

        token: tokenStorage.get(),
    }),

    getters: {

        user: (state) => getUserFromToken(state.token),
        roles: (state) => getTokenRoles(state.token),
        expiresAt: (state) => getTokenExpiration(state.token),

        isAuthenticated: (state) => Boolean(state.token) && !isTokenExpired(state.token),

        isAdmin() {

            return this.roles.includes(ROLES.ADMIN);
        },

        canManageProperties() {

            return this.roles.includes(ROLES.ADMIN)
                || this.roles.includes(ROLES.PROPERTIES_MANAGER);
        },

        canViewPropertyDetails() {

            return this.roles.includes(ROLES.ADMIN) || this.roles.includes(ROLES.USER);
        },

        hasRole() {

            return (role) => this.roles.includes(role);
        },
    },

    actions: {

        async login(credentials) {

            const session = await authService.login(credentials);

            tokenStorage.set(session.accessToken);
            this.token = session.accessToken;

            return this.user;
        },

        logout() {

            tokenStorage.clear();
            this.token = null;
        },

        restore() {

            this.token = tokenStorage.get();
        },
    },
});
