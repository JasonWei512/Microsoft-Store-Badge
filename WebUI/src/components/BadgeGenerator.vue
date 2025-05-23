<script setup lang="ts">
import { ref, type Ref } from "vue";
import {
  useMessage,
  NSpace,
  NSelect,
  NInput,
  NCode,
  NButton,
  NLayout,
  NLayoutHeader,
  NLayoutContent,
  NLayoutFooter,
  NGrid,
  NGridItem,
  NTable,
  NDivider,
  NScrollbar,
  NPopover,
} from "naive-ui";
import type { SelectMixedOption } from "naive-ui/es/select/src/interface";
import { computed } from "vue";
import urlcat from "urlcat";
import copy from "copy-to-clipboard";

const message = useMessage();

const storeLinkPreffix = "https://apps.microsoft.com/detail/";

const untrimmedStoreId: Ref<string> = ref("");
const storeId = computed(() => untrimmedStoreId.value.trim());

const market: Ref<string> = ref("US");
// prettier-ignore
const marketOptions: SelectMixedOption[] = [
  "US",

  "AD", "AE", "AF", "AG", "AI", "AL", "AM", "AO", "AQ", "AR", "AS", "AT", "AU", "AW", "AX", "AZ", 
  "BA", "BB", "BD", "BE", "BF", "BG", "BH", "BI", "BJ", "BL", "BM", "BN", "BO", "BQ", "BR", "BS", "BT", "BV", "BW", "BY", "BZ", 
  "CA", "CC", "CD", "CF", "CG", "CH", "CI", "CK", "CL", "CM", "CN", "CO", "CR", "CV", "CW", "CX", "CY", "CZ", 
  "DE", "DJ", "DK", "DM", "DO", "DZ", 
  "EC", "EE", "EG", "EH", "ER", "ES", "ET", 
  "FI", "FJ", "FK", "FM", "FO", "FR", 
  "GA", "GB", "GD", "GE", "GF", "GG", "GH", "GI", "GL", "GM", "GN", "GP", "GQ", "GR", "GS", "GT", "GU", "GW", "GY", 
  "HK", "HM", "HN", "HR", "HT", "HU", 
  "ID", "IE", "IL", "IM", "IN", "IO", "IQ", "IS", "IT", 
  "JE", "JM", "JO", "JP",
  "KE", "KG", "KH", "KI", "KM", "KN", "KR", "KW", "KY", "KZ",
  "LA", "LB", "LC", "LI", "LK", "LR", "LS", "LT", "LU", "LV", "LY",
  "MA", "MC", "MD", "ME", "MF", "MG", "MH", "MK", "ML", "MM", "MN", "MO", "MP", "MQ", "MR", "MS", "MT", "MU", "MV", "MW", "MX", "MY", "MZ",
  "NA", "NC", "NE", "NF", "NG", "NI", "NL", "NO", "NP", "NR", "NU", "NZ",
  "OM",
  "PA", "PE", "PF", "PG", "PH", "PK", "PL", "PM", "PN", "PS", "PT", "PW", "PY",
  "QA",
  "RE", "RO", "RS", "RU", "RW",
  "SA", "SB", "SC", "SE", "SG", "SH", "SI", "SJ", "SK", "SL", "SM", "SN", "SO", "SR", "ST", "SV", "SX", "SZ",
  "TC", "TD", "TF", "TG", "TH", "TJ", "TK", "TL", "TM", "TN", "TO", "TR", "TT", "TV", "TW", "TZ",
  "UA", "UG", "UM", "UY", "UZ",
  "VA", "VC", "VE", "VG", "VI", "VN", "VU",
  "WF", "WS",
  "YE", "YT",
  "ZA", "ZM", "ZW",
].map((x) => ({ value: x, label: x }));

