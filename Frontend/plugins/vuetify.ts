// // plugins/vuetify.ts
// import { createVuetify } from "vuetify";
// import * as components from "vuetify/components";
// import * as directives from "vuetify/directives";

// export default defineNuxtPlugin((nuxtApp) => {
//   const vuetify = createVuetify({
//     ssr: true,
//     components,
//     directives,
//   });
//   nuxtApp.vueApp.use(vuetify);
// });

import { createVuetify } from "vuetify";
import * as components from "vuetify/components";

export default defineNuxtPlugin((nuxtApp) => {
  const vuetify = createVuetify({
    ssr: true,
    theme: {
      defaultTheme: "light",
      themes: {
        light: {
          colors: {
            background: "#edeef2",
            surface: "#f8f9fd",
            "surface-dim": "#d9dade",
            "surface-bright": "#f8f9fd",
            "on-surface": "#191c1f",
            outline: "#71787f",
            "outline-variant": "#c1c7cf",
            primary: "#21638d",
            "on-primary": "#ffffff",
            secondary: "#006877",
            "on-secondary": "#ffffff",
            tertiary: "#4c5c92",
            "on-tertiary": "#ffffff",
            error: "#ba1a1a",
            "on-error": "#ffffff",
            "surface-light": "#e7e8ec",
          },
          dark: false,
          variables: {
            "overlay-background": "#161c22",
          },
        },
        "light-medium-contrast": {
          colors: {
            background: "#e7e8ec",
            surface: "#f8f9fd",
            "surface-dim": "#c5c6ca",
            "surface-bright": "#f8f9fd",
            "on-surface": "#0f1114",
            outline: "#4c535a",
            "outline-variant": "#676e75",
            primary: "#003a58",
            "on-primary": "#ffffff",
            secondary: "#003c46",
            "on-secondary": "#ffffff",
            tertiary: "#223367",
            "on-tertiary": "#ffffff",
            error: "#740006",
            "on-error": "#ffffff",
            "surface-light": "#e7e8ec",
          },
          dark: false,
          variables: {
            "overlay-background": "#161c22",
          },
        },
        "light-high-contrast": {
          colors: {
            background: "#e1e2e6",
            surface: "#f8f9fd",
            "surface-dim": "#b7b9bc",
            "surface-bright": "#f8f9fd",
            "on-surface": "#000000",
            outline: "#262d33",
            "outline-variant": "#434a51",
            primary: "#002f49",
            "on-primary": "#ffffff",
            secondary: "#00313a",
            "on-secondary": "#ffffff",
            tertiary: "#17295c",
            "on-tertiary": "#ffffff",
            error: "#600004",
            "on-error": "#ffffff",
            "surface-light": "#e7e8ec",
          },
          dark: false,
          variables: {
            "overlay-background": "#161c22",
          },
        },
        dark: {
          colors: {
            background: "#1d2023",
            surface: "#111416",
            "surface-dim": "#111416",
            "surface-bright": "#37393d",
            "on-surface": "#e1e2e6",
            outline: "#8b9199",
            "outline-variant": "#41474e",
            primary: "#93cdfc",
            "on-primary": "#00344f",
            secondary: "#7dd3e6",
            "on-secondary": "#00363f",
            tertiary: "#b5c4ff",
            "on-tertiary": "#1c2d61",
            error: "#ffb4ab",
            "on-error": "#690005",
            "surface-light": "#37393d",
          },
          dark: true,
          variables: {
            "overlay-background": "#161c22",
          },
        },
        "dark-medium-contrast": {
          colors: {
            background: "#25282b",
            surface: "#111416",
            "surface-dim": "#111416",
            "surface-bright": "#424548",
            "on-surface": "#ffffff",
            outline: "#acb3ba",
            "outline-variant": "#8a9199",
            primary: "#bee0ff",
            "on-primary": "#00283f",
            secondary: "#93e9fd",
            "on-secondary": "#002a32",
            tertiary: "#d3dbff",
            "on-tertiary": "#0f2255",
            error: "#ffd2cc",
            "on-error": "#540003",
            "surface-light": "#37393d",
          },
          dark: true,
          variables: {
            "overlay-background": "#161c22",
          },
        },
        "dark-high-contrast": {
          colors: {
            background: "#2e3134",
            surface: "#111416",
            "surface-dim": "#111416",
            "surface-bright": "#4e5054",
            "on-surface": "#ffffff",
            outline: "#eaf1f9",
            "outline-variant": "#bdc3cb",
            primary: "#e5f1ff",
            "on-primary": "#000000",
            secondary: "#d3f6ff",
            "on-secondary": "#000000",
            tertiary: "#eeefff",
            "on-tertiary": "#000000",
            error: "#ffece9",
            "on-error": "#000000",
            "surface-light": "#37393d",
          },
          dark: true,
          variables: {
            "overlay-background": "#161c22",
          },
        },
      },
    },
  });

  nuxtApp.vueApp.use(vuetify);
});
