// pages/protected-example.vue
<template>
  <v-container>
    <v-row class="mb-6">
      <v-col cols="12">
        <v-card class="pa-4">
          <v-card-title class="text-h4">Protected Page</v-card-title>
          <v-card-subtitle>
            This page demonstrates role-based access control
          </v-card-subtitle>
        </v-card>
      </v-col>
    </v-row>

    <!-- Auth-dependent content wrapped in client-only -->
    <client-only>
      <v-row>
        <v-col cols="12">
          <v-card class="pa-4">
            <v-card-title class="text-h6">Authentication Status</v-card-title>
            <v-card-text>
              <v-alert
                color="success"
                icon="mdi-check-circle"
                variant="tonal"
                border="start"
              >
                <strong>Authenticated:</strong> You are successfully logged in
                as
                {{ auth.user.value?.email }}
              </v-alert>

              <v-divider class="my-4"></v-divider>

              <h3 class="text-h6 mb-2">Your Current Roles:</h3>
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
            </v-card-text>
          </v-card>
        </v-col>
      </v-row>

      <v-row class="mt-6">
        <v-col cols="12">
          <v-card class="pa-4">
            <v-card-title class="text-h6">Role-Based Content Test</v-card-title>
            <v-card-text>
              <p class="mb-4">
                This card demonstrates how content can be conditionally rendered
                based on user roles. Try logging in with different account types
                to see what content is visible.
              </p>

              <v-divider class="my-4"></v-divider>

              <h3 class="text-h6 mb-2">Content Visibility Test:</h3>

              <v-list>
                <v-list-item
                  prepend-icon="mdi-account"
                  title="Basic User Content"
                  subtitle="All authenticated users can see this"
                ></v-list-item>

                <v-list-item
                  v-if="isPremiumUser"
                  prepend-icon="mdi-star"
                  title="Premium User Content"
                  subtitle="Only Premium Users and Admins can see this"
                  color="success"
                ></v-list-item>

                <v-list-item
                  v-if="auth.isAdmin.value"
                  prepend-icon="mdi-shield-crown"
                  title="Admin Content"
                  subtitle="Only Admins can see this"
                  color="error"
                ></v-list-item>
              </v-list>
            </v-card-text>
          </v-card>
        </v-col>
      </v-row>

      <v-row class="mt-6">
        <v-col cols="12" md="6">
          <v-card height="100%">
            <v-card-title class="text-h6">API Access Test</v-card-title>
            <v-card-text>
              <p class="mb-4">
                Test your access to different API endpoints based on your role.
              </p>

              <v-btn color="primary" class="mr-2 mb-2" @click="fetchBasicData">
                Basic Endpoint
              </v-btn>

              <v-btn
                color="success"
                class="mr-2 mb-2"
                @click="fetchPremiumData"
              >
                Premium Endpoint
              </v-btn>

              <v-btn color="error" class="mb-2" @click="fetchAdminData">
                Admin Endpoint
              </v-btn>

              <v-alert
                v-if="apiResult"
                :color="apiResultSuccess ? 'success' : 'error'"
                :icon="
                  apiResultSuccess ? 'mdi-check-circle' : 'mdi-alert-circle'
                "
                variant="tonal"
                class="mt-4"
              >
                <strong>{{ apiResultSuccess ? "Success" : "Error" }}:</strong>
                {{ apiResult }}
              </v-alert>
            </v-card-text>
          </v-card>
        </v-col>

        <v-col cols="12" md="6">
          <v-card height="100%">
            <v-card-title class="text-h6">Protected Routes</v-card-title>
            <v-card-text>
              <p class="mb-4">Test navigation to role-protected routes:</p>

              <v-list>
                <v-list-item
                  prepend-icon="mdi-view-dashboard"
                  title="Dashboard"
                  subtitle="Requires authentication"
                  append-icon="mdi-arrow-right"
                  link
                  to="/dashboard"
                ></v-list-item>

                <v-list-item
                  prepend-icon="mdi-shield-crown"
                  title="Admin Dashboard"
                  subtitle="Requires Admin role"
                  append-icon="mdi-arrow-right"
                  link
                  to="/admin"
                ></v-list-item>
              </v-list>
            </v-card-text>
          </v-card>
        </v-col>
      </v-row>
    </client-only>
  </v-container>
</template>

<script setup lang="ts">
import { useAuth } from "~/composables/useAuth";
import { useApi } from "~/composables/useApiFetch";
import { definePageMeta } from "#imports";

definePageMeta({
  requiresAuth: true,
  title: "Protected Example",
});

const auth = useAuth();
const api = useApi();

const isPremiumUser = computed(() => auth.isPremiumUser.value);

const apiResult = ref("");
const apiResultSuccess = ref(true);

// API access test functions
const fetchBasicData = async () => {
  try {
    const response = await api.get("/api/basic");
    apiResult.value = `Successfully accessed basic endpoint: ${JSON.stringify(
      response.payload
    )}`;
    apiResultSuccess.value = true;
  } catch (error) {
    apiResult.value =
      error instanceof Error
        ? error.message
        : "Failed to access basic endpoint";
    apiResultSuccess.value = false;
  }
};

const fetchPremiumData = async () => {
  try {
    const response = await api.get("/api/premium");
    apiResult.value = `Successfully accessed premium endpoint: ${JSON.stringify(
      response.payload
    )}`;
    apiResultSuccess.value = true;
  } catch (error) {
    apiResult.value =
      error instanceof Error
        ? error.message
        : "Failed to access premium endpoint";
    apiResultSuccess.value = false;
  }
};

const fetchAdminData = async () => {
  try {
    const response = await api.get("/api/admin");
    apiResult.value = `Successfully accessed admin endpoint: ${JSON.stringify(
      response.payload
    )}`;
    apiResultSuccess.value = true;
  } catch (error) {
    apiResult.value =
      error instanceof Error
        ? error.message
        : "Failed to access admin endpoint";
    apiResultSuccess.value = false;
  }
};
</script>
