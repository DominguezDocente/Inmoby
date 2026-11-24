<template>
    <div class="auth-shell">
        <div class="auth-shell__content">
            <RouterLink class="auth-brand" :to="{ name: 'login' }">Inmoby</RouterLink>

            <div class="auth-card">
                <RouterView />
            </div>

            <p class="auth-footer">Gestión inmobiliaria</p>
        </div>
    </div>
</template>

<script>
export default {
    name: 'AuthLayout',
};
</script>
