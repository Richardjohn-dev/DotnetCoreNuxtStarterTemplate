// layouts/default.vue
<template>
  <v-app>
    <!-- Left sidebar navigation -->
    <v-navigation-drawer
      v-model="drawer"
      :theme="theme.global.name.value"
      class="rounded-tr-xl rounded-br-xl"
      elevation="3"
    >
      <div class="pa-4 d-flex align-center">
        <v-avatar color="primary" size="40" class="mr-3">
          <span class="text-h6 font-weight-bold">A</span>
        </v-avatar>
        <span class="text-h6 font-weight-medium">Auth Template</span>
      </div>

      <v-divider class="my-3"></v-divider>

      <!-- Static navigation that doesn't depend on auth -->
      <v-list nav density="compact">
        <v-list-item to="/" prepend-icon="mdi-home" title="Home"></v-list-item>
      </v-list>

      <!-- Auth-dependent navigation items wrapped in client-only -->
      <client-only>
        <v-list nav density="compact">
          <!-- Loading state while auth initializes -->
          <template v-if="!auth.isHydrated">
            <v-list-item>
              <template v-slot:prepend>
                <v-progress-circular
                  indeterminate
                  size="20"
                  width="2"
                  color="primary"
                  class="mr-2"
                ></v-progress-circular>
              </template>
              <v-list-item-title>Loading...</v-list-item-title>
            </v-list-item>
          </template>

          <!-- Auth menu items after hydration -->
          <template v-else>
            <!-- Not authenticated menu items -->
            <template v-if="!auth.isAuthenticated.value">
              <v-list-item
                to="/login"
                prepend-icon="mdi-login"
                title="Login"
              ></v-list-item>
              <v-list-item
                to="/register"
                prepend-icon="mdi-account-plus"
                title="Register"
              ></v-list-item>
            </template>

            <!-- Authenticated menu items -->
            <template v-else>
              <v-list-item
                to="/dashboard"
                prepend-icon="mdi-view-dashboard"
                title="Dashboard"
              ></v-list-item>

              <v-list-item
                to="/profile"
                prepend-icon="mdi-account-circle"
                title="My Profile"
              ></v-list-item>

              <v-list-item
                v-if="auth.isPremiumUser.value"
                to="/premium"
                prepend-icon="mdi-star"
                title="Premium Features"
              ></v-list-item>

              <v-list-item
                v-if="auth.isAdmin.value"
                to="/admin"
                prepend-icon="mdi-shield-crown"
                title="Admin Area"
              ></v-list-item>

              <v-list-item
                @click="auth.logout()"
                prepend-icon="mdi-logout"
                title="Logout"
              ></v-list-item>
            </template>
          </template>
        </v-list>
      </client-only>
    </v-navigation-drawer>

    <!-- Top app bar -->
    <v-app-bar
      :elevation="2"
      :theme="theme.global.name.value"
      class="rounded-bl-xl"
    >
      <v-app-bar-nav-icon @click="drawer = !drawer"></v-app-bar-nav-icon>

      <v-app-bar-title class="font-weight-bold">
        <NuxtLink to="/" class="text-decoration-none">
          Modern Auth Template
        </NuxtLink>
      </v-app-bar-title>

      <v-spacer></v-spacer>

      <!-- Theme selector -->
      <v-menu>
        <template v-slot:activator="{ props }">
          <v-btn icon v-bind="props">
            <v-icon>{{
              isDark ? "mdi-weather-night" : "mdi-weather-sunny"
            }}</v-icon>
          </v-btn>
        </template>
        <v-list>
          <v-list-item
            @click="toggleTheme('light')"
            prepend-icon="mdi-weather-sunny"
            title="Light"
          ></v-list-item>
          <v-list-item
            @click="toggleTheme('dark')"
            prepend-icon="mdi-weather-night"
            title="Dark"
          ></v-list-item>
        </v-list>
      </v-menu>

      <!-- Auth Profile Section - properly wrapped with client-only -->
      <client-only>
        <AuthLoadingIndicator size="24" class="ml-2" />

        <div
          v-if="auth.isAuthenticated.value && !auth.isLoading.value"
          class="d-inline-block ml-2"
        >
          <v-menu location="bottom">
            <template v-slot:activator="{ props }">
              <v-chip
                v-bind="props"
                color="primary"
                variant="elevated"
                class="ma-2"
                prepend-avatar="https://i.pravatar.cc/150?img=7"
              >
                {{ auth.user.value?.email || "" }}
              </v-chip>
            </template>

            <v-card min-width="250">
              <v-list>
                <v-list-item>
                  <template v-slot:prepend>
                    <v-avatar color="primary">
                      <v-icon>mdi-account</v-icon>
                    </v-avatar>
                  </template>
                  <v-list-item-title class="text-subtitle-1 font-weight-bold">
                    {{ auth.user.value?.email || "" }}
                  </v-list-item-title>
                  <v-list-item-subtitle>
                    <v-chip-group>
                      <v-chip
                        v-for="role in auth.roles.value"
                        :key="role"
                        size="x-small"
                        variant="outlined"
                      >
                        {{ role }}
                      </v-chip>
                    </v-chip-group>
                  </v-list-item-subtitle>
                </v-list-item>

                <v-divider></v-divider>

                <v-list-item to="/profile" prepend-icon="mdi-account-details">
                  Profile
                </v-list-item>

                <v-list-item @click="auth.logout()" prepend-icon="mdi-logout">
                  Logout
                </v-list-item>
              </v-list>
            </v-card>
          </v-menu>
        </div>
      </client-only>
    </v-app-bar>

    <!-- Main content area -->
    <v-main>
      <v-container fluid class="py-8 px-6">
        <slot />
      </v-container>
    </v-main>

    <!-- Global error notification -->
    <!-- <v-snackbar
      v-model="showErrorSnackbar"
      :timeout="-1"
      color="error"
      location="top right"
    >
      {{ snackbarErrorTitle }}

      <template v-slot:actions>
        <v-btn color="white" variant="text" @click="errorStore.clearErrors()">
          Close
        </v-btn>
      </template>
    </v-snackbar> -->

    <v-snackbar
      v-model="snackbarVisible"
      :timeout="snackbarTimeout"
      color="error"
      location="top right"
      @timeout="onTimeout"
    >
      <div class="text-white font-weight-bold">
        {{ snackbarErrorTitle }}
        <v-btn
          size="x-small"
          variant="text"
          color="white"
          icon
          @click="toggleExpand"
        >
          <v-icon>
            {{ isExpanded ? "mdi-chevron-up" : "mdi-chevron-down" }}
          </v-icon>
        </v-btn>
      </div>

      <v-expand-transition>
        <div v-if="isExpanded" class="mt-2 text-white text-body-2">
          {{ snackbarErrorMessage }}
        </div>
      </v-expand-transition>

      <template v-slot:actions>
        <v-btn
          size="x-small"
          variant="text"
          color="white"
          icon
          @click="manualCloseSnackbar"
        >
          <v-icon>
            {{ "mdi-close" }}
          </v-icon>
        </v-btn>
      </template>
    </v-snackbar>

    <!-- Footer -->
    <v-footer class="d-flex flex-column">
      <div>
        <v-btn
          v-for="icon in icons"
          :key="icon"
          class="mx-2"
          :icon="icon"
          variant="text"
          density="comfortable"
        ></v-btn>
      </div>
      <div class="text-center text-caption mt-1">
        &copy; {{ new Date().getFullYear() }} Modern Auth Template
      </div>
    </v-footer>
  </v-app>
