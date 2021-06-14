using System;
using Keepr.Server.Models;
using Keepr.Server.Repositories;

namespace Keepr.Server.Services
{
    public class VaultKeepsService
    {
    private readonly VaultKeepsRepository _repo;
    private readonly KeepsRepository _repoKeep;

    public VaultKeepsService(VaultKeepsRepository vks, KeepsRepository ks)
    {
    _repo = vks;
    _repoKeep = ks;
    }

    internal VaultKeep Create(VaultKeep vk)
    {
      Keep keep = _repoKeep.GetById(vk.KeepId);
      keep.Keeps++;
      _repoKeep.Edit(keep);
      return _repo.Create(vk);
    }

    internal void Delete(int id, string creatorId)
    {
      VaultKeepViewModel vk = _repo.GetById(id);
      Keep keep = _repoKeep.GetById(vk.Id);
      if(keep.CreatorId != creatorId)
      {
        throw new Exception("You cannot delete this");
      }
      keep.Keeps--;
      _repoKeep.Edit(keep);
      _repo.Delete(id);

    }

    internal VaultKeepViewModel GetById(int id)
    {
      VaultKeepViewModel keep = _repo.GetById(id);
      if(keep == null)
      {
        throw new Exception("Invalid ID");
      }
      return keep;
    }
  }
}