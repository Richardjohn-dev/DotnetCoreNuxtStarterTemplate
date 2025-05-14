// pages/profile.vue
<template>
  <div>
    <v-row>
      <v-col cols="12">
        <v-card>
          <v-card-title class="text-h4">My Profile</v-card-title>
          <v-card-text>
            <v-alert
              color="info"
              icon="mdi-information"
              variant="tonal"
              class="mb-6"
            >
              This is a demonstration page showing user details from JWT auth.
            </v-alert>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>

    <!-- Auth-dependent profile content wrapped in client-only -->
    <client-only>
      <v-row>
        <!-- User Profile Card -->
        <v-col cols="12" md="6">
          <v-card>
            <v-card-item>
              <template v-slot:prepend>
                <v-avatar color="primary" size="80">
                  <v-icon size="48" color="white">mdi-account</v-icon>
                </v-avatar>
              </template>
              <v-card-title class="text-h5">Account Information</v-card-title>
              <v-card-subtitle>
                User details retrieved from authentication token
              </v-card-subtitle>
            </v-card-item>

            <v-card-text>
              <v-list lines="two">
                <v-list-item title="Email" :subtitle="auth.user.value?.email">
                  <template v-slot:prepend>
                    <v-icon color="primary">mdi-email</v-icon>
                  </template>
                </v-list-item>

                <v-list-item
                  title="User ID"
                  :subtitle="auth.user.value?.userId"
                >
                  <template v-slot:prepend>
                    <v-icon color="primary">mdi-identifier</v-icon>
                  </template>
                </v-list-item>

                <v-list-item title="Roles">
                  <template v-slot:prepend>
                    <v-icon color="primary">mdi-shield-account</v-icon>
                  </template>
                  <template v-slot:subtitle>
                    <v-chip-group>
                      <v-chip
                        v-for="role in auth.user.value?.roles"
                        :key="role"
                        color="primary"
                        variant="outlined"
                        size="small"
                      >
                        {{ role }}
                      </v-chip>
                    </v-chip-group>
                  </template>
                </v-list-item>

                <v-list-item
                  title="Account Created"
                  subtitle="January 15, 2025"
                >
                  <template v-slot:prepend>
                    <v-icon color="primary">mdi-calendar</v-icon>
                  </template>
                </v-list-item>

                <v-list-item
                  title="Authentication Status"
                  subtitle="Active Session"
                >
                  <template v-slot:prepend>
                    <v-icon color="success">mdi-shield-check</v-icon>
                  </template>
                </v-list-item>
              </v-list>

              <v-divider class="my-4"></v-divider>

              <div class="d-flex flex-wrap">
                <v-btn
                  color="primary"
                  prepend-icon="mdi-account-edit"
                  class="mr-2 mb-2"
                >
                  Edit Profile
                </v-btn>

                <v-btn
                  color="error"
                  prepend-icon="mdi-logout"
                  variant="outlined"
                  class="mb-2"
                  @click="auth.logout()"
                >
                  Sign Out
                </v-btn>
              </div>
            </v-card-text>
          </v-card>
        </v-col>

        <!-- Access Information and Settings -->
        <v-col cols="12" md="6">
          <v-card>
            <v-card-title class="text-h5">
              <v-icon start color="primary">mdi-security</v-icon>
              Access Information
            </v-card-title>

            <v-card-text>
              <v-list>
                <v-list-subheader>Available Resources</v-list-subheader>

                <!-- Basic resources everyone can access -->
                <v-list-item
                  title="Basic Resources"
                  subtitle="Available to all authenticated users"
                  prepend-icon="mdi-check-circle"
                  color="success"
                ></v-list-item>

                <!-- Premium resources -->
                <v-list-item
                  title="Premium Resources"
                  subtitle="Available to premium users"
                  :prepend-icon="
                    isPremiumUser ? 'mdi-check-circle' : 'mdi-lock'
                  "
                  :color="isPremiumUser ? 'success' : 'error'"
                ></v-list-item>

                <!-- Admin resources -->
                <v-list-item
                  title="Administrative Resources"
                  subtitle="Available to administrators only"
                  :prepend-icon="isAdmin ? 'mdi-check-circle' : 'mdi-lock'"
                  :color="isAdmin ? 'success' : 'error'"
                ></v-list-item>
              </v-list>

              <v-divider class="my-4"></v-divider>

              <v-expansion-panels variant="accordion">
                <v-expansion-panel title="Security Settings">
                  <v-expansion-panel-text>
                    <v-list density="compact">
                      <v-list-item>
                        <v-switch
                          label="Two-factor authentication"
                          color="primary"
                          hint="Enhance your account security"
                          persistent-hint
                        ></v-switch>
                      </v-list-item>

                      <v-list-item>
                        <v-switch
                          label="Email notifications"
                          color="primary"
                          hint="Receive alerts for account activity"
                          persistent-hint
                        ></v-switch>
                      </v-list-item>

                      <v-list-item class="mt-4">
                        <v-btn
                          color="primary"
                          variant="outlined"
                          prepend-icon="mdi-key"
                        >
                          Change Password
                        </v-btn>
                      </v-list-item>
                    </v-list>
                  </v-expansion-panel-text>
                </v-expansion-panel>

                <v-expansion-panel title="Personal Preferences">
                  <v-expansion-panel-text>
                    <v-list density="compact">
                      <v-list-item>
                        <v-switch
                          label="Dark Mode"
                          color="primary"
                          hint="Toggle between light and dark theme"
                          persistent-hint
                        ></v-switch>
                      </v-list-item>

                      <v-list-item>
                        <v-switch
                          label="Compact View"
                          color="primary"
                          hint="Use condensed layout for more content"
                          persistent-hint
                        ></v-switch>
                      </v-list-item>

                      <v-list-item class="mt-2">
                        <div class="text-body-2 mb-2">Language Preference</div>
                        <v-select
                          label="Select Language"
                          :items="['English', 'Spanish', 'French', 'German']"
                          variant="outlined"
                          density="compact"
                        ></v-select>
                      </v-list-item>
                    </v-list>
                  </v-expansion-panel-text>
                </v-expansion-panel>
              </v-expansion-panels>

              <v-card class="mt-6" variant="outlined">
                <v-card-title class="text-subtitle-1">
                  <v-icon start size="small" color="info"
                    >mdi-information</v-icon
                  >
                  Account Information
                </v-card-title>
                <v-card-text class="text-caption">
                  <p>Your account was created on January 15, 2025.</p>
                  <p>Last login: {{ formatDate(new Date()) }}</p>
                  <p>
                    Authentication method:
                    {{ isGoogleAccount ? "Google OAuth" : "Email/Password" }}
                  </p>
                </v-card-text>
              </v-card>
            </v-card-text>
          </v-card>
        </v-col>
      </v-row>
    </client-only>
  </div>
</template>

<script setup lang="ts">
import { useAuth } from "~/composables/useAuth";
import { definePageMeta } from "#imports";

definePageMeta({
  requiresAuth: true,
  title: "Profile",
});

const auth = useAuth();

const isPremiumUser = computed(() => auth.isPremiumUser.value);
const isAdmin = computed(() => auth.isAdmin.value);

const isGoogleAccount = computed(() => {
  // This is a placeholder - you would determine this from user data
  return false;
});

// Format date helper
const formatDate = (date: Date) => {
  return new Intl.DateTimeFormat("en-US", {
    year: "numeric",
    month: "short",
    day: "numeric",
    hour: "2-digit",
    minute: "2-digit",
  }).format(date);
};
</script>
