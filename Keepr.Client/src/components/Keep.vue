<template>
  <div class="Keep card rounded shadow">
    <img :src="keep.img"
         alt="keep image"
         class="w-100 image rounded"
         @click="getKeep"
    >
    <div class="card-title info">
      <p class="keep-name mr-1">
        {{ keep.name }}
      </p>
      <router-link :to="{ name: 'Account' }" v-if="state.account.id===keep.creatorId">
        <img :src="keep.creator.picture" alt="profile picture" class="rounded-circle small-img">
      </router-link>
      <router-link :to="{name: 'ProfileDetailsPage', params:{id:keep.creator.id}}" v-else>
        <img :src="keep.creator.picture" alt="profile picture" class="rounded-circle small-img">
      </router-link>
    </div>
    <KeepDetailsModal :keep="keep" />
  </div>
</template>

<script>
import { keepsService } from '../services/KeepsService'
import $ from 'jquery'
import Notification from '../utils/Notification'
import { computed, reactive } from 'vue'
import { AppState } from '../AppState'
export default {
  name: 'Keep',
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
          $('#keepDetails').modal('show')
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
  display: block;
  // margin: 0 !important;
// max-width: 20vw;

}
// .card:hover{

// }

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
overflow-wrap: break-word;
}

.keep-name{
  font-size: 1em;
  font-weight: bold;
  color: white;
}

.small-img{
  height:1.5em;
  width: 1.5em;
}
</style>
