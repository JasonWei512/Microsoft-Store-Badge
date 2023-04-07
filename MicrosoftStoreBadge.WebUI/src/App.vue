<script setup lang="ts">
import BadgeGenerator from "./components/BadgeGenerator.vue";
import { NMessageProvider } from "naive-ui";
</script>

<template>
  <n-message-provider placement="top-right">
    <BadgeGenerator />
  </n-message-provider>
</template>

<style scoped></style>