const style: Ref<string> = ref("flat");
const styleOptions: SelectMixedOption[] = [
  {
    label: "Plastic",
    value: "plastic",
  },
  {
    label: "Flat",
    value: "flat",
  },
  {
    label: "Flat Square",
    value: "flat-square",
  },
  {
    label: "For the Badge",
    value: "for-the-badge",
  },
  {
    label: "Social",
    value: "social",
  },
];

const label: Ref<string | null> = ref(null);

const color: Ref<string> = ref("brightgreen");
const colorOptions: SelectMixedOption[] = [
  {
    label: "Bright Green",
    value: "brightgreen",
  },
  {
    label: "Green",
    value: "green",
  },
  {
    label: "Yellow Green",
    value: "yellowgreen",
  },
  {
    label: "Yellow",
    value: "yellow",
  },
  {
    label: "Orange",
    value: "orange",
  },
  {
    label: "Red",
    value: "red",
  },
  {
    label: "Blue",
    value: "blue",
  },
  {
    label: "Light Grey",
    value: "lightgrey",
  },
  {
    label: "Blue Violet",
    value: "blueviolet",
  },
];

const logo: Ref<string | null> = ref(null);

const badgeImageUrl = computed(() =>
  urlcat("https://img.shields.io", "/endpoint", {
    url: urlcat("https://microsoft-store-badge.fly.dev", "/api/rating", {
      storeId: storeId.value,
      market: market.value,
    }),
    style: style.value,
    label: label.value ? label.value : null, // If label is an empty string, exclude it from queries
    color: color.value,
    logo: logo.value,
  })
);

const storeLink = computed(() => `${storeLinkPreffix}${storeId.value}`);

const badgeMarkdown = computed(
  () => `[![${label.value ?? ""}](${badgeImageUrl.value})](${storeLink.value})`
);

function copyBadgeMarkdown() {
  copy(badgeMarkdown.value);
  message.success("Markdown copied to clipboard!");
}

const badgeHtml = computed(
  () =>
    `<a href="${storeLink.value}" target="_blank">
    <img src="${badgeImageUrl.value}" alt="${label.value ?? ""}" />
</a>
`
);

function copyBadgeHtml() {
  copy(badgeHtml.value);
  message.success("HTML copied to clipboard!");
}
</script>

