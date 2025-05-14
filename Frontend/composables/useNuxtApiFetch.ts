// composables/useNuxtApiFetch.ts
import { useFetch } from "#app";
import type { UseFetchOptions } from "nuxt/app";
import type { ApiResponse, ProblemDetails } from "~/types";
import { useErrorStore } from "~/store/errorStore";
import { useAuthStore } from "~/store/authStore";

/**
 * Event system for auth-related events - completely independent from the old implementation
 */
export const nuxtApiEvents = {
  onAuthError: null as ((error: any) => void) | null,

  emitAuthError(error: any) {
    if (this.onAuthError) {
      this.onAuthError(error);
    }
  },
};

/**
 * A composable that uses Nuxt's built-in useFetch with your API patterns
 */
export function useNuxtApiFetch<T>(
  url: string | (() => string),
  options: UseFetchOptions<ApiResponse<T>> = {}
) {
  const errorStore = useErrorStore();
  const resolvedUrl = typeof url === "function" ? url() : url;
  const isUserCheck = resolvedUrl === "/auth/session";

  // Clear errors before fetching
  errorStore.clearErrors();

  // Set up fetch options aligned with your current pattern
  const defaultOptions: UseFetchOptions<ApiResponse<T>> = {
    baseURL: useRuntimeConfig().public.apiBaseUrl,
    key: typeof url === "string" ? `api-${url}` : undefined,
    headers: {
      Accept: "application/json",
      "X-Requested-With": "XMLHttpRequest",
    },
    credentials: "include",

    // Transform the response to match your current API pattern
    transform: (response) => {
      return response;
    },

    // Error handling
    onResponseError({ response }) {
      const statusCode = response.status;
      const problemDetails = response._data as ProblemDetails;

      // Store error information
      if (problemDetails) {
        errorStore.setError(problemDetails);
      }

      // Handle 401 errors
      if (statusCode === 401) {
        nuxtApiEvents.emitAuthError({
          statusCode,
          isUserCheck,
          url: resolvedUrl,
        });
      }
    },
  };

  // Merge with user-provided options
  const fetchOptions = {
    ...defaultOptions,
    ...options,
    // Always include credentials
    credentials: "include" as const,
    // Merge headers properly
    headers: {
      ...defaultOptions.headers,
      ...(options.headers || {}),
    },
  };

  // Use Nuxt's built-in useFetch
  return useFetch<ApiResponse<T>>(url, fetchOptions);
}

/**
 * A convenience wrapper to provide a similar API to your current useApi()
 */
export function useNuxtApi() {
  return {
    // Regular methods
    get: <T>(url: string, options = {}) =>
      useNuxtApiFetch<T>(url, { method: "GET", ...options }),

    post: <T>(url: string, body?: any, options = {}) =>
      useNuxtApiFetch<T>(url, { method: "POST", body, ...options }),

    put: <T>(url: string, body?: any, options = {}) =>
      useNuxtApiFetch<T>(url, { method: "PUT", body, ...options }),

    delete: <T>(url: string, options = {}) =>
      useNuxtApiFetch<T>(url, { method: "DELETE", ...options }),

    patch: <T>(url: string, body?: any, options = {}) =>
      useNuxtApiFetch<T>(url, { method: "PATCH", body, ...options }),

    // Lazy loading versions
    lazyGet: <T>(url: string, options = {}) =>
      useNuxtApiFetch<T>(url, { method: "GET", lazy: true, ...options }),

    // Server-only versions (only fetch on server)
    serverGet: <T>(url: string, options = {}) =>
      useNuxtApiFetch<T>(url, { method: "GET", server: true, ...options }),

    // Client-only versions (only fetch on client)
    clientGet: <T>(url: string, options = {}) =>
      useNuxtApiFetch<T>(url, { method: "GET", server: false, ...options }),
  };
}
/**
 * Setup function to initialize auth error handling
 * This should be called in a plugin if you want to handle auth errors
 */
export function setupNuxtApiAuthHandling() {
  // This function can be called from a plugin to set up auth handlers
  nuxtApiEvents.onAuthError = (error) => {
    console.log("Nuxt API auth error:", error);

    // Handle auth errors, such as redirecting to login
    if (!error.isUserCheck) {
      // Get auth store and clear user
      const authStore = useAuthStore();
      authStore.setUser(null);

      // Redirect to login
      if (import.meta.client) {
        navigateTo("/login", { replace: true });
      }
    }
  };
}
