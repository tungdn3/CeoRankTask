<template>
  <q-page class="row q-pa-md rounded-borders">
    <div class="column">
      <div class="text-h4 text-primary q-mb-md">Watch List</div>
      <q-list bordered separator>
        <q-item
          v-for="item in items"
          :key="item.id"
          :focused="item.id === selectedItem"
          clickable
          v-ripple
          @click="() => (selectedItem = item.id)"
        >
          <q-item-section>
            <q-item-label>{{ item.keyword }}</q-item-label>
            <q-item-label caption>{{ item.url }}</q-item-label>
          </q-item-section>
        </q-item>
      </q-list>
    </div>
    <div class="q-ml-sm">
      <HistoricalRankChart :watch-list-item-id="selectedItem" />
    </div>
  </q-page>
</template>

<script setup lang="ts">
import HistoricalRankChart from 'components/HistoricalRankChart.vue';
import { api } from 'src/boot/axios';
import { IPageResult } from 'src/interfaces/Common';
import { onMounted, ref } from 'vue';

defineOptions({
  name: 'WatchlistPage',
});

interface WatchListItem {
  id: number;
  keyword: string;
  url: string;
}

const selectedItem = ref<number>();
const items = ref<WatchListItem[]>([]);

onMounted(async () => {
  const response = await api.get<IPageResult<WatchListItem>>('watch-list');
  items.value = response.data.items;
  selectedItem.value = items.value[0]?.id;
});
</script>
