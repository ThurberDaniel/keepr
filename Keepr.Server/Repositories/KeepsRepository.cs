using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Keepr.Server.Models;

namespace Keepr.Server.Repositories
{
    public class KeepsRepository
    {
        private readonly IDbConnection _db;

        public KeepsRepository(IDbConnection db)
        {
            _db = db;
        }

    internal List<Keep> GetAll()
    {
      string sql = @"
      SELECT
      k.*,
      a.*
      FROM keeps k
      JOIN accounts a ON a.id = k.creatorId;";
      return _db.Query<Keep, Profile, Keep>(sql,(k,p)=>{
        k.Creator = p;
        return k;
      },splitOn: "id").ToList();
    }

    internal Keep GetById(int id)
    {
      string sql = @"
      SELECT
      k.*,
      a.*
      FROM keeps k
      JOIN accounts a ON a.id = k.creatorId
      WHERE k.id = @id;";
      return _db.Query<Keep, Profile, Keep>(sql, (k,p)=>{
      k.Creator = p;
      return k;
      },new {id}).FirstOrDefault();
    }

    internal List<Keep> GetKeepsByProfileId(string id)
    {
      string sql = @"
      SELECT
      k.*,
      a.*
      FROM keeps k
      JOIN accounts a ON a.id = k.creatorId
      WHERE k.creatorId = @id;";
      return _db.Query<Keep, Profile, Keep>(sql,(k,p)=>{
        k.Creator = p;
        return k;
      },new {id}).ToList();
    }

    internal Keep Create(Keep k)
    {
      string sql = @"
      INSERT INTO 
      keeps(name,description, img, creatorId, views, shares, keeps)
      VALUES(@Name,@Description, @Img, @CreatorId, 0, 0,0);
      SELECT LAST_INSERT_ID();";
      k.Id = _db.ExecuteScalar<int>(sql, k);
      return k;
    }

    internal Keep Edit(Keep original)
    {
      string sql =@"
      UPDATE keeps
      SET
      name = @Name,
      description=@Description,
      img=@Img,
      creatorId=@CreatorId,
      views=@Views,
      shares=@Shares,
      keeps=@Keeps
      WHERE id = @Id;";
      _db.Execute(sql, original);
      return original;
    }

    internal void Delete(int id)
    {
      string sql = "DELETE FROM keeps WHERE id = @id LIMIT 1;";
      _db.Execute(sql, new{id});
    }
  }
}