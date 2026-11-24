export const BOOKING_STATUS = Object.freeze({
    1: 'Pendiente',
    2: 'Confirmada',
    3: 'Cancelada',
    4: 'Completada',
});

export const BOOKING_STATUS_BADGE = Object.freeze({
    1: 'badge--warning',
    2: 'badge--ok',
    3: 'badge--muted',
    4: 'badge--info',
});

export function bookingStatusLabel(status) {

    return BOOKING_STATUS[status] || 'Desconocido';
}

export function bookingStatusBadge(status) {

    return BOOKING_STATUS_BADGE[status] || 'badge--muted';
}

export function isPending(status) {

    return status === 1;
}

export function isConfirmed(status) {

    return status === 2;
}
