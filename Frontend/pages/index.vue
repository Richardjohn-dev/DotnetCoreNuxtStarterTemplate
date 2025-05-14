// pages/index.vue
<template>
  <div>
    <!-- Welcome Section - static content, no client-only needed -->
    <v-card class="mb-6">
      <v-card-title class="text-h4"
        >Welcome to Modern Auth Template</v-card-title
      >
      <v-card-text>
        <p class="text-body-1">
          This is a demonstration of a Nuxt 3 frontend with a .NET Core backend.
          The template showcases authentication with JWT tokens, role-based
          access, and Google OAuth integration.
        </p>
      </v-card-text>
    </v-card>

    <!-- Feature Cards - static content, no client-only needed -->
    <!-- <v-row>
      <v-col cols="12" md="4">
        <v-card height="100%">
          <v-card-title>
            <v-icon icon="mdi-shield-account" class="mr-2"></v-icon>
            Authentication
          </v-card-title>
          <v-card-text>
            <p>Complete authentication system with:</p>
            <v-list density="compact">
              <v-list-item>
                <template v-slot:prepend>
                  <v-icon
                    icon="mdi-check"
                    color="success"
                    size="small"
                  ></v-icon>
                </template>
                <v-list-item-title>JWT Token Auth</v-list-item-title>
              </v-list-item>

              <v-list-item>
                <template v-slot:prepend>
                  <v-icon
                    icon="mdi-check"
                    color="success"
                    size="small"
                  ></v-icon>
                </template>
                <v-list-item-title>Email/Password Login</v-list-item-title>
              </v-list-item>

              <v-list-item>
                <template v-slot:prepend>
                  <v-icon
                    icon="mdi-check"
                    color="success"
                    size="small"
                  ></v-icon>
                </template>
                <v-list-item-title>Google OAuth Integration</v-list-item-title>
              </v-list-item>

              <v-list-item>
                <template v-slot:prepend>
                  <v-icon
                    icon="mdi-check"
                    color="success"
                    size="small"
                  ></v-icon>
                </template>
                <v-list-item-title>Refresh Token Support</v-list-item-title>
              </v-list-item>
            </v-list>
          </v-card-text>
        </v-card>
      </v-col>

      <v-col cols="12" md="4">
        <v-card height="100%">
          <v-card-title>
            <v-icon icon="mdi-account-key" class="mr-2"></v-icon>
            Authorization
          </v-card-title>
          <v-card-text>
            <p>Role-based access control with:</p>
            <v-list density="compact">
              <v-list-item>
                <template v-slot:prepend>
                  <v-icon
                    icon="mdi-check"
                    color="success"
                    size="small"
                  ></v-icon>
                </template>
                <v-list-item-title>Route Protection</v-list-item-title>
              </v-list-item>

              <v-list-item>
                <template v-slot:prepend>
                  <v-icon
                    icon="mdi-check"
                    color="success"
                    size="small"
                  ></v-icon>
                </template>
                <v-list-item-title>UI Adaptation by Role</v-list-item-title>
              </v-list-item>

              <v-list-item>
                <template v-slot:prepend>
                  <v-icon
                    icon="mdi-check"
                    color="success"
                    size="small"
                  ></v-icon>
                </template>
                <v-list-item-title>API Permission Controls</v-list-item-title>
              </v-list-item>

              <v-list-item>
                <template v-slot:prepend>
                  <v-icon
                    icon="mdi-check"
                    color="success"
                    size="small"
                  ></v-icon>
                </template>
                <v-list-item-title>Multi-Tier Access Levels</v-list-item-title>
              </v-list-item>
            </v-list>
          </v-card-text>
        </v-card>
      </v-col>

      <v-col cols="12" md="4">
        <v-card height="100%">
          <v-card-title>
            <v-icon icon="mdi-vuetify" class="mr-2"></v-icon>
            Modern UI
          </v-card-title>
          <v-card-text>
            <p>Built with Nuxt 3 and Vuetify:</p>
            <v-list density="compact">
              <v-list-item>
                <template v-slot:prepend>
                  <v-icon
                    icon="mdi-check"
                    color="success"
                    size="small"
                  ></v-icon>
                </template>
                <v-list-item-title>Responsive Design</v-list-item-title>
              </v-list-item>

              <v-list-item>
                <template v-slot:prepend>
                  <v-icon
                    icon="mdi-check"
                    color="success"
                    size="small"
                  ></v-icon>
                </template>
                <v-list-item-title
                  >Component-Based Architecture</v-list-item-title
                >
              </v-list-item>

              <v-list-item>
                <template v-slot:prepend>
                  <v-icon
                    icon="mdi-check"
                    color="success"
                    size="small"
                  ></v-icon>
                </template>
                <v-list-item-title>TypeScript Support</v-list-item-title>
              </v-list-item>

              <v-list-item>
                <template v-slot:prepend>
                  <v-icon
                    icon="mdi-check"
                    color="success"
                    size="small"
                  ></v-icon>
                </template>
                <v-list-item-title>Vuetify 3 Material Design</v-list-item-title>
              </v-list-item>
            </v-list>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row> -->

    <v-row>
      <!-- Security Card -->
      <v-col cols="12" md="4">
        <v-card height="100%">
          <v-card-title>
            <v-icon icon="mdi-shield-lock" class="mr-2"></v-icon>
            Advanced Security
          </v-card-title>
          <v-card-text>
            <p>Enterprise-grade security features:</p>
            <v-list density="compact">
              <v-list-item>
                <template v-slot:prepend>
                  <v-icon
                    icon="mdi-check"
                    color="success"
                    size="small"
                  ></v-icon>
                </template>
                <v-list-item-title>HTTP-Only Cookie JWT Auth</v-list-item-title>
                <v-tooltip activator="parent" location="end">
                  Secure authentication using HTTP-only cookies preventing XSS
                  attacks
                </v-tooltip>
              </v-list-item>

              <v-list-item>
                <template v-slot:prepend>
                  <v-icon
                    icon="mdi-check"
                    color="success"
                    size="small"
                  ></v-icon>
                </template>
                <v-list-item-title>CSRF Protection</v-list-item-title>
                <v-tooltip activator="parent" location="end">
                  Cross-Site Request Forgery protection via secure tokens
                </v-tooltip>
              </v-list-item>

              <v-list-item>
                <template v-slot:prepend>
                  <v-icon
                    icon="mdi-check"
                    color="success"
                    size="small"
                  ></v-icon>
                </template>
                <v-list-item-title>Content Security Policy</v-list-item-title>
                <v-tooltip activator="parent" location="end">
                  Prevents XSS and data injection attacks with strict CSP rules
                </v-tooltip>
              </v-list-item>

              <v-list-item>
                <template v-slot:prepend>
                  <v-icon
                    icon="mdi-check"
                    color="success"
                    size="small"
                  ></v-icon>
                </template>
                <v-list-item-title
                  >Rate Limiting & API Protection</v-list-item-title
                >
                <v-tooltip activator="parent" location="end">
                  Guards against brute force and DoS attacks
                </v-tooltip>
              </v-list-item>
            </v-list>
          </v-card-text>
        </v-card>
      </v-col>

      <!-- Authorization Card -->
      <v-col cols="12" md="4">
        <v-card height="100%">
          <v-card-title>
            <v-icon icon="mdi-account-key" class="mr-2"></v-icon>
            Authorization
          </v-card-title>
          <v-card-text>
            <p>Role-based access control with:</p>
            <v-list density="compact">
              <v-list-item>
                <template v-slot:prepend>
                  <v-icon
                    icon="mdi-check"
                    color="success"
                    size="small"
                  ></v-icon>
                </template>
                <v-list-item-title
                  >Meta-Driven Route Protection</v-list-item-title
                >
                <v-tooltip activator="parent" location="end">
                  Declarative route protection using page meta properties
                </v-tooltip>
              </v-list-item>

              <v-list-item>
                <template v-slot:prepend>
                  <v-icon
                    icon="mdi-check"
                    color="success"
                    size="small"
                  ></v-icon>
                </template>
                <v-list-item-title>Tiered Access System</v-list-item-title>
                <v-tooltip activator="parent" location="end">
                  Basic, Premium, and Admin role levels with hierarchical
                  permissions
                </v-tooltip>
              </v-list-item>

              <v-list-item>
                <template v-slot:prepend>
                  <v-icon
                    icon="mdi-check"
                    color="success"
                    size="small"
                  ></v-icon>
                </template>
                <v-list-item-title>Backend JWT Validation</v-list-item-title>
                <v-tooltip activator="parent" location="end">
                  .NET Core performs robust token validation with role-based
                  policies
                </v-tooltip>
              </v-list-item>

              <v-list-item>
                <template v-slot:prepend>
                  <v-icon
                    icon="mdi-check"
                    color="success"
                    size="small"
                  ></v-icon>
                </template>
                <v-list-item-title
                  >Seamless SSR Compatibility</v-list-item-title
                >
                <v-tooltip activator="parent" location="end">
                  Auth works perfectly in both server and client contexts
                </v-tooltip>
              </v-list-item>
            </v-list>
          </v-card-text>
        </v-card>
      </v-col>

      <!-- Form Validation & UX Card -->
      <v-col cols="12" md="4">
        <v-card height="100%">
          <v-card-title>
            <v-icon icon="mdi-form-textbox" class="mr-2"></v-icon>
            Enhanced User Experience
          </v-card-title>
          <v-card-text>
            <p>Superior form handling and validation:</p>
            <v-list density="compact">
              <v-list-item>
                <template v-slot:prepend>
                  <v-icon
                    icon="mdi-check"
                    color="success"
                    size="small"
                  ></v-icon>
                </template>
                <v-list-item-title>Schema-Based Validation</v-list-item-title>
                <v-tooltip activator="parent" location="end">
                  Type-safe form validation with Zod schemas
                </v-tooltip>
              </v-list-item>

              <v-list-item>
                <template v-slot:prepend>
                  <v-icon
                    icon="mdi-check"
                    color="success"
                    size="small"
                  ></v-icon>
                </template>
                <v-list-item-title>Real-Time Feedback</v-list-item-title>
                <v-tooltip activator="parent" location="end">
                  Instant validation with clear error messages
                </v-tooltip>
              </v-list-item>

              <v-list-item>
                <template v-slot:prepend>
                  <v-icon
                    icon="mdi-check"
                    color="success"
                    size="small"
                  ></v-icon>
                </template>
                <v-list-item-title>Complex Password Rules</v-list-item-title>
                <v-tooltip activator="parent" location="end">
                  Visual password strength indicators with requirement checks
                </v-tooltip>
              </v-list-item>

              <v-list-item>
                <template v-slot:prepend>
                  <v-icon
                    icon="mdi-check"
                    color="success"
                    size="small"
                  ></v-icon>
                </template>
                <v-list-item-title>Adaptive UI Components</v-list-item-title>
                <v-tooltip activator="parent" location="end">
                  UI adapts to your role with permission-based visibility
                </v-tooltip>
              </v-list-item>
            </v-list>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>

    <!-- Authentication Flow Card - Full Width -->
    <v-card class="mt-6">
      <v-card-title>
        <v-icon icon="mdi-transit-connection-variant" class="mr-2"></v-icon>
        Secure Authentication Flow
      </v-card-title>
      <v-card-text>
        <p class="mb-4">
          This template showcases a modern, secure authentication architecture:
        </p>

        <v-row>
          <v-col cols="12" md="6">
            <v-card variant="outlined">
              <v-card-title class="text-subtitle-1">
                HTTP-Only Cookie JWT Authentication
              </v-card-title>
              <v-card-text>
                <p>
                  Unlike traditional JWT implementations that use localStorage
                  (vulnerable to XSS), this template uses HTTP-only cookies to
                  store authentication tokens securely. The .NET Core backend
                  handles token generation, validation, and refresh with no
                  client-side access to tokens.
                </p>
                <p class="mt-2">
                  <strong>Benefits:</strong> Immune to JavaScript-based theft,
                  automatic CSRF protection when used with SameSite policy, and
                  seamless token refresh without disrupting the user experience.
                </p>
              </v-card-text>
            </v-card>
          </v-col>

          <v-col cols="12" md="6">
            <v-card variant="outlined">
              <v-card-title class="text-subtitle-1">
                Multi-Tiered Role Authorization
              </v-card-title>
              <v-card-text>
                <p>
                  The authentication system implements a sophisticated
                  role-based access control model with hierarchical permissions.
                  Users can hold Basic, Premium, or Admin roles with cascading
                  access levels.
                </p>
                <p class="mt-2">
                  <strong>Implementation:</strong> Roles are encoded in JWT
                  claims and validated on both frontend (for UI adapting) and
                  backend (for secure API access). Middleware ensures users can
                  only access appropriate routes and endpoints.
                </p>
              </v-card-text>
            </v-card>
          </v-col>
        </v-row>
      </v-card-text>
    </v-card>

    <!-- Technical Architecture Card -->
    <v-card class="mt-6">
      <v-card-title>
        <v-icon icon="mdi-puzzle" class="mr-2"></v-icon>
        Technical Architecture
      </v-card-title>
      <v-card-text>
        <v-row>
          <v-col cols="12" md="4">
            <v-card variant="outlined">
              <v-card-title class="text-subtitle-1">
                <v-icon icon="mdi-nuxt" class="mr-2" size="small"></v-icon>
                Frontend Stack
              </v-card-title>
              <v-card-text>
                <v-list density="compact">
                  <v-list-item>
                    <v-list-item-title
                      >Nuxt 3 with SSR & Hydration</v-list-item-title
                    >
                  </v-list-item>
                  <v-list-item>
                    <v-list-item-title
                      >Pinia Store for State Management</v-list-item-title
                    >
                  </v-list-item>
                  <v-list-item>
                    <v-list-item-title
                      >Vuetify 3 UI Components</v-list-item-title
                    >
                  </v-list-item>
                  <v-list-item>
                    <v-list-item-title
                      >VeeValidate Form Validation</v-list-item-title
                    >
                  </v-list-item>
                  <v-list-item>
                    <v-list-item-title
                      >TypeScript for Type Safety</v-list-item-title
                    >
                  </v-list-item>
                </v-list>
              </v-card-text>
            </v-card>
          </v-col>

          <v-col cols="12" md="4">
            <v-card variant="outlined">
              <v-card-title class="text-subtitle-1">
                <v-icon icon="mdi-microsoft" class="mr-2" size="small"></v-icon>
                Backend Stack
              </v-card-title>
              <v-card-text>
                <v-list density="compact">
                  <v-list-item>
                    <v-list-item-title
                      >.NET Core 7+ API with JWT Auth</v-list-item-title
                    >
                  </v-list-item>
                  <v-list-item>
                    <v-list-item-title>Entity Framework Core</v-list-item-title>
                  </v-list-item>
                  <v-list-item>
                    <v-list-item-title
                      >Identity Framework Integration</v-list-item-title
                    >
                  </v-list-item>
                  <v-list-item>
                    <v-list-item-title
                      >OAuth External Providers</v-list-item-title
                    >
                  </v-list-item>
                  <v-list-item>
                    <v-list-item-title
                      >Role-Based API Policies</v-list-item-title
                    >
                  </v-list-item>
                </v-list>
              </v-card-text>
            </v-card>
          </v-col>

          <v-col cols="12" md="4">
            <v-card variant="outlined">
              <v-card-title class="text-subtitle-1">
                <v-icon
                  icon="mdi-shield-check"
                  class="mr-2"
                  size="small"
                ></v-icon>
                Security Features
              </v-card-title>
              <v-card-text>
                <v-list density="compact">
                  <v-list-item>
                    <v-list-item-title
                      >nuxt-security Integration</v-list-item-title
                    >
                  </v-list-item>
                  <v-list-item>
                    <v-list-item-title
                      >Content Security Policy</v-list-item-title
                    >
                  </v-list-item>
                  <v-list-item>
                    <v-list-item-title>CSRF Protection</v-list-item-title>
                  </v-list-item>
                  <v-list-item>
                    <v-list-item-title
                      >Rate Limiting & DoS Protection</v-list-item-title
                    >
                  </v-list-item>
                  <v-list-item>
                    <v-list-item-title
                      >OAuth 2.0 & OpenID Connect</v-list-item-title
                    >
                  </v-list-item>
                </v-list>
              </v-card-text>
            </v-card>
          </v-col>
        </v-row>
      </v-card-text>
    </v-card>

    <!-- Authentication-dependent content - wrap in client-only -->
    <client-only>
      <!-- Authentication Status Section (displays only when logged in) -->
      <v-card v-if="auth.isAuthenticated.value" class="mt-6">
        <v-card-title>
          <v-icon
            icon="mdi-account-check"
            class="mr-2"
            color="success"
          ></v-icon>
          Authentication Status
        </v-card-title>
        <v-card-text>
          <p class="mb-2">
            You are currently logged in as
            <strong>{{ auth.user.value?.email }}</strong>
          </p>

          <v-chip-group>
            <v-chip
              v-for="role in auth.user.value?.roles"
              :key="role"
              color="primary"
              class="mr-2"
            >
              {{ role }}
            </v-chip>
          </v-chip-group>

          <div class="mt-4">
            <v-btn
              to="/dashboard"
              color="primary"
              prepend-icon="mdi-view-dashboard"
              class="mr-2"
            >
              Go to Dashboard
            </v-btn>

            <v-btn
              to="/profile"
              variant="outlined"
              color="primary"
              prepend-icon="mdi-account"
            >
              View Profile
            </v-btn>
          </div>
        </v-card-text>
      </v-card>

      <!-- Premium Content Preview (only for logged-in Premium or Admin users) -->
      <v-card v-if="isPremiumUser" class="mt-6" color="success" theme="dark">
        <v-card-title>
          <v-icon icon="mdi-star" class="mr-2"></v-icon>
          Premium Content Available
        </v-card-title>
        <v-card-text>
          <p>
            You have access to premium features with your current role. Premium
            content is only visible to users with Premium or Admin roles.
          </p>

          <v-btn
            to="/dashboard"
            color="white"
            variant="outlined"
            class="mt-2"
            prepend-icon="mdi-star"
          >
            View Premium Content
          </v-btn>
        </v-card-text>
      </v-card>

      <!-- Admin Preview (only for Admin users) -->
      <v-card v-if="isAdmin" class="mt-6" color="error" theme="dark">
        <v-card-title>
          <v-icon icon="mdi-shield-crown" class="mr-2"></v-icon>
          Administrator Access
        </v-card-title>
        <v-card-text>
          <p>
            You have administrator privileges. This section is only visible to
            users with the Admin role.
          </p>

          <v-btn
            to="/dashboard"
            color="white"
            variant="outlined"
            class="mt-2"
            prepend-icon="mdi-shield-crown"
          >
            View Admin Features
          </v-btn>
        </v-card-text>
      </v-card>

      <!-- Login/Register CTA (only when not logged in) -->
      <v-card
        v-if="!auth.isAuthenticated.value"
        class="mt-6"
        color="primary"
        theme="dark"
      >
        <v-card-title>Get Started</v-card-title>
        <v-card-text>
          <p class="mb-4">
            To explore the full template features, you'll need to be logged in.
            You can register a new account or log in with existing credentials.
          </p>

          <div class="d-flex flex-wrap">
            <v-btn
              to="/login"
              color="white"
              variant="outlined"
              class="mr-4 mb-2"
              prepend-icon="mdi-login"
            >
              Login
            </v-btn>

            <v-btn
              to="/register"
              color="white"
              class="mb-2"
              prepend-icon="mdi-account-plus"
            >
              Register
            </v-btn>
          </div>
        </v-card-text>
      </v-card>
    </client-only>
  </div>
</template>

<script setup lang="ts">
import { computed } from "vue";
import { useAuth } from "~/composables/useAuth";
import { definePageMeta } from "#imports";

definePageMeta({
  public: true,
  title: "Home",
});

const auth = useAuth();

// Check user roles - use computed properties directly from auth
const isPremiumUser = computed(() => auth.isPremiumUser.value);
const isAdmin = computed(() => auth.isAdmin.value);
</script>
