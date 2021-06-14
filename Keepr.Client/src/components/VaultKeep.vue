<template>
  <div class="VaultKeep card rounded shadow">
    <img :src="keep.img"
         alt="keep image"
         class="img-fluid w-100 image rounded"
         data-toggle="modal"
         data-target="#vaultKeepDetails"
         @click="getKeep"
    >
    <div class="card-title info">
      <h4 class="keep-name mr-4">
        {{ keep.name }}
      </h4>
      <router-link :to="{ name: 'Account' }" v-if="state.account.id===keep.creatorId">
        <img :src="keep.creator.picture" alt="profile picture" class="rounded-circle small-img">
      </router-link>
      <router-link :to="{name: 'ProfileDetailsPage', params:{id:keep.creator.id}}" v-else>
        <img :src="keep.creator.picture" alt="profile picture" class="rounded-circle small-img">
      </router-link>
    </div>
    <VaultKeepDetailsModal :keep="keep" />
  </div>
</template>

<script>
import { keepsService } from '../services/KeepsService'
import $ from 'jquery'
import Notification from '../utils/Notification'
import { computed, reactive } from 'vue'
import { AppState } from '../AppState'
export default {
  name: 'VaultKeep',
  props: {
    keep: {
      type: Object,
      required: true
    }
  },
  setup(props) {
    const state = reactive({
      account: computed(() => AppState.account)
    })
    return {
      state,
      async getKeep() {
        try {
          await keepsService.getKeep(props.keep.id)
          await keepsService.getVaultKeep(props.keep.vaultKeepId)
          $('#vaultKeepDetails').modal('show')
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
.card{
max-width: 20vw;
}

.image{
  position: relative;
  // height: auto;
}

.info{
position: absolute;
bottom: .5rem;
left: .5rem;
display: flex;
justify-content: space-between;
align-items: baseline;
margin-bottom: 0;
}
.keep-name{
  font-size: 1em;
  font-weight: bold;
  color: white;
  // -webkit-text-stroke: .1px white;
}

.small-img{
  height:1.5em;
  width: 1.5em;
}
</style>
