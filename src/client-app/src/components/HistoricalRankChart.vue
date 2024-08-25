<template>
  <apexchart
    width="500"
    type="line"
    :options="chartOptions"
    :series="series"
  ></apexchart>
</template>

<script setup lang="ts">
import { api } from 'src/boot/axios';
import { ref, watch } from 'vue';
import VueApexCharts from 'vue3-apexcharts';

defineOptions({
  name: 'HistoricalRankChart',
  components: {
    apexchart: VueApexCharts,
  },
});

interface Props {
  watchListItemId: number | undefined;
}

interface HistoricalRank {
  rank: number;
  checkedAt: Date;
}

const props = withDefaults(defineProps<Props>(), {});

watch(
  () => props.watchListItemId,
  async (val) => {
    if (!val) {
      series.value[0].data = [];
    } else {
      const response = await api.get<HistoricalRank[]>(
        `watch-list/${val}/historical-ranks`
      );
      series.value[0].data = response.data.map((item) => ({
        x: formatDateLabel(item.checkedAt),
        y: item.rank,
      }));
    }
  }
);

const chartOptions = ref({
  chart: {
    id: 'historical-rank',
  },
  xaxis: {
    type: 'datetime',
  },
  yaxis: {
    reversed: true,
  },
});

const series = ref([
  {
    name: 'Rank',
    data: [{ x: '', y: 0 }],
  },
]);

const formatter = new Intl.DateTimeFormat('en-US', { dateStyle: 'short' });
function formatDateLabel(date: Date) {
  const val = formatter.format(new Date(date));
  return val;
}
</script>
