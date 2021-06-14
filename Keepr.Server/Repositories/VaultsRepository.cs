using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Keepr.Server.Models;

namespace Keepr.Server.Repositories
{
    public class VaultsRepository
    {
        private readonly IDbConnection _db;

        public VaultsRepository(IDbConnection db)
        {
            _db = db;
        }

    // internal List<Vault> GetAll()
    // {
    //   string sql =@"
    //   SELECT 
    //   v.*,
    //   a.*
    //   FROM vaults v
    //   JOIN accounts a ON v.creatorId = a.id;
    //   ";
    //   return _db.Query<Vault, Profile, Vault>(sql, (v,p)=>{
    //     v.Creator = p;
    //     return v;
    //   }, splitOn:"id").ToList();
    // }
    internal List<Vault> GetVaultsByProfileId(string id)
    {
      string sql = @"
            SELECT 
      v.*, 
      a.*
      FROM vaults v
      JOIN accounts a ON a.id = v.creatorId
      WHERE v.creatorId = @id AND isPrivate = 0;";
      return _db.Query<Vault, Profile, Vault>(sql, (v,p)=>{
        v.Creator = p;
        return v;
      }, new{id}).ToList();
    }

    internal List<Vault> GetVaultsByAccountId(string userInfoId)
    {
      string sql = @"
      SELECT 
      v.*, 
      a.*
      FROM vaults v
      JOIN accounts a ON a.id = v.creatorId
      WHERE v.creatorId = @userInfoId;";
      return _db.Query<Vault, Profile, Vault>(sql, (v,p)=>{
        v.Creator = p;
        return v;
      }, new{userInfoId}).ToList();
    }
    internal Vault GetById(int id)
    {
      // GO
      string sql = @"
      SELECT 
      v.*,
      a.*
      FROM vaults v
      JOIN accounts a ON a.id = v.creatorId
      WHERE v.id = @id;";
      return _db.Query<Vault, Profile, Vault>(sql, (v,p)=>{
        v.Creator = p;
        return v;
      }, new {id}).FirstOrDefault();
    }

    internal Vault Create(Vault v)
    {
      string sql = @"
      INSERT INTO
      vaults(name, description, imgUrl, isPrivate, creatorId)
      VALUES(@Name, @Description, @ImgUrl, @IsPrivate, @CreatorId);
      SELECT LAST_INSERT_ID();";
      v.Id = _db.ExecuteScalar<int>(sql, v);
      return v;
    }

    internal Vault Edit(Vault v)
    {
      string sql = @"
      UPDATE vaults
      SET
        name=@Name,
        description = @Description,
        imgUrl = @ImgUrl,
        isPrivate = @IsPrivate,
        creatorId = @CreatorId
        
        WHERE id = @Id;";
        _db.Execute(sql, v);
        return v;
    }

    internal void Delete(int id)
    {
      string sql = "DELETE FROM vaults WHERE id = @Id LIMIT 1;";
      _db.Execute(sql, new {id});
    }


  }
}