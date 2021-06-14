<template>
  <div v-if="state.loading === true">
    <h1>Loading...</h1>
  </div>
  <div class="container-fluid">
    <div class="row justify-content-center">
      <div class="col-12">
        <div class="card-columns mt-3">
          <Keep v-for="keep in state.keeps" :key="keep.id" :keep="keep" />
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { computed, onMounted, reactive } from 'vue'
import Notification from '../utils/Notification'
import { AppState } from '../AppState'
import { keepsService } from '../services/KeepsService'
// import { vaultsService } from '../services/VaultsService'
export default {
  name: 'Home',
  setup() {
    const state = reactive({
      vaults: computed(() => AppState.vaults),
      keeps: computed(() => AppState.keeps),
      account: computed(() => AppState.account),
      loading: true
    })

    onMounted(async() => {
      try {
        await keepsService.getAll()
        // await vaultsService.getVaultsByAccountId(state.account.id)/
        state.loading = false
      } catch (error) {
        Notification.toast('Error: ' + error, 'error')
      }
    })
    return {
      state
    }
  }
}
</script>

<style scoped lang="scss">
    .card-columns {
        column-count: 5;
    }
@media(max-width: 900px){
      .card-columns {
        column-count: 3;
    }

}
@media(max-width: 574px){
      .card-columns {
        column-count: 2;
    }

}
</style>
