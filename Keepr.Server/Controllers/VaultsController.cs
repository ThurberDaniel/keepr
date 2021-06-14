using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CodeWorks.Auth0Provider;
using Keepr.Server.Models;
using Keepr.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Keepr.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VaultsController : ControllerBase
    {
        private readonly VaultsService _service;

        public VaultsController(VaultsService service)
        {
            _service = service;
        }

        // [HttpGet]
        // public ActionResult<List<Vault>> GetAll()
        // {
        //     try
        //     {
        //         List<Vault> vaults = _service.GetAll();
        //         return Ok(vaults);
        //     }
        //     catch (Exception e)
        //     {
        //         return BadRequest(e.Message);
        //     }
        // }

        [HttpGet("{id}")]
        public async Task<ActionResult<Vault>> GetById(int id)
        {
            try
            {
                Account userInfo = await HttpContext.GetUserInfoAsync<Account>();
                Vault vault = _service.GetById(id);
                if(userInfo == null && vault.IsPrivate == true){
                    throw new Exception("This is private;");
                }
                if(vault.IsPrivate == true){
                    if(userInfo.Id == vault.CreatorId){
                        return vault;
                    }
                    throw new Exception("This is private;");
                }
                return Ok(vault);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}/keeps")]
        public async Task<ActionResult<List<VaultKeepViewModel>>> GetVaultKeeps(int id)
        {
            try
            {
                Account userInfo = await HttpContext.GetUserInfoAsync<Account>();
                Vault vault = _service.GetById(id);
                List<VaultKeepViewModel> keeps = _service.GetVaultKeeps(id);
                if(userInfo == null && vault.IsPrivate == true){
                    throw new Exception("This is private;");
                }
                if(vault.IsPrivate == true){
                    if(userInfo.Id == vault.CreatorId){
                        return Ok(keeps);
                    }
                    throw new Exception("This is private;");
                }
                return Ok(keeps);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Vault>> Create([FromBody] Vault v)
        {
            try
            {
                Account userInfo = await HttpContext.GetUserInfoAsync<Account>();
                v.CreatorId = userInfo.Id;
                Vault newV = _service.Create(v);
                newV.Creator = userInfo;
                return Ok(newV);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<Vault>> Edit(int id, [FromBody] Vault v)
        {
            try
            {
                Account userInfo = await HttpContext.GetUserInfoAsync<Account>();
                v.Id = id;
                // v.CreatorId = userInfo.Id;
                Vault newV = _service.Edit(v, userInfo.Id);
                newV.Creator = userInfo;
                return Ok(newV); 
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<string>> Delete(int id)
        {
            try
            {
                Account userInfo = await HttpContext.GetUserInfoAsync<Account>();
                _service.Delete(id, userInfo.Id);
                return Ok("Successfully Deleted");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


















    }
}