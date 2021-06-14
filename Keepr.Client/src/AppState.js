import { reactive } from 'vue'

// NOTE AppState is a reactive object to contain app level data
export const AppState = reactive({
  user: {},
  account: {},
  keeps: [],
  vaults: [],
  accountVaults: [],
  accountKeeps: [],
  activeProfile: null,
  activeVault: null,
  activeKeep: null,
  activeVaultKeep: null
})
