// composables/useAuth.ts
import { useAuthStore } from "~/store/authStore";
import { useErrorStore } from "~/store/errorStore";
import { useApi } from "~/composables/useApiFetch";
import { storeToRefs } from "pinia";

import type {
  LoginRequest,
  RegisterRequest,
  UserInfoResponse,
  AuthResult,
} from "~/types/auth";

export function useAuth() {
  const api = useApi();
  const authStore = useAuthStore();
  const errorStore = useErrorStore();

  const { user, isLoading, loadingOperation, isInitialized } =
    storeToRefs(authStore);

  const isHydrated = ref(false);

  const localOperationLoading = ref(false);

  if (import.meta.client) {
    onMounted(async () => {
      nextTick(async () => {
        isHydrated.value = true;

        if (!isInitialized.value || authStore.needsFreshAuthCheck) {
          try {
            await authStore.checkAuthStatus(false, "initialCheck");
          } catch (error) {
            console.error("Error during initial auth check:", error);
          }
        }
      });
    });
  }
  const isAuthenticated = computed(() =>
    import.meta.server || !isHydrated.value ? false : authStore.isAuthenticated
  );

  const roles = computed(() =>
    import.meta.server || !isHydrated.value ? [] : authStore.roles
  );

  const isAdmin = computed(() =>
    import.meta.server || !isHydrated.value ? false : authStore.isAdmin
  );

  const isPremiumUser = computed(() =>
    import.meta.server || !isHydrated.value ? false : authStore.isPremiumUser
  );

  const isAuthLoading = computed(() => {
    if (import.meta.server) return true;
    return !isHydrated.value || isLoading.value;
  });

  const resetLoadingState = () => {
    console.log("useAuth: Resetting loading state");
    localOperationLoading.value = false;
    authStore.resetLoadingState();
  };

  return {
    // State
    user,
    isAuthenticated,
    isHydrated,
    isLoading: isAuthLoading,
    loadingOperation,
    roles,
    isAdmin,
    isPremiumUser,

    // Methods
    resetLoadingState,
    startLoading: (operation: string) => authStore.startLoading(operation),
    stopLoading: () => authStore.stopLoading(),

    // Auth operations with explicit handling
    async login(credentials: LoginRequest): Promise<AuthResult> {
      if (import.meta.server || !isHydrated.value) {
        return { success: false, error: "Cannot login during SSR" };
      }

      errorStore.clearErrors();
      authStore.startLoading("login");

      try {
        const response = await api.post<UserInfoResponse>(
          "/auth/login",
          credentials
        );

        authStore.setUser(response.payload);
        return { success: true, data: response.payload };
      } catch (error) {
        return {
          success: false,
          error: error instanceof Error ? error.message : "Login failed",
        };
      } finally {
        authStore.stopLoading();
      }
    },

    // Register
    async register(details: RegisterRequest): Promise<AuthResult> {
      if (import.meta.server || !isHydrated.value) {
        return { success: false, error: "Cannot register during SSR" };
      }

      errorStore.clearErrors();
      authStore.startLoading("register");

      try {
        const response = await api.post<UserInfoResponse>(
          "/auth/register",
          details
        );

        authStore.setUser(response.payload);
        return { success: true, data: response.payload };
      } catch (error) {
        return {
          success: false,
          error: error instanceof Error ? error.message : "Registration failed",
        };
      } finally {
        authStore.stopLoading();
      }
    },

    // Logout
    async logout(): Promise<void> {
      if (import.meta.server || !isHydrated.value) {
        return;
      }
      try {
        await authStore.logout();
      } catch (error) {
        console.error("Logout error:", error);
      }
    },

    async checkAuth(force = false): Promise<AuthResult> {
      if (import.meta.server || !isHydrated.value) {
        return { success: false, error: "Cannot check auth during SSR" };
      }

      try {
        const user = await authStore.checkAuthStatus(force, "checkAuth");
        return {
          success: !!user,
          data: user,
          // Optional: provide context for why auth check failed if user is null
          error: !user ? "Not authenticated" : undefined,
        };
      } catch (error) {
        return {
          success: false,
          error: error instanceof Error ? error.message : "Auth check failed",
        };
      }
    },

    async syncSessionAfterExternalLogin(): Promise<AuthResult> {
      console.log("useAuth: Syncing session after external login");
      if (import.meta.server || !isHydrated.value) {
        return { success: false, error: "Cannot check auth during SSR" };
      }

      try {
        const user = await authStore.fetchUser(true, "syncExternalLogin");
        return {
          success: !!user,
          data: user,
          error: !user
            ? "Failed fetching user after external login"
            : undefined,
        };
      } catch (error) {
        return {
          success: false,
          error:
            error instanceof Error
              ? error.message
              : "Auth check after external login failed",
        };
      }
    },
  };
}
