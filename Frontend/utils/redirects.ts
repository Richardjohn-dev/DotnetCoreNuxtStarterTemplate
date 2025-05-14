// utils/redirects.ts

/**
 * Redirects the user to the login page
 * @param currentPath Current path to use for redirect back after login
 * @param options Navigation options
 */
export function redirectToLogin(
  currentPath: string,
  options: { replace?: boolean } = {}
): ReturnType<typeof navigateTo> {
  const redirect =
    currentPath !== "/" ? `?redirect=${encodeURIComponent(currentPath)}` : "";

  return navigateTo(`/login${redirect}`, { replace: options.replace ?? true });
}

/**
 * Redirects after successful login
 * @param targetPath Target path from redirect param, or null
 * @param fallback Fallback path if no target provided
 */
export function redirectAfterLogin(
  targetPath: string | null = null,
  fallback: string = "/"
): ReturnType<typeof navigateTo> {
  const path = targetPath || fallback;
  return navigateTo(path, { replace: true });
}

/**
 * Redirects to the forbidden page
 */
export function redirectToForbidden(): ReturnType<typeof navigateTo> {
  return navigateTo("/forbidden", { replace: true });
}

/**
 * Redirects to the external auth callback handler
 * @param success Whether the auth was successful
 * @param error Optional error message
 * @param returnUrl URL to return to after auth
 */
export function redirectToAuthCallback(
  success: boolean,
  error?: string,
  returnUrl?: string
): ReturnType<typeof navigateTo> {
  const query: Record<string, string> = {
    success: success.toString(),
  };

  if (error) query.error = error;
  if (returnUrl) query.returnUrl = returnUrl;

  return navigateTo(
    {
      path: "/auth/external-callback",
      query,
    },
    { replace: true }
  );
}
