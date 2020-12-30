<template>
    <v-container fill-height fluid>
        <v-row align="left" justify="center">
            <v-banner elevation="10">Neuen Charakter erstellen</v-banner>
        </v-row>

        <v-row align="center" justify="center">
            <v-form>
                <v-text-field
                    required
                    type="text"
                    v-model="firstName"
                    label="Vorname"
                />

                <v-text-field
                    required
                    type="text"
                    v-model="lastName"
                    label="Nachname"
                />

                <v-text-field
                    required
                    type="text"
                    v-model="birthday"
                    label="Geburtstag"
                />

                <!-- radio buttons: gender -->

                <v-btn @click="handleSubmit" type="submit">Erstellen</v-btn>
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
    name: 'CharacterCreation',

    data: () => {
        return {
            firstName: '',
            lastName: '',
            birthday: new Date(),
            genderIsMale: true,
            errorMsg: '',
            inputDisabled: false,
        }
    },

    mounted() {},

    methods: {
        handleSubmit() {
            if (this.firstName.length <= 3) {
                this.errorMsg = 'Du musst einen gültigen Vornamen eingeben!'
                // validation: on input fields instead of block error message
                return
            }

            this.errorMsg = ''
            this.inputDisabled = true

            this.$alt.emit(
                'CharacterCreation:Submit',
                this.firstName,
                this.lastName,
                this.birthday
            )
        },
    },
})
</script>

<style scoped></style>
