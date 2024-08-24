<template>
  <div class="q-pa-md" style="width: 100%">
    <div class="text-h4 text-primary q-mb-lg">Check CEO Rank</div>

    <q-form @submit="onSubmit" @reset="onReset" class="q-gutter-sm">
      <q-input
        filled
        v-model="keyword"
        label="Keyword *"
        lazy-rules
        :rules="[(val) => (val && val.length > 0) || 'Please type something']"
      />

      <q-input
        filled
        v-model="url"
        label="Your website *"
        lazy-rules
        :rules="[
          (val) => (val && val.length > 0) || 'Please type something',
          (val) => val.match(urlRegex) || 'Please provide a valid URL',
        ]"
      />

      <div class="">
        <q-btn
          label="Submit"
          type="submit"
          color="primary"
          :disable="isLoading"
        />
        <q-btn
          label="Reset"
          type="reset"
          color="primary"
          flat
          class="q-ml-sm"
          :disable="isLoading"
        />
      </div>
    </q-form>
    <div class="q-mt-lg">
      <div class="text-secondary q-mb-sm text-h5">Your Ranks</div>
      <div class="row items-center q-pa-sm bg-grey-3" style="height: 3rem">
        <q-spinner v-if="isLoading" color="primary" size="2em" />
        {{ result }}
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { isAxiosError } from 'axios';
import { useQuasar } from 'quasar';
import { api } from 'src/boot/axios';
import { ref } from 'vue';

const $q = useQuasar();

const keyword = ref<string>('land registry search');
const url = ref<string>('www.infotrack.co.uk');
const isLoading = ref<boolean>(false);
const result = ref<string>('');
const urlRegex =
  /^(http:\/\/|https:\/\/)?([a-zA-Z0-9-_]+\.)*[a-zA-Z0-9][a-zA-Z0-9-_]+(\.[a-zA-Z]{2,11})/g;

async function onSubmit() {
  isLoading.value = true;
  result.value = '';
  try {
    const response = await api.post<number[]>('seo-ranks/check', {
      keyword: keyword.value,
      url: url.value,
    });
    result.value = response.data.length > 0 ? response.data.join(', ') : '0';
  } catch (error) {
    console.error(error);
    if (
      isAxiosError(error) &&
      error.response &&
      error.response.status === 400
    ) {
      $q.notify({
        color: 'red-5',
        textColor: 'white',
        icon: 'warning',
        message: 'Please check your website URL',
      });
    } else {
      $q.notify({
        color: 'red-5',
        textColor: 'white',
        icon: 'warning',
        message: 'Something went wrong. Please try again later',
      });
    }
  } finally {
    isLoading.value = false;
  }
}

function onReset() {
  keyword.value = '';
  url.value = '';
  result.value = '';
}
</script>
