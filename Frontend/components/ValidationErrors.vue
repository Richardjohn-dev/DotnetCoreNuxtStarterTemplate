<!-- components/ValidationErrors.vue -->
<template>
  <v-alert
    v-if="errorStore.hasValidationErrors"
    density="compact"
    type="warning"
    variant="tonal"
    class="mb-3"
    closable
  >
    <div v-if="title" class="font-weight-bold mb-1">{{ title }}</div>
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
</template>

<script setup lang="ts">
import { useErrorStore } from "~/store/errorStore";

// Define component props
const props = defineProps({
  // Title for validation errors section
  title: {
    type: String,
    default: "Please correct the following errors:",
  },
});

const errorStore = useErrorStore();

// Format field names for display (e.g., "emailAddress" -> "Email Address")
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
