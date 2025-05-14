// middleware/auth.global.ts
import { useAuthStore } from "~/store/authStore";
import { redirectToLogin, redirectToForbidden } from "~/utils/redirects";

export default defineNuxtRouteMiddleware(async (to) => {
  if (to.meta.public) {
    return;
  }

  // Only run auth checks on client-side to avoid hydration issues
  if (import.meta.client) {
    const authStore = useAuthStore();

    try {
      // console.log("Auth middleware: Checking auth", {
      //   isAuthenticated: authStore.isAuthenticated,
      //   isInitialized: authStore.isInitialized,
      // });

      // Check authentication status if not already initialized or needs fresh check
      if (!authStore.isInitialized || authStore.needsFreshAuthCheck) {
        await authStore.checkAuthStatus();
      }

      if (!authStore.isAuthenticated) {
        return redirectToLogin(to.fullPath);
      }

      // Check for required roles
      const requiredRoles = to.meta.roles as string[] | undefined;
      if (requiredRoles?.length) {
        const userRoles = authStore.roles;
        const hasRequiredRole = requiredRoles.some((role) =>
          userRoles.includes(role)
        );

        if (!hasRequiredRole) {
          // console.warn(`Auth Middleware: Role check failed for ${to.path}. ` + `User roles: ${userRoles.join(", ")}. ` + `Required roles: ${requiredRoles.join(", ")}.`);
          return redirectToForbidden();
        }
      }
    } catch (error) {
      console.error("Auth check failed in middleware:", error);
      return redirectToLogin("/");
    }
  }
});
