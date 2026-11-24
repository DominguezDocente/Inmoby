import { createRouter, createWebHistory } from 'vue-router';

import DefaultLayout from '@/layouts/DefaultLayout.vue';
import { authRoutes } from '@/modules/auth/routes';
import { bookingsRoutes } from '@/modules/bookings/routes';
import { propertiesRoutes } from '@/modules/properties/routes';
import { usersRoutes } from '@/modules/users/routes';
import { useAuthStore } from '@/modules/auth/stores/authStore';

const dashboardRoute = {
    path: '',
    name: 'dashboard',
    component: () => import('@/views/DashboardView.vue'),
};

const forbiddenRoute = {
    path: 'forbidden',
    name: 'forbidden',
    component: () => import('@/views/ForbiddenView.vue'),
};

const notFoundRoute = {
    path: '/:pathMatch(.*)*',
    name: 'not-found',
    component: () => import('@/views/NotFoundView.vue'),
};

const routes = [
    ...authRoutes,
    {
        path: '/',
        component: DefaultLayout,
        meta: { requiresAuth: true },
        children: [
            dashboardRoute,
            ...propertiesRoutes,
            ...bookingsRoutes,
            ...usersRoutes,
            forbiddenRoute,
        ],
    },
    notFoundRoute,
];

const router = createRouter({
    history: createWebHistory(import.meta.env.BASE_URL),
    routes,
    scrollBehavior: () => ({ top: 0 }),
});

router.beforeEach((to) => {

    const auth = useAuthStore();

    if (to.meta.requiresAuth) {

        if (!auth.isAuthenticated) {

            auth.logout();

            return {
                name: 'login',
                query: {
                    redirect: to.fullPath,
                },
            };
        }

        const requiredRoles = to.meta.roles;

        if (requiredRoles?.length && !requiredRoles.some((role) => auth.hasRole(role))) {

            return { name: 'forbidden' };
        }
    }

    if (to.meta.guestOnly && auth.isAuthenticated) {

        return { name: 'dashboard' };
    }

    return true;
});

export default router;
