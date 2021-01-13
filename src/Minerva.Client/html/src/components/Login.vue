<template>
    <v-container fill-height fluid>
        <v-row justify="center">
            <v-banner elevation="10" color="grey lighten-1"
                >alt:V Minerva - Login</v-banner
            >
        </v-row>

        <v-row align="center" justify="center">
            <v-form class="formStyle">
                <v-text-field
                    v-model="password"
                    required
                    type="password"
                    label="Passwort"
                    :disabled="inputDisabled"
                />

                <v-btn
                    :disabled="inputDisabled"
                    :loading="inputDisabled"
                    @click="handleSubmit"
                    >Anmelden</v-btn
                >
            </v-form>
        </v-row>

        <v-row align="center" justify="center">
            <v-alert
                v-if="errorMsg.length > 0"
                dense
                type="error"
                v-html="errorMsg"
            >
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
                'Anmeldung fehlgeschlagen!<br/>Entweder hast du ein falsches Passwort verwendet oder dein Account ist gesperrt worden.<br/>Überprüfe deine Eingabe oder melde dich im Support.'
        })
    },

    methods: {
        handleSubmit() {
            if (this.password.length <= 3) {
                this.errorMsg = 'Du musst ein gültiges Passwort eingeben!'
                return
            }

            this.errorMsg = ''
            this.inputDisabled = true

            this.$alt.emit('ui:EmitServer', 'Login:Authenticate', this.password)
        },
    },
})
</script>

<style scoped>
.formStyle {
    background: rgba(180, 180, 180, 1);
    border-radius: 5px;
    padding: 30px;
}
</style>
