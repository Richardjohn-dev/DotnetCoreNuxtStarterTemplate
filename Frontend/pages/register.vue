<template>
  <v-container class="fill-height" fluid>
    <v-row align="center" justify="center">
      <v-col cols="12" xl="6" lg="7" sm="6" md="7" max-width="800px">
        <v-card class="elevation-12 rounded-lg">
          <v-toolbar color="primary">
            <v-toolbar-title>Create Account</v-toolbar-title>
          </v-toolbar>

          <v-card-text class="pa-6">
            <!-- Loading state while fetching domain properties -->
            <client-only>
              <div v-if="formLoading" class="text-center py-4">
                <v-progress-circular
                  indeterminate
                  color="primary"
                ></v-progress-circular>
                <div class="mt-2">Loading registration options...</div>
              </div>

              <div v-else>
                <DisplayAuthenticationErrors />

                <v-form v-model="formValid">
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
                    :disabled="isRegistering"
                  ></v-text-field>

                  <v-text-field
                    v-model="fullname"
                    :rules="requiredRules"
                    label="Full Name"
                    prepend-inner-icon="mdi-account"
                    variant="outlined"
                    required
                    autocomplete="name"
                    class="mb-4"
                    :disabled="isRegistering"
                  ></v-text-field>

                  <v-row cols="12">
                    <v-col sm="12" md="6">
                      <!-- Password fields on the left -->
                      <v-text-field
                        v-model="password"
                        :rules="passwordRules"
                        label="Password"
                        prepend-inner-icon="mdi-lock"
                        variant="outlined"
                        :type="showPassword ? 'text' : 'password'"
                        required
                        autocomplete="new-password"
                        class="mb-4"
                        :append-inner-icon="
                          showPassword ? 'mdi-eye-off' : 'mdi-eye'
                        "
                        @click:append-inner="showPassword = !showPassword"
                        :disabled="isRegistering"
                      ></v-text-field>

                      <v-text-field
                        v-model="confirmPassword"
                        :rules="confirmPasswordRules"
                        label="Confirm Password"
                        prepend-inner-icon="mdi-lock-check"
                        variant="outlined"
                        :type="showPassword ? 'text' : 'password'"
                        required
                        autocomplete="new-password"
                        class="mb-4"
                        :disabled="isRegistering"
                      ></v-text-field>
                    </v-col>

                    <v-col>
                      <!-- Password policy on the right -->
                      <DisplayPasswordPolicy
                        :password="password"
                        :policy="passwordPolicy"
                      />
                    </v-col>
                  </v-row>

                  <v-select
                    v-model="selectedRole"
                    :items="domainStore.supportedRoles"
                    label="Account Type"
                    prepend-inner-icon="mdi-account-cog"
                    variant="outlined"
                    item-title="displayName"
                    item-value="role"
                    class="mb-4"
                    :disabled="isRegistering"
                  />

                  <v-btn
                    color="primary"
                    block
                    @click="handleRegister"
                    size="large"
                    :loading="
                      isRegistering &&
                      auth.loadingOperation.value === 'register'
                    "
                    :disabled="!formValid || isRegistering"
                    class="mt-2"
                  >
                    Register
                  </v-btn>
                </v-form>
              </div>
            </client-only>

            <v-divider class="my-4">
              <span class="text-caption">OR</span>
            </v-divider>

            <v-btn
              block
              color="error"
              @click="handleGoogleLogin"
              prepend-icon="mdi-google"
              variant="elevated"
              :loading="
                isRegistering && auth.loadingOperation.value === 'googleLogin'
              "
              :disabled="isRegistering"
              class="mb-4"
            >
              Sign up with Google
            </v-btn>

            <div class="text-center mt-4">
              Already have an account?
              <v-btn variant="text" to="/login" color="primary">
                Sign in
              </v-btn>
            </div>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup lang="ts">
