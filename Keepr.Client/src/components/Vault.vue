<template>
  <div class="card col-md-3 col-6">
    <div class="card-body" @click="getVault">
      <img :src="vault.imgUrl" class="card-img-top img-fluid w-100" alt="...">
      <h5 class="card-title">
        {{ vault.name }}
      </h5>
      <p v-if="vault.isPrivate ==true">
        PRIVATE
      </p>
    </div>
    <div>
      <button type="button" class="btn btn-secondary" data-dismiss="modal" @click="deleteVault" v-if="state.account.id === vault.creatorId">
        Delete
      </button>
    </div>
  </div>
</template>

<script>
import { computed, reactive } from 'vue'
import { useRouter } from 'vue-router'
import { vaultsService } from '../services/VaultsService'
import { AppState } from '../AppState'
import Swal from 'sweetalert2'
import Notification from '../utils/Notification'
export default {
  name: 'Vault',
  props: {
    vault: {
      type: Object,
      required: true
    }
  },
  setup(props) {
    const router = useRouter()
    const state = reactive({
      account: computed(() => AppState.account),
      profile: computed(() => AppState.activeProfile)
    })
    return {
      router,
      state,
      async getVault() {
        await vaultsService.getById(props.vault.id)
        router.push({ name: 'VaultDetailsPage', params: { id: props.vault.id } })
      },
      async deleteVault() {
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
              vaultsService.delete(props.vault.id)
              // vaultsService.getVaultsByProfileId(props.vault.creatorId)
              Notification.toast('Successfully Deleted', 'success')
            }
          })
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
img{
  max-height: 25vh;
}
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
</style>
