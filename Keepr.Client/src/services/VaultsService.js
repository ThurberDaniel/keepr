import { AppState } from '../AppState'
import { api } from './AxiosService'

class VaultsService {
  async getById(id) {
    const res = await api.get(`api/vaults/${id}`)
    AppState.activeVault = res.data
  }

  async getVaultsByProfileId(id) {
    const res = await api.get(`api/profiles/${id}/vaults`)
    AppState.vaults = res.data.reverse()
  }

  async getVaultsByAccountId(id) {
    const res = await api.get(`api/profiles/${id}/vaults`)
    AppState.accountVaults = res.data.reverse()
  }

  async create(newVault) {
    const res = await api.post('api/vaults', newVault)
    this.getVaultsByAccountId(res.data.creatorId)
  }

  async edit(vault) {
    await api.put(`api/vaults/${vault.id}`, vault)
    this.getVaultsByProfileId(vault.creatorId)
  }

  async delete(id) {
    await api.delete(`api/vaults/${id}`)
    this.getVaultsByAccountId(AppState.account.id)
  }
}
export const vaultsService = new VaultsService()
