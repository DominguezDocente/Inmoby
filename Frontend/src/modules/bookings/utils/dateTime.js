export function toDateTimeLocal(value) {

    if (!value) return '';

    const date = new Date(value);

    if (Number.isNaN(date.getTime())) return '';

    const pad = (number) => String(number).padStart(2, '0');

    const datePart = `${date.getFullYear()}-${pad(date.getMonth() + 1)}-${pad(date.getDate())}`;
    const timePart = `${pad(date.getHours())}:${pad(date.getMinutes())}`;

    return `${datePart}T${timePart}`;
}

export function minimumBookingDateTime(minutes = 5) {

    return toDateTimeLocal(new Date(Date.now() + minutes * 60 * 1000));
}
