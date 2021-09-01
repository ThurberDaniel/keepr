using System.Data;
using Dapper;
using Keepr.Server.Models;

namespace Keepr.Server.Repositories
{
    public class AccountsRepository
    {
        private readonly IDbConnection _db;

        public AccountsRepository(IDbConnection db)
        {
            _db = db;
        }

        internal Account GetById(string id)
        {
            string sql = "SELECT * FROM Accounts WHERE id = @id";
            return _db.QueryFirstOrDefault<Account>(sql, new { id });
        }

        internal Account Create(Account newAccount)
        {
            string sql = @"
            INSERT INTO Accounts
              (name, picture, email, id)
            VALUES
              (@Name, @Picture, @Email, @Id)";
            _db.Execute(sql, newAccount);
            
        }

    }
}