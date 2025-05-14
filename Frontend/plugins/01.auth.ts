// plugins/01.auth.ts
import { defineNuxtPlugin } from "#app";
import { useAuthStore } from "~/store/authStore";

export default defineNuxtPlugin(async (nuxtApp) => {
  const authStore = useAuthStore();

  // Setup event handlers for auth events
  authStore.setupAuthEvents();

  // Only run on client-side to avoid SSR issues
  if (import.meta.client) {
    try {
      authStore.resetLoadingState();

      if (!authStore.isInitialized || authStore.needsFreshAuthCheck) {
        await authStore.fetchUser();
      }
    } catch (error) {
      console.error("Failed to initialize auth store:", error);
      authStore.setUser(null);
    }
  } else {
    // Server-side logic - always assume not authenticated during SSR
    // This prevents hydration mismatches
    if (authStore.user) {
      console.log("Resetting user during SSR to prevent hydration issues");
      authStore.setUser(null);
    }
  }

  // Add a hook for route changes to ensure auth state is not stuck
  nuxtApp.hook("page:finish", () => {
    // Reset any stuck loading state after navigation
    if (import.meta.client && authStore.isLoading) {
      setTimeout(() => {
        if (authStore.isLoading) {
          authStore.resetLoadingState();
        }
      }, 500); // Give a small delay to allow any in-progress operations to complete
    }
  });
});
