using DataMatch.Enums;
using DataMatch.Models;
using DataMatch.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DataMatch.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    public class DiffController : ControllerBase
    {
        private readonly IDiffService _diffService;

        public DiffController(IDiffService diffService)
        {
            _diffService = diffService;
        }

        [HttpPut("{id}/left")]
        public IActionResult SetDataLeft(int id, [FromBody] InputDiff body)
        {
            bool boolBase64 = _diffService.ValidateBase64Encoded(body.Data);
            if (boolBase64 == false)
                return BadRequest();

            _diffService.SetData(id, DiffDirection.Left, body.Data);
            return StatusCode(201);
        }

        [HttpPut("{id}/right")]
        public IActionResult SetDataRight(int id, [FromBody] InputDiff body)
        {
            bool boolBase64 = _diffService.ValidateBase64Encoded(body.Data);
            if (boolBase64 == false)
                return BadRequest();

            _diffService.SetData(id, DiffDirection.Right, body.Data);
            return StatusCode(201);
        }

        [HttpGet("{id}")]
        public IActionResult GetData(int id)
        {
            var result = _diffService.DataMatch(id);

            if (result == null)
                return NotFound();

            return Ok(result);
        }
    }
}
