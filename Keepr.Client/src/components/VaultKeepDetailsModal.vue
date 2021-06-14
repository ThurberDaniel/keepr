<template>
  <div class="modal"
       id="vaultKeepDetails"
       tabindex="-1"
       aria-labelledby="exampleModalLabel"
       aria-hidden="true"
       v-if="state.keep"
  >
    <div class="modal-dialog modal-xl">
      <div class="modal-content">
        <div class="row">
          <div class="col-6">
            <img :src="state.keep.img" alt="keep image" class="img-fluid w-100">
          </div>
          <div class="col-6">
            <div class="row">
              <div class="col">
                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                  <span aria-hidden="true">&times;</span>
                </button>
              </div>
            </div>
            <div class="row ">
              <div class="col d-flex justify-content-center align-items-baseline">
                <i class="fas fa-eye"></i><span class="ml-2 mr-4 number-details">{{ state.keep.views }}</span>
                <i class="fab fa-kaggle"></i><span class="ml-2 mr-4 number-details">{{ state.keep.keeps }}</span>
                <i class="fas fa-share-alt"></i><span class="ml-2 mr-4 number-details">{{ state.keep.shares }}</span>
              </div>
            </div>
            <div class="row mt-4">
              <div class="col d-flex justify-content-center">
                <h2>{{ state.keep.name }}</h2>
              </div>
            </div>
            <div class="row">
              <div class="col d-flex justify-content-center mt-4">
                <p>{{ state.keep.description }}</p>
              </div>
            </div>
          </div>
        </div>
        <div class="modal-footer d-flex justify-content-between">
          <button type="button" class="btn btn-secondary" data-dismiss="modal" @click="deleteKeep">
            Remove Keep
          </button>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { computed, reactive } from 'vue'
import { AppState } from '../AppState'
import { keepsService } from '../services/KeepsService'
import Notification from '../utils/Notification'
import { useRouter } from 'vue-router'
import Swal from 'sweetalert2'
// import { vaultsService } from '../services/VaultsService'
// import $ from 'jquery'
export default {
  name: 'VaultKeepDetailsModal',
  setup() {
    const router = useRouter()
    const state = reactive({
      profile: computed(() => AppState.activeProfile),
      keep: computed(() => AppState.activeKeep),
      vaultKeep: computed(() => AppState.activeVaultKeep),
      accountVaults: computed(() => AppState.vaults),
      newVaultKeep: {}
    })
    return {
      state,
      router,
      async deleteKeep() {
        try {
          await Swal.fire({
            title: 'Are you sure?',
            text: "You won't be able to revert this!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, delete it!'
          }).then((result) => {
            if (result.isConfirmed) {
              keepsService.removeVaultKeep(state.vaultKeep.vaultKeepId, state.vaultKeep.vaultId)
              Notification.toast('Successfully Deleted', 'success')
            }
          })
        } catch (error) {
          Notification.toast('Error: ' + error, 'warning')
        }
      }
    }
  },
  components: {}
}
</script>

<style lang="scss" scoped>
.number-details{
  font-weight: bold;
}
</style>
