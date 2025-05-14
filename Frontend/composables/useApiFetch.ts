// composables/useApiFetch.ts
import { baseFetch } from "~/utils/fetchUtils";
import type { FetchOptions } from "ofetch";
import type { ProblemDetails, ApiResponse } from "~/types";
import { useErrorStore } from "~/store/errorStore";

export class ApiError extends Error {
  public problemDetails: ProblemDetails;
  public statusCode: number;

  constructor(problemDetails: ProblemDetails, statusCode: number) {
    super(problemDetails.title || `API Error: ${statusCode}`);
    this.name = "ApiError";
    this.problemDetails = problemDetails;
    this.statusCode = statusCode;
  }
}

export const apiEvents = {
  onAuthError: null as ((error: any) => void) | null,

  emitAuthError(error: any) {
    if (this.onAuthError) {
      this.onAuthError(error);
    }
  },
};

export function useApiFetch() {
  const errorStore = useErrorStore();

  return async function apiFetch<T>(
    url: string,
    options: FetchOptions<"json"> = {}
  ): Promise<ApiResponse<T>> {
    // Clear any previous errors
    errorStore.clearErrors();

    const isUserCheck = url === "/auth/session";

    // Prepare fetch options
    const fetchOptions: FetchOptions<"json"> = {
      // Don't retry auth checks
      retry: isUserCheck ? 0 : 1,
      timeout: isUserCheck ? 3000 : 30000, // Shorter timeout for auth checks

      onResponse(context) {
        const { response } = context;
        const body = response._data;

        if (
          response.ok &&
          (typeof body !== "object" || body === null || !("payload" in body))
        ) {
          // Only log warnings for non-auth check responses
          if (!isUserCheck) {
            console.warn(
              `API response for ${response.url} doesn't match ApiResponse<T>.`,
              body
            );
          }
        }
      },

      async onResponseError(context) {
        const { request, response } = context;
        const statusCode = response.status;
        const problemDetails = response._data as ProblemDetails;

        // Don't log 401 errors for auth checks
        if (!(isUserCheck && statusCode === 401)) {
          console.error(
            `API Request Error (${statusCode}) for ${request}:`,
            problemDetails || response._data
          );
        }

        // Store the error in the error store if we have problem details
        if (problemDetails) {
          errorStore.setError(problemDetails);
        }

        // Special handling for 401 (Unauthorized)
        if (statusCode === 401) {
          // Emit auth error event for the auth system to handle
          apiEvents.emitAuthError({
            statusCode,
            isUserCheck,
            url: request,
          });

          // For user checks, just return the error
          if (isUserCheck) {
            console.log("Auth check returned 401 - not authenticated");
            throw new Error("UnauthorizedRedirect");
          } else {
            // For other requests, throw an error that will redirect to login
            throw new Error("UnauthorizedRedirect");
          }
        } else if (
          problemDetails &&
          typeof problemDetails === "object" &&
          problemDetails.title
        ) {
          throw new ApiError(problemDetails, statusCode);
        } else {
          const message =
            response._data?.message || "An unexpected API error occurred.";
          // Store generic errors as well
          errorStore.setError({
            title: "Error",
            detail: `API Error (${statusCode}): ${message}`,
          });
          throw new Error(`API Error (${statusCode}): ${message}`);
        }
      },

      ...options,
    };

    try {
      // Use the baseFetch utility function
      const result = await baseFetch<ApiResponse<T>>(url, fetchOptions);
      return result;
    } catch (error) {
      if (
        error instanceof Error &&
        (error.message.includes("Failed to fetch") ||
          error.message.includes("ERR_CONNECTION_REFUSED") ||
          error.message.includes("network error") ||
          error.message.includes("fetch failed"))
      ) {
        errorStore.setError({
          title: "Connection Error",
          detail: error.message || "Unable to connect to the server.",
        });

        // For auth checks specifically, convert connection errors to 401s
        if (isUserCheck) {
          throw new Error("UnauthorizedRedirect");
        }

        throw error;
      }

      // For "UnauthorizedRedirect" errors, we might want to redirect
      if (
        error instanceof Error &&
        error.message === "UnauthorizedRedirect" &&
        !isUserCheck
      ) {
        // Only navigate on client-side
        if (import.meta.client) {
          // Allow time for any auth error handlers to process
          setTimeout(() => {
            navigateTo("/login", { replace: true });
          }, 0);
        }
      }

      throw error;
    }
  };
}

export function useApi() {
  const apiFetch = useApiFetch();

  return {
    get: <T>(url: string, options: FetchOptions<"json"> = {}) =>
      apiFetch<T>(url, { method: "GET", ...options }),
    post: <T>(url: string, data?: any, options: FetchOptions<"json"> = {}) =>
      apiFetch<T>(url, { method: "POST", body: data, ...options }),
    put: <T>(url: string, data?: any, options: FetchOptions<"json"> = {}) =>
      apiFetch<T>(url, { method: "PUT", body: data, ...options }),
    delete: <T>(url: string, options: FetchOptions<"json"> = {}) =>
      apiFetch<T>(url, { method: "DELETE", ...options }),
    patch: <T>(url: string, data?: any, options: FetchOptions<"json"> = {}) =>
      apiFetch<T>(url, { method: "PATCH", body: data, ...options }),
  };
}
