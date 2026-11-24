import { defineStore } from 'pinia';

import { propertiesService } from '../services/propertiesService';

export const usePropertiesStore = defineStore('properties', {

    state: () => ({

        items: [],
        totalCount: 0,
        pageNumber: 1,
        pageSize: 12,
        totalPages: 0,
        hasPreviousPage: false,
        hasNextPage: false,
        loading: false,
        error: '',
    }),

    actions: {

        async fetchList(filters = {}) {

            this.loading = true;
            this.error = '';

            try {

                const data = await propertiesService.list({
                    pageNumber: this.pageNumber,
                    pageSize: this.pageSize,
                    ...filters,
                });

                this.items = data.items;
                this.totalCount = data.totalCount;
                this.pageNumber = data.pageNumber;
                this.pageSize = data.pageSize;
                this.totalPages = data.totalPages;
                this.hasPreviousPage = data.hasPreviousPage;
                this.hasNextPage = data.hasNextPage;
            } catch (error) {

                this.error = error.message;
                this.items = [];
                this.totalCount = 0;
                this.totalPages = 0;
                this.hasPreviousPage = false;
                this.hasNextPage = false;
            } finally {

                this.loading = false;
            }
        },
    },
});
