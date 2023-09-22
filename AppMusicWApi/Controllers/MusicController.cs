using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DbContext;
using DbModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Models;
using Models.DTO;
using Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AppMusicWApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class MusicController : Controller
    {
        IMusicService _service;
        ILogger<MusicController> _logger;

        //GET: api/music/seed?count={count}
        [HttpGet()]
        [ActionName("Seed")]
        [ProducesResponseType(200, Type = typeof(int))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> Seed(string count)
        {
            try
            {
                int _count = int.Parse(count);

                int cnt = await _service.Seed(_count);
                return Ok(cnt);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
           
        }

        //GET: api/music/removeseed
        [HttpGet()]
        [ActionName("RemoveSeed")]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> RemoveSeed()
        {         
            try
            {
                int _count = await _service.RemoveSeed();
                return Ok(_count);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message);
            }

        }

        //GET: api/music/read
        [HttpGet()]
        [ActionName("Read")]
        [ProducesResponseType(200, Type = typeof(List<csMusicGroup>))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> Read(string flat = "true")
        {
            try
            {
                bool _flat = bool.Parse(flat);
                var _list = await _service.Read(_flat);

                return Ok(_list);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        //GET: api/music/readitem
        [HttpGet()]
        [ActionName("ReadItem")]
        [ProducesResponseType(200, Type = typeof(csMusicGroup))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> ReadItem(string id, string flat = "false")
        {
            try
            {
                var _id = Guid.Parse(id);
                bool _flat = bool.Parse(flat);
                var mg = await _service.ReadItem(_id, _flat);

                if (mg == null)
                {
                    return BadRequest($"Item with id {id} does not exist");
                }

                return Ok(mg);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //GET: api/music/readitemdto
        [HttpGet()]
        [ActionName("ReadItemDto")]
        [ProducesResponseType(200, Type = typeof(csMusicGroupCUdto))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> ReadItemDto(string id)
        {
            try
            {
                var _id = Guid.Parse(id);
                var mg = await _service.ReadItem(_id, false);

                if (mg == null)
                {
                    return BadRequest($"Item with id {id} does not exist");
                }

                var dto = new csMusicGroupCUdto(mg);
                return Ok(dto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //GET: api/music/deleteitem
        [HttpGet()]
        [ActionName("DeleteItem")]
        [ProducesResponseType(200, Type = typeof(csMusicGroup))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> DeleteItem(string id)
        {
            try
            {
                var _id = Guid.Parse(id);
                using (var db = csMainDbContext.DbContext("sysadmin"))
                {
                    var mg = await db.MusicGroups
                                      .FirstOrDefaultAsync(mg => mg.MusicGroupId == _id);
                    if (mg == null)
                    {
                        return BadRequest($"Item with id {id} does not exist");
                    }

                    db.MusicGroups.Remove(mg);

                    await db.SaveChangesAsync();
                    return Ok(mg);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //Put: api/music/updateitem
        [HttpPut("{id}")]
        [ActionName("UpdateItem")]
        [ProducesResponseType(200, Type = typeof(csMusicGroup))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> UpdateItem(string id, [FromBody] csMusicGroupCUdto item)
        {
            try
            {
                var _id = Guid.Parse(id);

                if (item.MusicGroupId != _id)
                    throw new Exception("Id mismatch");

                var mg = await _service.UpdateItem(item);

                return Ok(mg);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        public MusicController(IMusicService service , ILogger<MusicController> logger)
        {
            _service = service;
            _logger = logger;
        }
    }
}