import { useAuth } from "~/composables/useAuth";
import { useDomainStore } from "~/store/domainStore";
import type { RegisterRequest } from "~/types/auth";
import type { PasswordPolicy, RoleDto } from "~/types";
import DisplayAuthenticationErrors from "~/components/auth/AuthenticationErrors.vue";
import DisplayPasswordPolicy from "~/components/auth/PasswordPolicy.vue";
import { redirectAfterLogin } from "~/utils/redirects";

// Initialize stores
const auth = useAuth();
const domainStore = useDomainStore();
const route = useRoute();
const config = useRuntimeConfig();

// Form field values
const email = ref("");
const fullname = ref("");
const password = ref("");
const confirmPassword = ref("");
const selectedRole = ref<RoleDto | null>(null);
const formLoading = ref(true);
const showPassword = ref(false);

// Form validation rules
const formValid = ref(false); // Form validation state
const requiredRules: Array<(value: string) => true | string> = [
  (value: string) => !!value || "This field is required.",
];

// Create a computed for all registration-related loading states
const isRegistering = computed(
  () =>
    auth.isLoading.value &&
    (auth.loadingOperation.value === "register" ||
      auth.loadingOperation.value === "googleLogin")
);

const passwordRules: Array<(value: string) => true | string> = [
  (value: string) => !!value || "Password is required.",
  (value: string) => {
    if (!passwordPolicy.value) return true;
    return (
      value.length >= passwordPolicy.value.minLength ||
      `Password must be at least ${passwordPolicy.value.minLength} characters.`
    );
  },
  (value: string) => {
    if (!passwordPolicy.value?.requiresUppercase) return true;
    return /[A-Z]/.test(value) || "Password must contain an uppercase letter.";
  },
  (value: string) => {
    if (!passwordPolicy.value?.requiresLowercase) return true;
    return /[a-z]/.test(value) || "Password must contain a lowercase letter.";
  },
  (value: string) => {
    if (!passwordPolicy.value?.requiresDigit) return true;
    return /\d/.test(value) || "Password must contain a digit.";
  },
  (value: string) => {
    if (!passwordPolicy.value?.requiresSpecialCharacter) return true;
    return (
      /[!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?]/.test(value) ||
      "Password must contain a special character."
    );
  },
];

const confirmPasswordRules: Array<(value: string) => true | string> = [
  (value: string) => !!value || "Confirm Password is required.",
  (value: string) => value === password.value || "Passwords do not match.",
];

// Password policy
const passwordPolicy = computed<PasswordPolicy | null>(
  () => domainStore.passwordPolicy
);

onMounted(async () => {
  if (import.meta.client) {
    try {
      await domainStore.ensureLoaded();

      if (domainStore.supportedRoles.length > 0) {
        initializeDefaultRole();
      }
    } finally {
      formLoading.value = false;
    }
  }
});

// Initialize the default selected role
function initializeDefaultRole() {
  if (domainStore.supportedRoles.length > 0) {
    // Find BasicUser role or use the first available role
    const basicRole =
      domainStore.supportedRoles.find((r) => r.roleName === "Basic User") ||
      domainStore.supportedRoles[0];

    selectedRole.value = basicRole;
  }
}

// Registration handler
async function handleRegister() {
  // No need to manually set isLoading anymore
  const registerRequest: RegisterRequest = {
    email: email.value,
    fullname: fullname.value,
    password: password.value,
    confirmPassword: confirmPassword.value,
    role: selectedRole.value!.role ?? 0,
  };

  const result = await auth.register(registerRequest);
  if (result.success) {
    return redirectAfterLogin(route.query.redirect as string);
  }
}

// Google OAuth login handler
async function handleGoogleLogin() {
  // Only run on client-side
  if (import.meta.client) {
    // Start loading state for Google login
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

    // No need to stop loading because of redirect
  }
}

// Define page metadata
definePageMeta({
  public: true,
  title: "Register",
});
</script>
