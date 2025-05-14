<!-- Updated login.vue template section -->
<template>
  <v-container class="fill-height" fluid>
    <v-row align="center" justify="center">
      <v-col cols="12" lg="4" sm="8" md="6" min-width="550px">
        <v-card class="elevation-12 rounded-lg">
          <v-toolbar color="primary">
            <v-toolbar-title>Login</v-toolbar-title>
          </v-toolbar>

          <v-card-text class="pa-6">
            <v-form v-model="formValid">
              <client-only>
                <DisplayAuthenticationErrors />

                <v-text-field
                  v-model="email"
                  :rules="requiredRules"
                  label="Email"
                  prepend-inner-icon="mdi-email"
                  variant="outlined"
                  type="email"
                  required
                  autocomplete="email"
                  class="mb-4"
                  :disabled="isAuthenticating"
                ></v-text-field>

                <v-text-field
                  v-model="password"
                  :rules="requiredRules"
                  label="Password"
                  prepend-inner-icon="mdi-lock"
                  variant="outlined"
                  :type="showPassword ? 'text' : 'password'"
                  required
                  autocomplete="current-password"
                  :append-inner-icon="showPassword ? 'mdi-eye-off' : 'mdi-eye'"
                  @click:append-inner="showPassword = !showPassword"
                  class="mb-4"
                  :disabled="isAuthenticating"
                ></v-text-field>
              </client-only>
              <v-btn
                color="primary"
                block
                @click="handleLogin"
                size="large"
                :loading="isAuthenticating"
                class="mt-2"
                :disabled="!formValid"
              >
                Sign In
              </v-btn>
            </v-form>

            <v-divider class="my-4">
              <span class="text-caption">OR</span>
            </v-divider>

            <v-btn
              block
              color="error"
              @click="handleGoogleLogin"
              prepend-icon="mdi-google"
              variant="elevated"
              :loading="isAuthenticating"
              class="mb-4"
            >
              Sign in with Google
            </v-btn>

            <div class="text-center mt-4">
              Don't have an account?
              <v-btn variant="text" to="/register" color="primary">
                Register now
              </v-btn>
            </div>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup lang="ts">
import { useErrorStore } from "~/store/errorStore";
import { useAuth } from "~/composables/useAuth";
import type { LoginRequest } from "~/types/auth";
import DisplayAuthenticationErrors from "~/components/auth/AuthenticationErrors.vue";
import { redirectAfterLogin } from "~/utils/redirects";

const auth = useAuth();
const errorStore = useErrorStore();
const route = useRoute();
const config = useRuntimeConfig();

// Form state
const email = ref("");
const password = ref("");

const isLoading = ref(false);
const showPassword = ref(false);

// Form validation rules
const formValid = ref(false); // Form validation state
const requiredRules: Array<(value: string) => true | string> = [
  (value: string) => !!value || "This field is required.",
];

const isAuthenticating = computed(
  () =>
    auth.isLoading.value &&
    (auth.loadingOperation.value === "login" ||
      auth.loadingOperation.value === "googleLogin")
);

// Login handler
async function handleLogin() {
  isLoading.value = true;

  try {
    const loginRequest: LoginRequest = {
      email: email.value,
      password: password.value,
    };

    const result = await auth.login(loginRequest);
    if (result.success) {
      return redirectAfterLogin(route.query.redirect as string);
    }
  } finally {
    isLoading.value = false;
  }
}

// Google OAuth login handler
async function handleGoogleLogin() {
  if (import.meta.client) {
    auth.startLoading("googleLogin");
    // Get the current path to redirect back after login
    const returnUrl = route.query.redirect
      ? `https://localhost:3000${route.query.redirect}`
      : "https://localhost:3000";

    const backendUrl = `${
      config.public.apiBaseUrl
    }/auth/login/google?returnUrl=${encodeURIComponent(returnUrl)}`;
    console.log("Redirecting to Google login:", backendUrl);
    window.location.href = backendUrl;
  }
}

// Handle errors passed from external auth callback
onMounted(() => {
  // Only run on client-side
  if (import.meta.client) {
    // Check for error from external auth callback
    const queryError = route.query.error as string;
    if (queryError) {
      errorStore.setError({
        title: "Authentication Failed",
        detail: queryError,
      });
    }
  }
});

// Define page metadata
definePageMeta({
  public: true,
  title: "Login",
});
</script>
