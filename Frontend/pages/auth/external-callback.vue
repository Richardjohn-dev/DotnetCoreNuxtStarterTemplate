// pages/auth/external-callback.vue
<template>
  <v-container class="fill-height">
    <v-row align="center" justify="center">
      <v-col cols="12" sm="8" md="6">
        <v-card class="text-center pa-6">
          <v-card-title class="text-h5 mb-4">
            Processing authentication...
          </v-card-title>

          <v-card-text>
            <v-progress-circular
              indeterminate
              color="primary"
              size="64"
              class="mb-4"
            ></v-progress-circular>

            <p class="mt-4">
              Please wait while we complete your authentication process.
            </p>

            <p v-if="errorMessage" class="text-error mt-4">
              {{ errorMessage }}
            </p>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup lang="ts">
import { useErrorStore } from "~/store/errorStore";
import { useAuth } from "~/composables/useAuth";
import { redirectAfterLogin, redirectToLogin } from "~/utils/redirects";

definePageMeta({ public: true });

const route = useRoute();
const auth = useAuth(); // Use the composable for session management
const errorStore = useErrorStore();
const errorMessage = ref("");

const processCallback = async () => {
  try {
    const success = route.query.success === "true";
    const error = route.query.error as string | undefined;
    const returnUrl = (route.query.returnUrl as string) || "/";

    if (success) {
      console.log(
        "External callback success. Attempting session verification..."
      );

      // Wait for cookies to be properly set
      await new Promise((resolve) => setTimeout(resolve, 500));

      try {
        const result = await auth.syncSessionAfterExternalLogin();
        if (result.success) {
          console.log("Authentication successful, redirecting to:", returnUrl);
          setTimeout(() => {
            redirectAfterLogin(returnUrl);
          }, 300);
        } else {
          console.error("Session verification failed:", result.error);
          errorMessage.value =
            result.error || "Authentication verification failed";
          setTimeout(() => {
            redirectToLogin("/");
          }, 1000);
        }
      } catch (apiError) {
        console.error("API verification failed:", apiError);
        errorMessage.value = "Authentication verification failed";
        setTimeout(() => {
          redirectToLogin("/");
        }, 1000);
      }
    } else {
      // Rest of your logic for handling failure
      errorStore.setError({
        title: "Authentication Failed",
        detail: error,
      });

      errorMessage.value = error || "Authentication failed. Please try again.";
      setTimeout(() => {
        redirectToLogin("/");
      }, 1000);
    }
  } catch (err) {
    // Error handling
    errorStore.setError({
      title: "Authentication Error",
      detail: "An error occurred while processing your login.",
    });

    errorMessage.value = "An error occurred during authentication.";
    setTimeout(() => {
      redirectToLogin("/");
    }, 1000);
  }
};

onMounted(() => {
  if (import.meta.client) {
    // Start processing after a small delay to let the page render
    setTimeout(processCallback, 50);
  }
});
</script>

<style scoped>
.text-error {
  color: #f44336;
}
</style>
