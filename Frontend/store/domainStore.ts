// store/domainStore.ts
import { defineStore } from "pinia";
import { useApi } from "~/composables/useApiFetch";
import {
  type DomainPropertiesDto,
  type RegistrationDetails,
  Role,
} from "~/types";

interface DomainState {
  registrationDetails: RegistrationDetails | null;
  apiVersion: string | null;
  status: "idle" | "loading" | "error" | "success";
  error: string | null;
  lastFetchedAt: number | null;
}

export const useDomainStore = defineStore("domain", {
  state: (): DomainState => ({
    registrationDetails: null,
    apiVersion: null,
    status: "idle",
    error: null,
    lastFetchedAt: null,
  }),

  getters: {
    isLoading: (state) => state.status === "loading",
    hasLoaded: (state) =>
      state.status === "success" && state.registrationDetails !== null,
    supportedRoles: (state) => state.registrationDetails?.supportedRoles || [],
    passwordPolicy: (state) =>
      state.registrationDetails?.passwordPolicy || null,

    // Helper method to get a role by enum value
    getRoleById: (state) => (role: Role) => {
      return state.registrationDetails?.supportedRoles.find(
        (r) => r.role === role
      );
    },

    // Helper method to get a role by name
    getRoleByName: (state) => (roleName: string) => {
      return state.registrationDetails?.supportedRoles.find(
        (r) => r.roleName === roleName
      );
    },

    // Get all role enum values
    roleEnumValues: (state): Role[] => {
      if (!state.registrationDetails?.supportedRoles) return [];
      return state.registrationDetails.supportedRoles.map((r) => r.role);
    },

    // Check if properties need to be refreshed (older than 1 hour)
    needsRefresh: (state) => {
      if (!state.lastFetchedAt) return true;
      const oneHour = 60 * 60 * 1000; // 1 hour in milliseconds
      return Date.now() - state.lastFetchedAt > oneHour;
    },
  },

  actions: {
    async fetchDomainProperties() {
      // Skip if already loading
      if (this.status === "loading") return;

      // Skip if data is fresh and already loaded
      if (this.registrationDetails && !this.needsRefresh) {
        return;
      }

      this.status = "loading";
      this.error = null;

      try {
        const api = useApi();
        const response = await api.get<DomainPropertiesDto>(
          "/domain-properties"
        );
        // Store properties separately
        this.registrationDetails = response.payload.registrationDetails;
        this.apiVersion = response.payload.apiVersion;

        this.status = "success";
        this.lastFetchedAt = Date.now();
      } catch (error) {
        this.status = "error";
        this.error =
          error instanceof Error
            ? error.message
            : "Failed to fetch domain properties";
        throw error; // Re-throw to allow components to handle errors
      }
    },

    // New method for auto-initialization
    async ensureLoaded() {
      if (!this.hasLoaded || this.needsRefresh) {
        return this.fetchDomainProperties();
      }
      return Promise.resolve();
    },

    // Reset the store (useful for testing)
    reset() {
      this.registrationDetails = null;
      this.apiVersion = null;
      this.status = "idle";
      this.error = null;
      this.lastFetchedAt = null;
    },
  },
});