<template>
  <n-space vertical size="large">
    <n-layout>
      <n-layout-header>
        <h1 class="title">Microsoft Store Badge Generator</h1>
      </n-layout-header>

      <n-layout-content>
        <n-grid x-gap="24" y-gap="24" cols="1 700:2" :item-responsive="true">
          <n-grid-item>
            <n-space vertical class="vertical-center">
              <n-table :bordered="false">
                <tbody>
                  <tr>
                    <td>
                      <n-popover trigger="hover">
                        <template #trigger> Store ID ﹖</template>
                        How to get the store ID: <br />
                        1. Open Microsoft Store and go to the app page. <br />
                        2. Click the share button and select "copy link". <br />
                        3. A url like
                        <a
                          href="https://apps.microsoft.com/detail/9NF7JTB3B17P"
                          target="_blank"
                          >https://apps.microsoft.com/detail/<span style="font-weight: bold"
                            >9NF7JTB3B17P</span
                          ></a
                        >
                        will be copied to clipboard. <br />
                        4. The last segment <span style="font-weight: bold">9NF7JTB3B17P</span> is
                        the store ID. <br />
                        <br />
                        Note that unpackaged Win32 apps whose IDs start with "XP" are not supported
                        at the moment. <br />
                        For example, you can't generate badges for
                        <a
                          href="https://apps.microsoft.com/store/detail/XP9KHM4BK9FZ7Q"
                          target="_blank"
                          >VSCode</a
                        >
                        and
                        <a
                          href="https://apps.microsoft.com/store/detail/XP89DCGQ3K6VLD"
                          target="_blank"
                          >PowerToys</a
                        >.
                      </n-popover>
                    </td>
                    <td>
                      <n-input v-model:value="untrimmedStoreId" type="text" placeholder="9NF7JTB3B17P" />
                    </td>
                  </tr>
                  <tr>
                    <td>
                      <n-popover trigger="hover">
                        <template #trigger> Market ﹖</template>
                        Change this if your app is not published in USA.
                      </n-popover>
                    </td>
                    <td>
                      <n-select
                        :options="marketOptions"
                        v-model:value="market"
                        :consistent-menu-width="false"
                        :show-checkmark="false"
                      />
                    </td>
                  </tr>
                  <tr>
                    <td>Style</td>
                    <td>
                      <n-select
                        :options="styleOptions"
                        v-model:value="style"
                        :consistent-menu-width="false"
                        :show-checkmark="false"
                      />
                    </td>
                  </tr>
                  <tr>
                    <td>Label</td>
                    <td>
                      <n-input
                        v-model:value="label"
                        type="text"
                        placeholder="Label on the left side of the badge"
                      />
                    </td>
                  </tr>
                  <tr>
                    <td>Color</td>
                    <td>
                      <n-select
                        :options="colorOptions"
                        v-model:value="color"
                        placeholder="Color"
                        :consistent-menu-width="false"
                        :show-checkmark="false"
                      />
                    </td>
                  </tr>
                  <tr>
                    <td>
                      <n-popover trigger="hover">
                        <template #trigger> Logo ﹖</template>
                        One of the named logos supported by
                        <a href="https://shields.io/" target="_blank">Shields</a> or
                        <a href="https://simpleicons.org/" target="_blank">simple-icons</a>.
                      </n-popover>
                    </td>
                    <td>
                      <n-input v-model:value="logo" type="text" placeholder="Windows" />
                    </td>
                  </tr>
                </tbody>
              </n-table>
            </n-space>
          </n-grid-item>

          <n-grid-item>
            <n-space vertical class="vertical-center">
              <h3 class="subtitle">Preview</h3>
              <a :href="storeLink" target="_blank" class="badge">
                <img :src="badgeImageUrl" alt="Badge Preview" />
              </a>

              <n-divider />

              <h3 class="subtitle">
                Markdown
                <n-button class="copy-button" size="small" round @click="copyBadgeMarkdown"
                  >Copy</n-button
                >
              </h3>
              <n-scrollbar class="codeblock-scrollbar" :x-scrollable="true">
                <n-code :code="badgeMarkdown" language="markdown" />
              </n-scrollbar>

              <n-divider />

              <h3 class="subtitle">
                HTML
                <n-button class="copy-button" size="small" round @click="copyBadgeHtml"
                  >Copy</n-button
                >
              </h3>
              <n-scrollbar :x-scrollable="true" class="codeblock-scrollbar">
                <n-code :code="badgeHtml" language="html" />
              </n-scrollbar>
            </n-space>
          </n-grid-item>
        </n-grid>
      </n-layout-content>

      <n-layout-footer style="margin-top: 2rem; background-color: transparent">
        <n-space justify="center">
          <a href="https://github.com/JasonWei512/Microsoft-Store-Badge" target="_blank">
            <img
              alt="GitHub Repo stars"
              src="https://img.shields.io/github/stars/JasonWei512/Microsoft-Store-Badge?style=social"
            />
          </a>
        </n-space>
      </n-layout-footer>
    </n-layout>
  </n-space>
</template>

<style scoped>
h1 {
  font-size: 1.5rem;
}

h3 {
  font-size: 1.2rem;
}

.title {
  font-weight: bolder;
  text-align: center;
  margin-bottom: 2rem;
}

.subtitle {
  font-weight: 400;
}

.n-divider {
  margin-top: 0.2rem;
  margin-bottom: 0.2rem;
}

.copy-button {
  margin-left: 0.5rem;
  margin-right: 0.5rem;
}

:deep(.codeblock-scrollbar) {
  background-color: rgba(0, 0, 0, 0.025);
  padding: 1rem;
  border-radius: 6px;
}

.vertical-center {
  position: relative;
  top: 50%;
  transform: translateY(-50%);
}
</style>
