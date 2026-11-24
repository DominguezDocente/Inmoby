import { createApp } from 'vue';
import { createPinia } from 'pinia';

import App from './App.vue';
import router from './router';
import { setUnauthorizedHandler } from '@/core/api/httpClient';
import { useAuthStore } from '@/modules/auth/stores/authStore';

import './assets/styles.css';

const app = createApp(App);
const pinia = createPinia();

app.use(pinia);
app.use(router);

// Centraliza el cierre de sesión cuando cualquier endpoint responde 401.
setUnauthorizedHandler(() => {

    const auth = useAuthStore(pinia);
    auth.logout();

    const currentRoute = router.currentRoute.value;

    if (currentRoute.name !== 'login') {

        router.push({
            name: 'login',
            query: {
                redirect: currentRoute.fullPath,
                expired: '1',
            },
        });
    }
});

app.mount('#app');
