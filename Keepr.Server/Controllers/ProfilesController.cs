using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CodeWorks.Auth0Provider;
using Keepr.Server.Models;
using Keepr.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace Keepr.Server.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ProfilesController : ControllerBase
    {
        private readonly AccountService _accountService;
        private readonly KeepsService _keepsService;
        private readonly VaultsService _vaultsService;

    public ProfilesController(AccountService accountService, KeepsService keepsService, VaultsService vs)
        {
            _accountService = accountService;
            _keepsService = keepsService;
            _vaultsService = vs;
        }


        [HttpGet("{id}")]
        public ActionResult<Profile> GetProfileById(string id)
        {
            try
            {
                Profile profile = _accountService.GetProfileById(id);
                return Ok(profile);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}/keeps")]
        public ActionResult<List<Keep>> GetKeepsByProfileId(string id)
        {
            try
            {
                List<Keep> keeps = _keepsService.GetKeepsByProfileId(id);
                return Ok(keeps);

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpGet("{id}/vaults")]
        public async Task<ActionResult<List<Vault>>> GetVaultsByProfileId(string id)
        {
            try
            {
                Account userInfo = await HttpContext.GetUserInfoAsync<Account>();

                //Private
                if(userInfo!=null){
                //if owner return all
                    if(id == userInfo.Id)
                    {
                    List<Vault> vaults = _vaultsService.GetVaultsByAccountId(id);
                    return Ok(vaults);
                    }else{
                    List<Vault> pubvaults = _vaultsService.GetVaultsByProfileId(id);
                    return Ok(pubvaults);
                    }
                }
                else if(userInfo==null)
                {
                List<Vault> vaults = _vaultsService.GetVaultsByProfileId(id);
                return Ok(vaults);
                }else{
                return BadRequest("That is Private");
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
















    }
}