using eazy.rest.data.Models;
using eazy.rest.services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SimpleValidator.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiTest.Controllers
{
    [Route("api/[controller]")]
    public class RestController : BaseController
    {
        private readonly IRestService<Guid> _repo;

        /// <summary>
        /// </summary>
        /// <param name="repo"></param>
        public RestController(IRestService<Guid> repo)
        {
            _repo = repo;
        }

        /// <summary>
        ///     Get all the record based on type
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetTaskAll/")]
        public async Task<IActionResult> GetTaskAll()
        {
            var result = await _repo.GetTaskAllAsync();
            return Ok(result);
        }

        /// <summary>
        ///     Get a single 
        /// </summary>
        /// <param name="pkId"></param>
        /// <returns></returns>
        [HttpGet("GetSingleTask/{pkId}/")]
        public async Task<IActionResult> GetSingleTask(Guid pkId)
        {
            var result = await _repo.GetSingleTaskAync(pkId);
            return Ok(result);
        }


        /// <summary>
        ///     Create a new  detail
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("CreateTask/")]
        public async Task<IActionResult> CreateTask([FromBody] IEnumerable<CreateDto> dto)
        {
            var result = new List<Guid>();

            if (dto.IsNotNull())
                foreach (var codes in dto)
                    result.Add(await _repo.CreateTaskAync(codes, AuthId()));

            return Ok(result);
        }


        /// <summary>
        ///     Update a single  code record
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="pkId"></param>
        /// <returns></returns>
        [HttpPut("UpdateTask/{pkId}/")]
        public async Task<IActionResult> UpdateTask([FromBody] UpdateDto dto, Guid pkId)
        {
            if (dto.IsNotNull())
            {
                var result = await _repo.UpdateTaskAync(dto, pkId, AuthId());
                return Ok("Updated!");
            }

            return StatusCode(StatusCodes.Status422UnprocessableEntity, "Object cannot contain null values.");
        }

        /// <summary>
        ///     Soft Delete Record
        /// </summary>
        /// <param name="pkId"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        [HttpDelete("DeleteTask/{pkId}/")]
        public async Task<IActionResult> DeleteTask(Guid pkId)
        {
            var result = await _repo.DeleteTaskAync(pkId);
            return Ok(result);
        }

    }
}
