<template>
  <div v-if="state.loading === true && !state.activeProfile">
    <h1>Loading...</h1>
  </div>
  <div class="ProfileDetailsPage container-fluid" v-else>
    <div class="row">
      <div class="col-2 d-flex">
        <img :src="state.profile.picture" alt="">
      </div>
      <div class="col-10">
        <h2>{{ state.profile.name }}</h2>
        <p>Vaults: <span>{{ state.vaults.length }}</span> </p>
        <p>Keeps: <span>{{ state.keeps.length }}</span></p>
      </div>
    </div>
    <div class="row">
      <div class="col d-flex">
        <h3>Vaults </h3>
        <button type="button"
                class="btn btn-success ml-3"
                v-if="state.account.id === route.params.id && state.createVault===false"
                @click="state.createVault=true"
        >
          Add Vault
        </button>
        <form @submit.prevent="createVault" v-if="state.createVault === true">
          <div class="form-group">
            <label for="vault name">Name</label>
            <input type="text" class="form-control" id="vault name" v-model="state.newVault.name" required>
          </div>
          <div class="form-group">
            <label for="vault description">Description</label>
            <input type="text" class="form-control" id="vault description" v-model="state.newVault.description">
          </div>
          <div class="form-group">
            <label for="vault image">Image Url</label>
            <input type="text" class="form-control" id="vault imgUrl" v-model="state.newVault.imgUrl">
          </div>
          <label>
            <input type="checkbox" id="vault privacy" v-model="state.newVault.isPrivate" />
            Private
          </label>
          <div class="modal-footer">
            <button type="button" class="btn btn-secondary" @click="state.createVault=false">
              Close
            </button>
            <button type="submit" class="btn btn-primary">
              Create
            </button>
          </div>
        </form>
      </div>
    </div>
    <div class="row" v-if="state.account.id === route.params.id">
      <Vault v-for="vault in state.accountVaults" :key="vault.id" :vault="vault" />
    </div>
    <div class="row" v-else>
      <Vault v-for="vault in state.vaults" :key="vault.id" :vault="vault" />
    </div>
    <div class="row">
      <div class="col">
        <h3>Keeps</h3>
        <button type="button"
                class="btn btn-success ml-3"
                data-toggle="modal"
                data-target="#CreateKeepModal"
                v-if="state.account.id === route.params.id && state.createKeep===false"
                @click="state.createKeep=true"
        >
          Add Keep
        </button>
        <form @submit.prevent="createKeep" v-if="state.createKeep===true">
          <div class="form-group">
            <label for="Keep name">Name</label>
            <input type="text" class="form-control" id="keep name" v-model="state.newKeep.name" required>
          </div>
          <div class="form-group">
            <label for="Keep description">Description</label>
            <input type="text" class="form-control" id="keep description" v-model="state.newKeep.description">
          </div>
          <div class="form-group">
            <label for="Keep image">Image Url</label>
            <input type="text" class="form-control" id="keep imgUrl" v-model="state.newKeep.img">
          </div>
          <div class="modal-footer">
            <button type="button" class="btn btn-secondary" @click="state.createKeep=false">
              Close
            </button>
            <button type="submit" class="btn btn-primary">
              Create
            </button>
          </div>
        </form>
      </div>
    </div>
    <div class="row justify-content-center">
      <div class="col-12">
        <div class="card-columns mt-3" v-if="state.account.id === route.params.id">
          <Keep v-for="keep in state.accountKeeps" :key="keep.id" :keep="keep" />
        </div>
        <div class="card-columns mt-3" v-else>
          <Keep v-for="keep in state.keeps" :key="keep.id" :keep="keep" />
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { computed, onMounted, reactive } from 'vue'
import { AppState } from '../AppState'
import { profilesService } from '../services/ProfilesService'
import Notification from '../utils/Notification'
import { useRoute } from 'vue-router'
import { keepsService } from '../services/KeepsService'
import { vaultsService } from '../services/VaultsService'

export default {
  name: 'ProfileDetailsPage',
  setup() {
    const route = useRoute()
    const state = reactive({
      loading: true,
      profile: computed(() => AppState.activeProfile),
      vaults: computed(() => AppState.vaults),
      accountVaults: computed(() => AppState.accountVaults),
      accountKeeps: computed(() => AppState.accountKeeps),
      keeps: computed(() => AppState.keeps),
      account: computed(() => AppState.account),
      newVault: {},
      newKeep: {},
      createVault: false,
      createKeep: false
    })
    onMounted(async() => {
      try {
        await profilesService.getById(route.params.id)
        await vaultsService.getVaultsByProfileId(route.params.id)
        await keepsService.getKeepsByProfileId(route.params.id)
        state.loading = false
      } catch (error) {
        Notification.toast('Error: ' + error, 'error')
      }
    })
    return {
      state,
      route,
      async createVault() {
        try {
          state.newVault.creatorId = state.account.id
          await vaultsService.create(state.newVault)
          Notification.toast('Successfully created', 'success')
          state.newVault = {}
          state.createVault = false
        } catch (error) {
          Notification.toast('Error: ' + error, 'error')
        }
      },
      async createKeep() {
        try {
          state.newKeep.creatorId = state.account.id
          await keepsService.create(state.newKeep)
          Notification.toast('Successfully created', 'success')
          state.createKeep = false
          state.newKeep = {}
        } catch (error) {
          Notification.toast('Error: ' + error, 'error')
        }
      }
    }
  },
  components: {}
}
</script>

<style lang="scss" scoped>
    .card-columns {
        column-count: 8;
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
