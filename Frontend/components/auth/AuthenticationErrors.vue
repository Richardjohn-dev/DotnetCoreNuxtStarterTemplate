<template>
  <div v-if="showAlert">
    <v-alert
      v-if="errorStore.errors"
      type="error"
      variant="tonal"
      closable
      class="mb-3 pa-4"
      @click:close="errorStore.clearErrors()"
    >
      <div class="font-weight-bold mb-2">
        Please correct the following {{ errorStore.title }} errors:
      </div>

      <ul class="error-list">
        <template v-for="(errorArray, field) in errorStore.errors" :key="field">
          <li v-for="(error, index) in errorArray" :key="`${field}-${index}`">
            {{ error }}
          </li>
        </template>
      </ul>
    </v-alert>

    <v-alert
      v-if="errorStore.hasValidationErrors"
      density="compact"
      type="warning"
      variant="tonal"
      class="mb-3 pa-4"
      closable
    >
      <div class="font-weight-bold mb-2">{{ errorStore.title }}</div>
      <ul class="ps-4 mb-0">
        <template
          v-for="(errors, field) in errorStore.validationErrors"
          :key="field"
        >
          <li v-for="(error, i) in errors" :key="`${field}-${i}`">
            <strong>{{ formatFieldName(field) }}:</strong> {{ error }}
          </li>
        </template>
      </ul>
    </v-alert>
  </div>
</template>

<script setup lang="ts">
import { useErrorStore } from "~/store/errorStore";

const errorStore = useErrorStore();

const showAlert = computed(() => {
  if (errorStore.title === "Internal Server Error") {
    return false;
  }

  // Show for any error condition
  return (
    !!errorStore.title ||
    !!errorStore.detail ||
    (errorStore.errors && Object.keys(errorStore.errors).length > 0) ||
    (errorStore.validationErrors &&
      Object.keys(errorStore.validationErrors).length > 0)
  );
});

function formatFieldName(field: string): string {
  return (
    field
      // Insert space before uppercase letters
      .replace(/([A-Z])/g, " $1")
      // Capitalize first letter
      .replace(/^./, (str) => str.toUpperCase())
      // Remove leading space if present
      .trim()
  );
}
</script>

<style scoped>
.error-list {
  list-style-type: disc;
  padding-left: 20px;
  margin-top: 8px;
}
</style>
