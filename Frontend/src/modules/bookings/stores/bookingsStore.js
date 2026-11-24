import { defineStore } from 'pinia';

import { bookingsService } from '../services/bookingsService';

export const useBookingsStore = defineStore('bookings', {

    state: () => ({

        items: [],
        totalCount: 0,
        currentPage: 1,
        recordsPerPage: 12,
        totalPages: 0,
        hasPrevious: false,
        hasNext: false,
        loading: false,
        error: '',
    }),

    actions: {

        async fetchList(filters = {}) {

            this.loading = true;
            this.error = '';

            try {

                const data = await bookingsService.list({
                    page: this.currentPage,
                    recordsPerPage: this.recordsPerPage,
                    ...filters,
                });

                this.items = data.list;
                this.totalCount = data.totalCount;
                this.currentPage = data.currentPage;
                this.recordsPerPage = data.recordsPerPage;
                this.totalPages = data.totalPages;
                this.hasPrevious = data.hasPrevious;
                this.hasNext = data.hasNext;
            } catch (error) {

                this.error = error.message;
                this.items = [];
                this.totalCount = 0;
                this.totalPages = 0;
                this.hasPrevious = false;
                this.hasNext = false;
            } finally {

                this.loading = false;
            }
        },
    },
});
