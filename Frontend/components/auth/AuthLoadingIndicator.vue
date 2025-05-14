<!-- components/auth/AuthLoadingIndicator.vue -->
<template>
  <div v-if="isVisible" class="auth-loading-indicator">
    <v-progress-circular
      indeterminate
      :color="color"
      :size="size"
      class="mr-2"
    ></v-progress-circular>
    <span v-if="showText" class="loading-text">
      <slot>
        <span v-if="currentOperation === 'login'">Logging in...</span>
        <span v-else-if="currentOperation === 'register'"
          >Creating account...</span
        >
        <span v-else-if="currentOperation === 'logout'">Logging out...</span>
        <span v-else-if="currentOperation === 'checkAuth'"
          >Verifying session...</span
        >
        <span v-else-if="currentOperation === 'syncExternalLogin'"
          >Syncing login...</span
        >
        <span v-else-if="currentOperation === 'initialCheck'"
          >Initializing...</span
        >
        <span v-else>Loading...</span>
      </slot>
    </span>
  </div>
</template>

<script setup lang="ts">
import { useAuth } from "~/composables/useAuth";
import { useAuthStore } from "~/store/authStore";

const props = defineProps({
  // Only show for specific operations
  operation: {
    type: String,
    default: null,
  },
  // Show the text label
  showText: {
    type: Boolean,
    default: false,
  },
  // Color of the spinner
  color: {
    type: String,
    default: "primary",
  },
  // Size of the spinner
  size: {
    type: [Number, String],
    default: 30,
  },
});

const authStore = useAuthStore();
const { isLoading, loadingOperation } = storeToRefs(authStore);

const currentOperation = computed(() => loadingOperation.value);

const isVisible = computed(() => {
  // Always hide during SSR or before hydration
  if (import.meta.server) return false;

  // Must be loading
  if (!isLoading.value) return false;

  // If specific operation requested, must match
  if (props.operation && loadingOperation.value !== props.operation)
    return false;

  return true;
});
</script>

<style scoped>
.auth-loading-indicator {
  display: inline-flex;
  align-items: center;
}

.loading-text {
  font-size: 14px;
  margin-left: 8px;
}
</style>
