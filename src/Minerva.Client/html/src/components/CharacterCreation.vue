<template>
    <v-container fill-height fluid>
        <v-row justify="center">
            <v-banner elevation="10" color="grey lighten-1"
                >Neuen Charakter erstellen</v-banner
            >
        </v-row>

        <v-row align="center" justify="center">
            <v-form class="formStyle">
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

                <v-menu
                    ref="birthdayMenu"
                    v-model="birthdayMenu"
                    :close-on-content-click="false"
                    transition="scale-transition"
                    offset-y
                    min-width="290px"
                >
                    <template v-slot:activator="{ on, attrs }">
                        <v-text-field
                            v-model="birthday"
                            label="Geburtstag"
                            prepend-icon="mdi-calendar"
                            readonly
                            v-bind="attrs"
                            v-on="on"
                        ></v-text-field>
                    </template>
                    <v-date-picker
                        ref="birthdayPicker"
                        v-model="birthday"
                        :max="maxBirthdayPicker"
                        min="1950-01-01"
                        @change="save"
                    ></v-date-picker>
                </v-menu>

                <!-- radio buttons: gender -->

                <v-btn @click="handleSubmit">Erstellen</v-btn>
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
            birthday: '',
            genderIsMale: true,
            errorMsg: '',
            inputDisabled: false,
            birthdayMenu: false,
        }
    },

    mounted() {},

    watch: {
        birthdayMenu(val) {
            if (val) {
                setTimeout(() => {
                    const birthdayPicker = this.$refs.birthdayPicker as any
                    birthdayPicker.activePicker = 'YEAR'
                })
            }
        },
    },

    computed: {
        maxBirthdayPicker() {
            const date = new Date()
            date.setFullYear(date.getFullYear() - 18)

            return date.toISOString().substr(0, 10)
        },
    },

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

        save(date: string) {
            const menu = this.$refs.birthdayMenu as any
            menu.save(date)
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
