// store/authStore.ts - Simplified without isLoggedIn
import { baseFetch } from "~/utils/fetchUtils";
import type { UserInfoResponse } from "~/types/auth";
import type { ApiResponse } from "~/types";
import { redirectToLogin } from "~/utils/redirects";

interface AuthState {
  user: UserInfoResponse | null;
  isLoading: boolean;
  loadingOperation: string | null; // Track which operation is loading
  isInitialized: boolean;
  lastCheckedAt: number;
}

export const useAuthStore = defineStore("auth", {
  state: (): AuthState => ({
    user: null,
    isLoading: false,
    loadingOperation: null,
    isInitialized: false,
    lastCheckedAt: 0,
  }),

  getters: {
    // Infer authentication status directly from user
    isAuthenticated(): boolean {
      return !!this.user;
    },
    currentUser(): UserInfoResponse | null {
      return this.user;
    },
    isAdmin(): boolean {
      return this.user?.roles?.includes("Admin") ?? false;
    },
    isPremiumUser(): boolean {
      if (!this.user?.roles) return false;
      return this.user.roles.some((role) =>
        ["PremiumUser", "Admin"].includes(role)
      );
    },
    roles(): string[] {
      return this.user?.roles || [];
    },
    email(): string {
      return this.user?.email || "";
    },
    userId(): string {
      return this.user?.userId || "";
    },

    // Determine if we need a fresh auth check
    needsFreshAuthCheck(): boolean {
      if (this.isLoading) return false; // Skip if already loading

      // Always check if not initialized
      if (!this.isInitialized) return true;

      // Check if the last check was more than 5 minutes ago
      const timeSinceLastCheck = Date.now() - this.lastCheckedAt;
      return timeSinceLastCheck > 5 * 60 * 1000; // 5 minutes
    },
  },

  actions: {
    // Set the user and update auth state
    setUser(user: UserInfoResponse | null) {
      this.user = user;
      // No need to set isLoggedIn as it's inferred from user
      this.isLoading = false;
      this.isInitialized = true;
      this.lastCheckedAt = Date.now();
    },

    // Start loading state
    startLoading(operation: string = "general") {
      this.isLoading = true;
      this.loadingOperation = operation;
    },
    stopLoading() {
      this.isLoading = false;
      this.loadingOperation = null;
    },

    // Reset loading state (can be called if loading gets stuck)
    resetLoadingState() {
      this.isLoading = false;
      this.loadingOperation = null;
    },

    // Initialize auth event handlers
    setupAuthEvents() {
      // Handle authentication errors from API calls
      apiEvents.onAuthError = (error) => {
        // If it's not a user check, log the user out
        if (!error.isUserCheck) {
          this.setUser(null);
          if (import.meta.client) {
            navigateTo("/login", { replace: true });
          }
        }
      };
    },

    // Fetch user info
    async fetchUser(force = false, operation = "fetchUser") {
      // Skip check if already loaded and not forced
      if (
        this.user && // Check if user exists instead of isLoggedIn
        this.isInitialized &&
        !force &&
        !this.needsFreshAuthCheck
      ) {
        return this.user;
      }

      // Skip if already loading and not forced
      if (this.isLoading && !force) {
        return this.user;
      }

      // Start loading
      this.startLoading(operation);

      try {
        const userInfo = await this.fetchUserFromApi();

        if (userInfo) {
          this.setUser(userInfo);
          return userInfo;
        } else {
          this.setUser(null);
          return null;
        }
      } catch (error) {
        this.setUser(null);
        return null;
      } finally {
        this.stopLoading();
        this.isInitialized = true;
      }
    },

    // Direct API call to fetch user info - without using the API client
    async fetchUserFromApi() {
      try {
        const response = await baseFetch<ApiResponse<UserInfoResponse>>(
          "/auth/session"
        );

        return response.payload;
      } catch (error) {
        // console.error("Error fetching user from API:", error);
        return null;
      }
    },

    // Check authentication status
    async checkAuthStatus(force = false, operation = "checkAuth") {
      if (force || !this.isInitialized || this.needsFreshAuthCheck) {
        return await this.fetchUser(force, operation);
      } else {
        return this.user;
      }
    },
    // Logout the user
    async logout() {
      console.log("Auth store: Logging out user");
      this.startLoading("logout");

      try {
        await baseFetch("/auth/logout", { method: "POST" });
      } catch (error) {
        console.error("Logout API call failed:", error);
      } finally {
        // Always clear auth state
        this.setUser(null);
        this.stopLoading();

        // Navigate to login page
        if (import.meta.client) {
          redirectToLogin("/");
        }
      }
    },
  },
});
