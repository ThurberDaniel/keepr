import { AppState } from '../AppState'
import { logger } from '../utils/Logger'
import { api } from './AxiosService'

class KeepsService {
  async getAll() {
    const res = await api.get('api/keeps')
    AppState.keeps = res.data.reverse()
  }

  async getKeepsByProfileId(id) {
    const res = await api.get(`api/profiles/${id}/keeps`)
    AppState.keeps = res.data.reverse()
  }

  async getKeepsByAccountId(id) {
    const res = await api.get(`api/profiles/${id}/keeps`)
    AppState.accountKeeps = res.data.reverse()
  }

  async getKeepsByVaultId(id) {
    const res = await api.get(`api/vaults/${id}/keeps`)
    AppState.keeps = res.data.reverse()
  }

  async getVaultKeep(id) {
    const res = await api.get(`api/vaultkeeps/${id}`)
    logger.log(res.data)
    AppState.activeVaultKeep = res.data
  }

  async getKeep(id) {
    const res = await api.get(`api/keeps/${id}`)
    AppState.activeKeep = res.data
  }

  async create(keep) {
    await api.post('api/keeps', keep)
    await this.getKeepsByAccountId(keep.creatorId)
  }

  async addVaultKeep(vaultKeep) {
    await api.post('api/vaultkeeps', vaultKeep)
  }

  async removeVaultKeep(vaultKeepId, vaultId) {
    await api.delete(`api/vaultkeeps/${vaultKeepId}`)
    this.getKeepsByVaultId(vaultId)
  }

  async delete(id) {
    await api.delete(`api/keeps/${id}`)
    this.getAll()
    this.getKeepsByAccountId(AppState.account.id)
  }

  async edit(keep) {
    await api.put(`api/keeps/${keep.id}`, keep)
    this.getAll()
  }
}
export const keepsService = new KeepsService()
