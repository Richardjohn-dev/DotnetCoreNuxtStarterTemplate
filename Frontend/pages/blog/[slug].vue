//pages/blog/[slug].vue
<template>
  <v-container>
    <v-row>
      <v-col cols="12">
        <v-card class="pa-4">
          <v-card-title class="text-h4">{{ post?.title }}</v-card-title>
          <v-card-subtitle class="mb-4 text-body-2"
            >Published on: {{ post?.date }}</v-card-subtitle
          >
          <v-card-text class="text-body-1">
            <div v-html="post?.content"></div>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup lang="ts">
import { useRoute } from "#app";
import { useAsyncData, useHead, definePageMeta } from "#imports"; // Ensure definePageMeta is imported

type BlogPost = {
  title: string;
  date: string;
  content: string;
};
// Mark this page as public
definePageMeta({
  public: true, // <--- Add this line
});

const route = useRoute();
const postSlug = route.params.slug;

// In a real application, you would fetch blog post data here
// using useAsyncData or useFetch based on the postSlug.
// This is a placeholder for demonstration purposes.
const { data: post } = useAsyncData(`blog-post-${postSlug}`, () => {
  // Simulate fetching data from an API or markdown file
  // This function runs on the server during SSR and on the client
  return new Promise<BlogPost>((resolve) => {
    setTimeout(() => {
      resolve({
        title: `Sample Blog Post: ${postSlug}`,
        date: "2023-10-27",
        content: `<p>This is the content of the blog post with slug <strong>${postSlug}</strong>.</p><p>This content is rendered on the server.</p>`,
      });
    }, 50); // Simulate network delay
  });
});

// Set page title based on the blog post title
useHead({
  title: post.value ? `${post.value.title} - My App` : "Blog Post - My App",
});
</script>

<style scoped>
/* No custom styles needed, relying on Vuetify utilities */
</style>
