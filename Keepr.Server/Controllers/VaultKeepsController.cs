using System;
using System.Threading.Tasks;
using CodeWorks.Auth0Provider;
using Keepr.Server.Models;
using Keepr.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Keepr.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VaultKeepsController : ControllerBase
    {
        private readonly VaultKeepsService _service;

        public VaultKeepsController(VaultKeepsService service)
        {
            _service = service;
        }

                [HttpGet("{id}")]
        public ActionResult<VaultKeepViewModel> GetById(int id)
        {
            try
            {
                // VaultKeepViewModel keep = _service.GetById(id);

                Keep keep = _service.GetById(id);
                return Ok(keep);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


    [Authorize]
    [HttpPost]
    public async Task<ActionResult<VaultKeep>> Create([FromBody] VaultKeep vk)
    {
        try
        {
            Account userInfo = await HttpContext.GetUserInfoAsync<Account>();
            vk.CreatorId = userInfo.Id;
            VaultKeep newVk = _service.Create(vk);
            return Ok(newVk);
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