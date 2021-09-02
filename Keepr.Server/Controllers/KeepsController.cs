using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CodeWorks.Auth0Provider;
using Keepr.Server.Models;
using Keepr.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Keepr.Server.
{
    [ApiController]
    [Route("api/[controller]")]
    public class KeepsController : ControllerBase
    {
        private readonly KeepsService _service;
        private readonly AccountService _serviceAccount;

        public KeepsController(KeepsService service, AccountService acct)
        {
            _service = service;
            _serviceAccount = acct;
        }

        [HttpGet]
        public ActionResult<List<Keep>> GetAll()
        {
            try
            {
                List<Keep> keeps = _service.GetAll();
                return Ok(keeps);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<Keep> GetById(int id)
        {
            try
            {
                Keep keep = _service.GetById(id);
                return Ok(keep);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Keep>> Create([FromBody] Keep k)
        {
            try
            {
                Account userInfo = await HttpContext.GetUserInfoAsync<Account>();
                k.CreatorId = userInfo.Id;


                Keep newK = _service.Create(k);
                newK.Creator = userInfo;
                return Ok(newK);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<Keep>> Edit(int id, [FromBody] Keep k)
        {
            try
            {
                Account userInfo = await HttpContext.GetUserInfoAsync<Account>();
                k.Id = id;
                // k.CreatorId = userInfo.Id;
                Keep newK = _service.Edit(k, userInfo.Id);
                newK.Creator = userInfo;
                return Ok(newK);
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