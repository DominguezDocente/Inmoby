export const bookingsRoutes = [
    {
        path: 'reservas',
        name: 'bookings',
        component: () => import('./views/BookingsView.vue'),
    },
    {
        path: 'reservas/nueva',
        name: 'booking-create',
        component: () => import('./views/BookingFormView.vue'),
    },
    {
        path: 'reservas/:id/editar',
        name: 'booking-edit',
        component: () => import('./views/BookingFormView.vue'),
        props: true,
    },
];
