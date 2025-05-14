export async function baseFetch<T>(url: string, options: any = {}): Promise<T> {
  const config = useRuntimeConfig();
  // Use the cookie headers from the request for SSR
  const headers = import.meta.server
    ? (useRequestHeaders(["cookie"]) as HeadersInit)
    : undefined;

  return await $fetch<T>(url, {
    baseURL: config.public.apiBaseUrl,
    headers: {
      Accept: "application/json",
      "X-Requested-With": "XMLHttpRequest",
      ...headers,
      ...options.headers,
    },
    credentials: "include",
    method: options.method || "GET",
    body: options.body,
    responseType: "json",
    ...options,
  });
}
