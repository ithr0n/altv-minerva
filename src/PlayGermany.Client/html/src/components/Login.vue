<template>
    <v-container fill-height fluid>
        <v-row align="left" justify="center">
            <v-banner elevation="10">Play Germany - Anmeldung</v-banner>
        </v-row>

        <v-row align="center" justify="center">
            <v-form>
                <v-text-field
                    required
                    type="password"
                    v-model="password"
                    label="Passwort"
                    :disabled="inputDisabled"
                />

                <v-btn
                    @click="handleSubmit"
                    type="submit"
                    :disabled="inputDisabled"
                    :loading="inputDisabled"
                    >Anmelden</v-btn
                >
            </v-form>
        </v-row>

        <v-row align="center" justify="center">
            <v-alert dense type="error" v-if="errorMsg.length > 0">
                {{ errorMsg }}
            </v-alert>
        </v-row>
    </v-container>
</template>

<script lang="ts">
import Vue from 'vue'

export default Vue.extend({
    name: 'Login',

    data: () => {
        return {
            password: '',
            errorMsg: '',
            inputDisabled: false,
        }
    },

    mounted() {
        this.$alt.on('Login:Failed', () => {
            this.inputDisabled = false
            this.errorMsg =
                'Anmeldung fehlgeschlagen! Entweder hast du ein falsches Passwort verwendest oder dein Account ist gesperrt worden. Überprüfe deine Eingabe oder melde dich im Support.'
        })
    },

    methods: {
        handleSubmit() {
            if (this.password.length <= 3) {
                this.errorMsg =
                    'Du musst ein gültiges Passwort eingeben!'
                return
            }

            this.errorMsg = ''
            this.inputDisabled = true

            this.$alt.emit(
                'ui:EmitServer',
                'Login:Authenticate',
                this.password
            )
        },
    },
})
</script>

<style scoped></style>
