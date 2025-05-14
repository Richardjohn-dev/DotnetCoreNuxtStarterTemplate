// pages/dashboard.vue
<template>
  <div>
    <!-- Basic Dashboard Card -->
    <v-card class="mb-6">
      <v-card-title>
        <v-icon icon="mdi-view-dashboard" class="mr-2"></v-icon>
        Basic Dashboard
      </v-card-title>

      <!-- Auth-dependent content in client-only -->
      <client-only>
        <v-card-text>
          <v-card-subtitle class="mb-4">
            Welcome, {{ userEmail }}</v-card-subtitle
          >

          <v-row>
            <v-col cols="12" sm="6" md="3">
              <v-card color="primary" theme="dark" class="text-center pa-4">
                <v-icon
                  icon="mdi-file-document"
                  size="x-large"
                  class="mb-2"
                ></v-icon>
                <div class="text-h4 font-weight-bold">12</div>
                <div class="text-subtitle-1">Documents</div>
              </v-card>
            </v-col>

            <v-col cols="12" sm="6" md="3">
              <v-card color="info" theme="dark" class="text-center pa-4">
                <v-icon icon="mdi-message" size="x-large" class="mb-2"></v-icon>
                <div class="text-h4 font-weight-bold">24</div>
                <div class="text-subtitle-1">Messages</div>
              </v-card>
            </v-col>

            <v-col cols="12" sm="6" md="3">
              <v-card color="success" theme="dark" class="text-center pa-4">
                <v-icon
                  icon="mdi-check-circle"
                  size="x-large"
                  class="mb-2"
                ></v-icon>
                <div class="text-h4 font-weight-bold">8</div>
                <div class="text-subtitle-1">Completed</div>
              </v-card>
            </v-col>

            <v-col cols="12" sm="6" md="3">
              <v-card color="warning" theme="dark" class="text-center pa-4">
                <v-icon
                  icon="mdi-alert-circle"
                  size="x-large"
                  class="mb-2"
                ></v-icon>
                <div class="text-h4 font-weight-bold">3</div>
                <div class="text-subtitle-1">Pending</div>
              </v-card>
            </v-col>
          </v-row>
        </v-card-text>
      </client-only>
    </v-card>

    <!-- User Info Section -->
    <client-only>
      <v-card class="mb-6">
        <v-card-title>
          <v-icon icon="mdi-account" class="mr-2"></v-icon>
          Your Account
        </v-card-title>
        <v-card-text>
          <v-row>
            <v-col cols="12" md="6">
              <div class="text-subtitle-1 font-weight-bold mb-1">Email:</div>
              <div class="mb-4">{{ userEmail }}</div>

              <div class="text-subtitle-1 font-weight-bold mb-1">User ID:</div>
              <div class="mb-4">{{ userId }}</div>
            </v-col>

            <v-col cols="12" md="6">
              <div class="text-subtitle-1 font-weight-bold mb-1">Roles:</div>
              <v-chip-group>
                <v-chip
                  v-for="role in auth.user.value?.roles"
                  :key="role"
                  color="primary"
                  variant="outlined"
                  class="mr-2"
                >
                  {{ role }}
                </v-chip>
              </v-chip-group>

              <div class="text-subtitle-1 font-weight-bold mb-1 mt-4">
                Last Login:
              </div>
              <div>{{ formatDate(new Date()) }}</div>
            </v-col>
          </v-row>

          <v-divider class="my-4"></v-divider>

          <v-btn to="/profile" color="primary" prepend-icon="mdi-account-edit">
            Edit Profile
          </v-btn>
        </v-card-text>
      </v-card>
    </client-only>

    <!-- Premium Content (visible only to premium and admin users) -->
    <client-only>
      <v-card v-if="isPremiumUser" class="mb-6" color="success" theme="dark">
        <v-card-title>
          <v-icon icon="mdi-star" class="mr-2"></v-icon>
          Premium Content
        </v-card-title>
        <v-card-text>
          <p class="mb-4">
            This section is only visible to users with Premium or Admin roles.
            You have access to exclusive premium features.
          </p>

          <v-row>
            <v-col cols="12" md="6">
              <v-card theme="light" class="mb-4">
                <v-card-title>Advanced Analytics</v-card-title>
                <v-card-text>
                  <p>
                    As a premium user, you have access to detailed analytics and
                    reporting features.
                  </p>
                  <v-btn
                    color="success"
                    class="mt-2"
                    prepend-icon="mdi-chart-line"
                  >
                    View Analytics
                  </v-btn>
                </v-card-text>
              </v-card>
            </v-col>

            <v-col cols="12" md="6">
              <v-card theme="light" class="mb-4">
                <v-card-title>Priority Support</v-card-title>
                <v-card-text>
                  <p>
                    Premium users receive priority customer support with faster
                    response times.
                  </p>
                  <v-btn
                    color="success"
                    class="mt-2"
                    prepend-icon="mdi-headset"
                  >
                    Contact Support
                  </v-btn>
                </v-card-text>
              </v-card>
            </v-col>
          </v-row>
        </v-card-text>
      </v-card>
    </client-only>

    <!-- Admin Content (visible only to admin users) -->
    <client-only>
      <v-card v-if="isAdmin" class="mb-6" color="error" theme="dark">
        <v-card-title>
          <v-icon icon="mdi-shield-crown" class="mr-2"></v-icon>
          Admin Controls
        </v-card-title>
        <v-card-text>
          <p class="mb-4">
            This section is only visible to users with the Admin role. You have
            access to system administration features.
          </p>

          <v-row>
            <v-col cols="12" md="4">
              <v-card theme="light">
                <v-card-title>System Settings</v-card-title>
                <v-card-text>
                  <p>Configure application settings and preferences.</p>
                  <v-btn color="error" class="mt-2" prepend-icon="mdi-cog">
                    Settings
                  </v-btn>
                </v-card-text>
              </v-card>
            </v-col>

            <v-col cols="12" md="4">
              <v-card theme="light">
                <v-card-title>User Management</v-card-title>
                <v-card-text>
                  <p>Manage users, roles, and permissions.</p>
                  <v-btn
                    color="error"
                    class="mt-2"
                    prepend-icon="mdi-account-group"
                  >
                    Manage Users
                  </v-btn>
                </v-card-text>
              </v-card>
            </v-col>

            <v-col cols="12" md="4">
              <v-card theme="light">
                <v-card-title>System Logs</v-card-title>
                <v-card-text>
                  <p>View application logs and system activity.</p>
                  <v-btn
                    color="error"
                    class="mt-2"
                    prepend-icon="mdi-text-box-search"
                  >
                    View Logs
                  </v-btn>
                </v-card-text>
              </v-card>
            </v-col>
          </v-row>
        </v-card-text>
      </v-card>
    </client-only>

    <!-- API Testing Section -->
    <client-only>
      <v-card class="mb-6">
        <v-card-title>
          <v-icon icon="mdi-api" class="mr-2"></v-icon>
          API Access Test
        </v-card-title>
        <v-card-text>
          <p class="mb-4">
            Test your access to different API endpoints based on your role. This
            demonstrates role-based API authorization.
          </p>

          <div class="d-flex flex-wrap gap-2 mb-4">
            <v-btn
              color="primary"
              class="mr-2 mb-2"
              @click="fetchBasicData"
              :loading="isBasicLoading"
            >
              Basic Endpoint
            </v-btn>

            <v-btn
              color="success"
              class="mr-2 mb-2"
              @click="fetchPremiumData"
              :loading="isPremiumLoading"
            >
              Premium Endpoint
            </v-btn>

            <v-btn
              color="error"
              class="mb-2"
              @click="fetchAdminData"
              :loading="isAdminLoading"
            >
              Admin Endpoint
            </v-btn>
          </div>

          <v-alert
            v-if="apiResult"
            :color="apiResultSuccess ? 'success' : 'error'"
            :icon="apiResultSuccess ? 'mdi-check-circle' : 'mdi-alert-circle'"
            variant="tonal"
            border="start"
            class="mt-4"
          >
            <strong>{{ apiResultSuccess ? "Success" : "Error" }}:</strong>
            {{ apiResult }}
          </v-alert>
        </v-card-text>
      </v-card>
    </client-only>
  </div>
