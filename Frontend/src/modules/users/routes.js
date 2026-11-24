import { ROLES } from '@/core/constants/roles';

export const usersRoutes = [
    {
        path: 'usuarios',
        name: 'users',
        component: () => import('./views/UsersView.vue'),
        meta: { roles: [ROLES.ADMIN] },
    },
];
