<template>
    <div class="app-shell">
        <AppNavbar />

        <main class="app-main">
            <RouterView />
        </main>

        <footer class="app-footer">Inmoby</footer>
    </div>
</template>

<script>
import AppNavbar from '@/components/AppNavbar.vue';

export default {
    name: 'DefaultLayout',
    components: { AppNavbar },
};
</script>
