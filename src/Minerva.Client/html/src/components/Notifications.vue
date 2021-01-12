<template></template>

<script lang="ts">
import Vue from 'vue'

export default Vue.extend({
    name: 'Notifications',

    mounted() {
        this.$alt.on('Notifications:Notify', (message: string) => {
            this.notify(message)
        })

        this.$alt.on('Notifications:Info', (message: string) => {
            this.info(message)
        })

        this.$alt.on('Notifications:Success', (message: string) => {
            this.success(message)
        })

        this.$alt.on('Notifications:Error', (message: string) => {
            this.error(message)
        })

        this.$alt.on('Notifications:Warning', (message: string) => {
            this.warning(message)
        })
    },

    methods: {
        notify(message: string) {
            this.$toast(message)
        },

        info(message: string) {
            this.$toast.info(message)
        },

        success(message: string) {
            this.$toast.success(message)
        },

        error(message: string) {
            this.$toast.error(message)
        },

        warning(message: string) {
            this.$toast.warning(message)
        },
    },
})
</script>

<style>
.Vue-Toastification__icon {
    font-size: 1.1vw;
    color: #000000 !important;
    text-align: center;
    margin: 0 auto;

    text-shadow: 0 0 0 rgba(0, 0, 0, 0);
    animation: notify-pulse 0.8s infinite alternate;
}

.Vue-Toastification__toast-body {
    overflow-wrap: break-word;
    max-width: 15vw;
    color: #1d1d1d !important;
}

.Vue-Toastification__toast--default {
    background-color: #e7f5ff !important;
}

.Vue-Toastification__toast--warning {
    background-color: #ffda09 !important;
}

@keyframes notify-pulse {
    to {
        text-shadow: 0 0 18px rgba(0, 0, 0, 0.7);
    }
}
</style>
