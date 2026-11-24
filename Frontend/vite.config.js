import { fileURLToPath, URL } from 'node:url';
import { defineConfig, loadEnv } from 'vite';
import vue from '@vitejs/plugin-vue';

// Las rutas del SPA (por ejemplo /propiedades) se sirven como index.html.
// Las llamadas axios al gateway no envían "text/html", así que sí pasan por el proxy.
const spaFallback = (req) => {

    const acceptsHtml = req.headers.accept?.includes('text/html');

    return acceptsHtml ? '/index.html' : undefined;
};

export default defineConfig(({ mode }) => {

    const env = loadEnv(mode, process.cwd(), '');
    const gatewayUrl = env.VITE_GATEWAY_URL || 'https://localhost:7777';

    return {
        plugins: [vue()],
        resolve: {
            alias: {
                '@': fileURLToPath(new URL('./src', import.meta.url)),
            },
        },
        server: {
            port: 5173,
            proxy: {
                '/identity': {
                    target: gatewayUrl,
                    changeOrigin: true,
                    secure: false,
                    bypass: spaFallback,
                },
                '/properties': {
                    target: gatewayUrl,
                    changeOrigin: true,
                    secure: false,
                    bypass: spaFallback,
                },
                '/bookings': {
                    target: gatewayUrl,
                    changeOrigin: true,
                    secure: false,
                    bypass: spaFallback,
                },
            },
        },
    };
});
