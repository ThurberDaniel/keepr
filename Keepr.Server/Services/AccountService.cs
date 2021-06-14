using Keepr.Server.Models;
using Keepr.Server.Repositories;

namespace Keepr.Server.Services
{
    public class AccountService
    {
        private readonly AccountsRepository _repo;
        public AccountService(AccountsRepository repo)
        {
            _repo = repo;
        }
        internal Account GetOrCreateProfile(Account userInfo)
        {
            Account profile = _repo.GetById(userInfo.Id);
            if (profile == null)
            {
                return _repo.Create(userInfo);
            }
            return profile;
        }

        internal Profile GetProfileById(string id)
        {
            return _repo.GetById(id);
        }

    }
}