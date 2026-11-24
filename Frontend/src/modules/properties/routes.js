import { ROLES } from '@/core/constants/roles';

export const propertiesRoutes = [
    {
        path: 'propiedades',
        name: 'properties',
        component: () => import('./views/PropertiesView.vue'),
    },
    {
        path: 'propiedades/nueva',
        name: 'property-create',
        component: () => import('./views/PropertyCreateView.vue'),
        meta: {
            roles: [ROLES.ADMIN, ROLES.PROPERTIES_MANAGER],
        },
    },
    {
        path: 'propiedades/:id',
        name: 'property-detail',
        component: () => import('./views/PropertyDetailView.vue'),
        props: true,
        meta: {
            roles: [ROLES.ADMIN, ROLES.USER],
        },
    },
];
