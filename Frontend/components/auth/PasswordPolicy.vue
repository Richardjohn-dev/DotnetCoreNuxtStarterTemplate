<template>
  <v-card v-if="policy" variant="outlined" class="pa-2">
    <v-card-title class="text-subtitle-2 pb-1"
      >Password Requirements</v-card-title
    >
    <div class="d-flex flex-column gap-1 pa-2">
      <div class="d-flex align-center">
        <v-icon
          size="x-small"
          class="mr-2"
          :color="password.length >= policy.minLength ? 'success' : 'error'"
        >
          {{
            password.length >= policy.minLength
              ? "mdi-check-circle"
              : "mdi-alert-circle-outline"
          }}
        </v-icon>
        <span class="text-caption"
          >Minimum {{ policy.minLength }} characters</span
        >
      </div>

      <div v-if="policy.requiresUppercase" class="d-flex align-center">
        <v-icon
          size="x-small"
          class="mr-2"
          :color="hasUppercase ? 'success' : 'error'"
        >
          {{ hasUppercase ? "mdi-check-circle" : "mdi-alert-circle-outline" }}
        </v-icon>
        <span class="text-caption">At least one uppercase letter</span>
      </div>

      <div v-if="policy.requiresLowercase" class="d-flex align-center">
        <v-icon
          size="x-small"
          class="mr-2"
          :color="hasLowercase ? 'success' : 'error'"
        >
          {{ hasLowercase ? "mdi-check-circle" : "mdi-alert-circle-outline" }}
        </v-icon>
        <span class="text-caption">At least one lowercase letter</span>
      </div>

      <div v-if="policy.requiresDigit" class="d-flex align-center">
        <v-icon
          size="x-small"
          class="mr-2"
          :color="hasDigit ? 'success' : 'error'"
        >
          {{ hasDigit ? "mdi-check-circle" : "mdi-alert-circle-outline" }}
        </v-icon>
        <span class="text-caption">At least one number</span>
      </div>

      <div v-if="policy.requiresSpecialCharacter" class="d-flex align-center">
        <v-icon
          size="x-small"
          class="mr-2"
          :color="hasSpecialChar ? 'success' : 'error'"
        >
          {{ hasSpecialChar ? "mdi-check-circle" : "mdi-alert-circle-outline" }}
        </v-icon>
        <span class="text-caption">At least one special character</span>
      </div>
    </div>
  </v-card>
</template>
<script setup lang="ts">
import type { PasswordPolicy } from "~/types";

// Props
const props = defineProps<{
  password: string;
  policy: PasswordPolicy | null;
}>();

// Password validation helpers
const hasUppercase = computed(() => /[A-Z]/.test(props.password));
const hasLowercase = computed(() => /[a-z]/.test(props.password));
const hasDigit = computed(() => /\d/.test(props.password));
const hasSpecialChar = computed(() =>
  /[!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?]/.test(props.password)
);
</script>
