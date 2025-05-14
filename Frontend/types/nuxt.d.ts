// types/nuxt.d.ts
import "@nuxt/schema";

declare module "@nuxt/schema" {
  interface NuxtConfig {
    auth?: {
      baseURL?: string;
      provider?: {
        type?: string;
        endpoints?: {
          signIn?: { path: string; method: string };
          signOut?: { path: string; method: string };
          getSession?: { path: string; method: string };
        };
        pages?: {
          login?: string;
        };
        token?: {
          signInResponseTokenPointer?: string;
          maxAgeInSeconds?: number;
        };
      };
      session?: {
        enableRefreshPeriodically?: number;
        enableRefreshOnWindowFocus?: boolean;
      };
      globalAppMiddleware?: {
        isEnabled?: boolean;
        allow404WithoutAuth?: boolean;
        authenticatedNavigations?: string[];
      };
    };
    security?: Record<string, any>;
  }
}
