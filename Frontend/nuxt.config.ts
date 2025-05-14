// nuxt.config.ts
import vuetify, { transformAssetUrls } from "vite-plugin-vuetify";
import fs from "fs";
import path from "path";

export default defineNuxtConfig({
  pages: true,
  devtools: { enabled: false },
  css: ["vuetify/styles", "@mdi/font/css/materialdesignicons.min.css"],
  plugins: ["~/plugins/vuetify.ts"],
  components: true,
  build: {
    transpile: ["vuetify"],
  },

  modules: [
    "@pinia/nuxt",
    (_options, nuxt) => {
      nuxt.hooks.hook("vite:extendConfig", (config) => {
        // @ts-expect-error
        config.plugins.push(vuetify({ autoImport: true }));
      });
    },
  ],

  // Add this to help with cookie handling across browsers
  routeRules: {
    "/api/**": { cors: true },
    "/auth/**": {
      /* removed security property */
    },
  },

  vite: {
    vue: {
      template: {
        transformAssetUrls,
      },
    },
    // Optimize server response handling
    server: {
      hmr: {
        timeout: 2000, // Increase HMR timeout for more reliable hot reloading
      },
    },
  },

  devServer: {
    https: {
      key: fs.readFileSync(
        path.resolve(__dirname, "ssl/localhost+2-key.pem"),
        "utf-8"
      ),
      cert: fs.readFileSync(
        path.resolve(__dirname, "ssl/localhost+2.pem"),
        "utf-8"
      ),
    },
    port: 3000,
  },

  // Runtime Config
  runtimeConfig: {
    apiSecret: "",
    public: {
      apiBaseUrl: "https://localhost:7001/api",
      authOrigin: "https://localhost:7001",
    },
  },

  // Add SSR optimization for auth
  nitro: {
    // Remove problematic properties
  },

  compatibilityDate: "2025-04-13",
});
