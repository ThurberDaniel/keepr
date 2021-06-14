using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Keepr.Server.Models;

namespace Keepr.Server.Repositories
{
    public class VaultKeepsRepository
    {
        private readonly IDbConnection _db;

        public VaultKeepsRepository(IDbConnection db)
        {
            _db = db;
        }

    internal List<VaultKeepViewModel> GetVaultKeeps(int id)
    {
      //GO BACK HERE
      string sql = @"
      SELECT 
      k.*,
      vk.id AS vaultKeepId,
      vk.keepId AS keepId,
      vk.vaultId AS vaultId,
      profile.*
      FROM 
      vault_keeps vk
      JOIN keeps k ON k.id = vk.keepId
      JOIN accounts profile ON k.creatorId = profile.id
      WHERE vk.vaultId = @id;";
      return _db.Query<VaultKeepViewModel,Profile,VaultKeepViewModel>(sql,(v,p)=>{
        v.Creator = p;
        return v;
      }, new{id},splitOn: "id").ToList();
    }

    internal VaultKeepViewModel GetById(int id)
    {
      string sql = @"
      SELECT 
      k.*,
      vk.id AS vaultKeepId,
      vk.keepId AS keepId,
      vk.vaultId AS vaultId,
      profile.*
      FROM 
      vault_keeps vk
      JOIN keeps k ON k.id = vk.keepId
      JOIN accounts profile ON k.creatorId = profile.id
      WHERE vk.id = @id;";
      return _db.Query<VaultKeepViewModel, Profile, VaultKeepViewModel>(sql,(v,p)=>{
        v.Creator = p;
        return v;
      }, new{id},splitOn: "id").FirstOrDefault();
    }

    internal VaultKeep Create(VaultKeep vk)
    {
      string sql = @"
      INSERT INTO
      vault_keeps(vaultId, keepId, creatorId)
      VALUES(@VaultId, @KeepId, @CreatorId);
      SELECT LAST_INSERT_ID();";
      vk.Id = _db.ExecuteScalar<int>(sql,vk);
      return vk;
    }

    internal void Delete(int id)
    {
      string sql = @"DELETE FROM vault_keeps WHERE id=@Id LIMIT 1;";
      _db.Execute(sql, new{id});
    }
  }
}