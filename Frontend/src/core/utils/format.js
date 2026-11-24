export function formatCurrency(amount, currencyCode = 'COP') {

    if (amount === null || amount === undefined) return '—';

    try {

        return new Intl.NumberFormat('es-CO', {
            style: 'currency',
            currency: currencyCode || 'COP',
            maximumFractionDigits: 0,
        }).format(amount);
    } catch {

        return `${amount} ${currencyCode || ''}`.trim();
    }
}

export function formatDate(value) {

    if (!value) return '—';

    const date = new Date(value);

    if (Number.isNaN(date.getTime())) return '—';

    return new Intl.DateTimeFormat('es-CO', {
        dateStyle: 'medium',
        timeStyle: 'short',
    }).format(date);
}

export function formatArea(value) {

    if (value === null || value === undefined) return '—';

    return `${value} m²`;
}
