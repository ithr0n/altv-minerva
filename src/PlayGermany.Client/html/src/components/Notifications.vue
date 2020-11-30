<template></template>

<script lang="ts">
import Vue from 'vue'

export default Vue.extend({
    name: 'Notifications',

    mounted() {
        this.$alt.on('Notifications:Simple', (message: string) => {
            this.simpleNotification(message)
        })
    },

    methods: {
        simpleNotification(message: string) {
            // to do
        },
    },
})
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped></style>