</template>

<script setup lang="ts">
import { useTheme } from "vuetify";
import { useErrorStore } from "~/store/errorStore";
import { useAuth } from "~/composables/useAuth";
import AuthLoadingIndicator from "~/components/auth/AuthLoadingIndicator.vue";

const errorStore = useErrorStore();

const { displayGlobalError, title, errors, detail } = storeToRefs(errorStore);

const drawer = ref(false);
const theme = useTheme();
const auth = useAuth();

const isChecking = ref(false);

const isDark = computed(() => theme.global.current.value.dark);
const icons = ["mdi-facebook", "mdi-twitter", "mdi-linkedin", "mdi-instagram"];

// snackbar

const snackbarVisible = ref(false);
const isExpanded = ref(false);
const snackbarTimeout = ref(10000); // 10 seconds

// Watch displayGlobalError to update snackbarVisible
watch(
  displayGlobalError,
  (newValue) => {
    snackbarVisible.value = newValue;
    if (newValue) {
      // Reset expansion state and set initial timeout when an error appears
      isExpanded.value = false;
      snackbarTimeout.value = 10000; // Initial 10 second timeout
      console.log("Error appeared, starting 10 second timer");
    }
  },
  { immediate: true }
);

watch(snackbarVisible, (newValue) => {
  if (!newValue && displayGlobalError.value) {
    closeSnackbar();
  }
});

const closeSnackbar = () => {
  isExpanded.value = false;
  snackbarVisible.value = false;
};

const manualCloseSnackbar = () => {
  closeSnackbar();
  errorStore.clearErrors();
};

const onTimeout = () => {
  console.log("Snackbar timeout triggered");
  closeSnackbar();
};

const snackbarErrorTitle = computed(() => {
  return displayGlobalError.value ? title.value || "Error:" : null;
});
const snackbarErrorMessage = computed(() => {
  if (!displayGlobalError.value) return null;

  if (errors.value && Object.keys(errors.value).length > 0) {
    return Object.values(errors.value).flat().join(", ");
  }

  return detail.value || "An unexpected error occurred.";
});

const toggleExpand = () => {
  isExpanded.value = !isExpanded.value;

  if (isExpanded.value) {
    // When expanded, extend the timeout to 20 seconds from now
    snackbarTimeout.value += 10000;
    console.log("Expanded, adding 10 seconds to timer");
  } else {
    // When collapsed, reset to 10 seconds
    snackbarTimeout.value = 10000;
    console.log("Collapsed, resetting timer to 10 seconds");
  }
};

const toggleTheme = (themeName: string) => {
  theme.global.name.value = themeName;
  // Only save theme on client side
  if (import.meta.client) {
    localStorage.setItem("theme", themeName);
  }
};

onMounted(async () => {
  if (import.meta.client) {
    const savedTheme = localStorage.getItem("theme");
    if (savedTheme) {
      theme.global.name.value = savedTheme;
    }

    isChecking.value = true;
    try {
      const authResult = await auth.checkAuth();
      if (!authResult.success) {
        // console.log("Auth check failed:", authResult.error);
      }
    } finally {
      isChecking.value = false;
    }

    setTimeout(async () => {
      if (isChecking.value) {
        console.log("Safety timeout: resetting checking state");
        isChecking.value = false;
        const authResult = await auth.checkAuth(true);
        if (!authResult.success) {
          // console.log("Auth check failed:", authResult.error);
        }
      }
    }, 3000);
  }
});
</script>
