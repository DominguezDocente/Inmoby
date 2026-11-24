<template>
    <div v-if="totalPages > 0" class="pagination">
        <button
            class="btn btn--sm"
            type="button"
            :disabled="!hasPrevious"
            @click="$emit('change', page - 1)"
        >
            Anterior
        </button>

        <span class="pagination__label">Página {{ page }} de {{ totalPages }}</span>

        <button
            class="btn btn--sm"
            type="button"
            :disabled="!hasNext"
            @click="$emit('change', page + 1)"
        >
            Siguiente
        </button>
    </div>
</template>

<script>
export default {
    name: 'AppPagination',

    props: {

        page: {
            type: Number,
            required: true,
        },
        totalPages: {
            type: Number,
            default: 0,
        },
        hasPrevious: {
            type: Boolean,
            default: false,
        },
        hasNext: {
            type: Boolean,
            default: false,
        },
    },

    emits: ['change'],
};
</script>
