<template>
  <v-container fluid fill-height>
    <v-row align="center" justify="center">
      <v-col cols="12" sm="8" md="6">
        <v-card class="pa-4 text-center">
          <v-card-title
            class="text-h4 error--text d-flex justify-center align-center"
          >
            <v-icon large color="error" class="mr-2">mdi-alert-octagon</v-icon>
            Access Denied
          </v-card-title>
          <v-card-text class="text-body-1">
            <p>
              You do not have the necessary permissions to access this page.
            </p>
          </v-card-text>
          <v-card-actions class="justify-center">
            <v-btn color="primary" to="/">Go to Home</v-btn>
          </v-card-actions>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup lang="ts">
// Optional: Define page meta if needed, though global middleware usually handles it
</script>
