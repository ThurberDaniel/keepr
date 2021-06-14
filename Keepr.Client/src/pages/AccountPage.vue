<template>
  <div v-if="state.loading === true && !state.account">
    <h1>Loading...</h1>
  </div>
  <div class="AccountDetailsPage container-fluid" v-else>
    <div class="row mt-2">
      <div class="col-md-2 col-4 d-flex">
        <img :src="state.account.picture" alt="users image" class="img-fluid w-100 rounded-circle">
      </div>
      <div class="col-10 d-flex flex-column">
        <div>
          <h2 class="name">
            {{ state.account.name }}
          </h2>
        </div>
        <p>Vaults: <span>{{ state.vaults.length }}</span> </p>
        <p>Keeps: <span>{{ state.keeps.length }}</span></p>
      </div>
    </div>
    <div class="row">
      <div class="col d-flex">
        <h3>Vaults </h3>
        <button type="button" class="btn btn-success ml-3" v-if="state.createVault === false" @click="state.createVault=true">
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
    <div class="row" v-if="state.vaults.length > 0">
      <Vault v-for="vault in state.vaults" :key="vault.id" :vault="vault" />
    </div>
    <div class="row">
      <div class="col">
        <h3>Keeps</h3>
        <button type="button" class="btn btn-success ml-3" v-if="state.createKeep === false" @click="state.createKeep=true">
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
        <div class="card-columns mt-3">
          <Keep v-for="keep in state.keeps" :key="keep.id" :keep="keep" />
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { computed, onMounted, reactive, watchEffect } from 'vue'
import { AppState } from '../AppState'
import { profilesService } from '../services/ProfilesService'
import { vaultsService } from '../services/VaultsService'
import { keepsService } from '../services/KeepsService'
import Notification from '../utils/Notification'
export default {
  name: 'Account',
  setup() {
    const state = reactive({
      loading: true,
      profile: computed(() => AppState.activeProfile),
      vaults: computed(() => AppState.accountVaults),
      keeps: computed(() => AppState.accountKeeps),
      account: computed(() => AppState.account),
      newVault: {},
      newKeep: {},
      createVault: false,
      createKeep: false
    })
    watchEffect(async() => {
      await vaultsService.getVaultsByAccountId(state.account.id)
      await keepsService.getKeepsByProfileId(state.account.id)
    })
    onMounted(async() => {
      try {
        await profilesService.getById(state.account.id)
        await vaultsService.getVaultsByAccountId(state.account.id)
        await keepsService.getKeepsByProfileId(state.account.id)
        state.loading = false
      } catch (error) {
        Notification.toast('Error: ' + error, 'error')
      }
    })
    return {
      state,
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
button{
  margin: 2px;
  padding: 3px 10px;
  text-align: center;
  text-transform: uppercase;
  transition: 0.5s;
  background-size: 200% auto;
  color: white;
  box-shadow: 0 0 0px #eee;
  border-radius: 10px;
  display: block;
  font-size: .7em;
}
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
.name{
font-size: 1.5rem;
}
}
</style>
