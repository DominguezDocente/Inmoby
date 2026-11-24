import AuthLayout from '@/layouts/AuthLayout.vue';

export const authRoutes = [
    {
        path: '/login',
        component: AuthLayout,
        children: [
            {
                path: '',
                name: 'login',
                component: () => import('./views/LoginView.vue'),
                meta: { guestOnly: true },
            },
        ],
    },
];
