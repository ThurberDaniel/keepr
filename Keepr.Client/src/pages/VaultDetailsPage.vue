<template>
  <div v-if="state.loading === true && !state.vault">
    <h1>Loading...</h1>
  </div>
  <div class="VaultDetails container-fluid" v-else>
    <div class="row">
      <div class="col">
        <h1>{{ state.vault.name }}</h1>
      </div>
    </div>
    <div class="row justify-content-center">
      <div class="col-12">
        <div class="card-columns mt-3">
          <VaultKeep v-for="keep in state.keeps" :key="keep.id" :keep="keep" />
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { computed, onMounted, reactive } from 'vue'
import { AppState } from '../AppState'
// import { vaultsService } from '../services/VaultsService'
import { useRoute } from 'vue-router'
import { keepsService } from '../services/KeepsService'
import Notification from '../utils/Notification'
import { vaultsService } from '../services/VaultsService'
export default {
  name: 'VaultDetailsPage',
  setup() {
    const route = useRoute()
    const state = reactive({
      vault: computed(() => AppState.activeVault),
      keeps: computed(() => AppState.keeps),
      loading: true
    })
    onMounted(async() => {
      try {
        await vaultsService.getById(route.params.id)
        await keepsService.getKeepsByVaultId(route.params.id)
        state.loading = false
      } catch (error) {
        Notification.toast('Error: ' + error, 'error')
      }
    })
    return {
      state,
      route
    }
  },
  components: {}
}
</script>

<style lang="scss" scoped>

</style>
