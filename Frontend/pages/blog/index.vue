// pages/blog/index.vue
<template>
  <v-container>
    <v-row>
      <v-col cols="12">
        <v-card class="pa-4">
          <v-card-title class="text-h4">Blog Posts</v-card-title>
          <v-card-text>
            <p class="text-body-1 mb-4">Read our latest articles.</p>

            <v-list>
              <v-list-item to="/blog/sample-blog-post" link>
                <template v-slot:prepend>
                  <v-icon>mdi-article</v-icon>
                </template>
                <v-list-item-title class="text-primary"
                  >Sample Blog Post: sample-blog-post</v-list-item-title
                >
                <template v-slot:append>
                  <v-icon>mdi-chevron-right</v-icon>
                </template>
              </v-list-item>

              <v-list-item disabled>
                <template v-slot:prepend>
                  <v-icon>mdi-article-outline</v-icon>
                </template>
                <v-list-item-title>Another Placeholder Post</v-list-item-title>
              </v-list-item>
            </v-list>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup lang="ts">
import { definePageMeta } from "#imports"; // Ensure definePageMeta is imported

definePageMeta({
  public: true, // <--- Add this line
  title: "Blog Index", // Set a specific title for this page
});
</script>

<style scoped>
/* Add any specific styling for the blog index page here */
</style>