</template>

<script setup lang="ts">
import { ref, computed } from "vue";
import { useAuth } from "~/composables/useAuth";
import { useApi } from "~/composables/useApiFetch";
import { definePageMeta } from "#imports";

definePageMeta({
  requiresAuth: true,
  title: "Dashboard",
});

// Store access
const auth = useAuth();
const api = useApi();

// Check premium status
const isPremiumUser = computed(() => auth.isPremiumUser.value);

// Check admin status
const isAdmin = computed(() => auth.isAdmin.value);

const userEmail = computed(() => auth.user.value?.email || "");
const userId = computed(() => auth.user.value?.userId || "");

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

// API test states
const apiResult = ref("");
const apiResultSuccess = ref(true);
const isBasicLoading = ref(false);
const isPremiumLoading = ref(false);
const isAdminLoading = ref(false);

// API access test functions
const fetchBasicData = async () => {
  isBasicLoading.value = true;
  try {
    const response = await api.get<string>("/basic");
    apiResult.value = `basic endpoint response: ${response.message}`;
    apiResultSuccess.value = true;
  } catch (error) {
    apiResult.value =
      error instanceof Error
        ? error.message
        : "Failed to access basic endpoint";
    apiResultSuccess.value = false;
  } finally {
    isBasicLoading.value = false;
  }
};

const fetchPremiumData = async () => {
  isPremiumLoading.value = true;
  try {
    const response = await api.get<string>("/premium");
    apiResult.value = `Premium endpoint response: ${response.message}`;
    apiResultSuccess.value = true;
  } catch (error) {
    apiResult.value =
      error instanceof Error
        ? error.message
        : "Failed to access premium endpoint - requires Premium or Admin role";
    apiResultSuccess.value = false;
  } finally {
    isPremiumLoading.value = false;
  }
};

const fetchAdminData = async () => {
  isAdminLoading.value = true;
  try {
    const response = await api.get<string>("/admin");
    apiResult.value = `Admin endpoint response: ${response.message}`;
    apiResultSuccess.value = true;
  } catch (error) {
    apiResult.value =
      error instanceof Error
        ? error.message
        : "Failed to access admin endpoint - requires Admin role";
    apiResultSuccess.value = false;
  } finally {
    isAdminLoading.value = false;
  }
};
</script>
