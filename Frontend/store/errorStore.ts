// store/errorStore.ts
import { defineStore } from "pinia";
import type { ProblemDetails } from "~/types";

interface ErrorState {
  title: string | null;
  detail: string | null;
  errors: Record<string, string[]> | null;
  validationErrors: Record<string, string[]> | null;
}

export const useErrorStore = defineStore("error", {
  state: (): ErrorState => ({
    title: null,
    detail: null,
    errors: null,
    validationErrors: null,
  }),

  getters: {
    displayGlobalError: (state): boolean => {
      // If it's a validation error, don't display globally
      if (state.title === "Validation Error") {
        return false;
      }

      // Display if we have a title/detail or general errors
      return (
        !!state.title ||
        !!state.detail ||
        (!!state.errors && Object.keys(state.errors).length > 0)
      );
    },

    hasValidationErrors: (state): boolean =>
      !!state.validationErrors &&
      Object.keys(state.validationErrors).length > 0,

    isValidationError: (state): boolean => state.title === "Validation Error",
  },

  actions: {
    setError(error: unknown) {
      this.clearErrors();

      if (error instanceof Error) {
        // Handle regular JS errors
        this.title = error.name;
        this.detail = error.message;
      } else if (error && typeof error === "object" && "title" in error) {
        // Handle ProblemDetails object
        const problemDetails = error as ProblemDetails;
        this.title = problemDetails.title || null;
        this.detail = problemDetails.detail || null;

        if (problemDetails.title === "Internal Server Error") {
        }
        if (problemDetails.errors) {
          if (problemDetails.title === "Validation Error") {
            this.validationErrors = problemDetails.errors;
          } else {
            this.errors = problemDetails.errors;
          }
        }
      } else {
        // Fallback for other error types
        this.title = "Error";
        this.detail =
          typeof error === "string" ? error : "An unexpected error occurred";
      }
    },

    clearErrors() {
      this.title = null;
      this.detail = null;
      this.errors = null;
      this.validationErrors = null;
    },
  },
});
