// plugins/00.domain-properties.ts
import { defineNuxtPlugin } from "#app";
import { useDomainStore } from "~/store/domainStore";

export default defineNuxtPlugin(async (nuxtApp) => {
  const domainStore = useDomainStore();
  if (import.meta.client) {
    try {
      if (!domainStore.hasLoaded || domainStore.needsRefresh) {
        await domainStore.fetchDomainProperties();
      }
    } catch (error) {
      console.error("Failed to load domain properties:", error);
    }
  }

  // Expose a helper method to refresh domain properties
  nuxtApp.provide("refreshDomainProperties", async () => {
    await domainStore.fetchDomainProperties();
  });
});
