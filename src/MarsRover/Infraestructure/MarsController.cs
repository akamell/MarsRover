using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Memory;

using MarsRoverTracking.MarsRover.Application;
using MarsRoverTracking.MarsRover.Domain;

namespace MarsRoverTracking.MarsRover.Infraestructure
{
    [ApiController]
    [Route("[controller]")]
    public class MarsRoverController : ControllerBase
    {
        private readonly IMarsGetService _marsGetService;
        private readonly IMarsUpdateService _marsUpdateService;
        private readonly IMemoryCache _cache;
        public MarsRoverController(
            IMarsGetService marsGetService,
            IMarsUpdateService marsUpdateService,
            IMemoryCache cache
        )
        {
            _marsGetService = marsGetService;
            _marsUpdateService = marsUpdateService;
            _cache = cache;
        }

        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            Domain.MarsRover result = this._marsGetService.Get(id).Result;
            if (string.IsNullOrEmpty(result?.RoverId))
                return NotFound();

            MarsRoverResponse response = new MarsRoverResponse()
            {
                Message = result.Message,
                CurrentPosition = $"({result.CurrentPositionX},{result.CurrentPositionY})"
            };
            return Ok(response);
        }

        [HttpPost]
        public IActionResult MovementRover(MovementRequest movement)
        {
            char[] validLetters = { 'L', 'R', 'M' };
            var notChars = movement.MovementInstruction.Where(
                item => !char.IsLetter(item) || !validLetters.Contains(item)
            );
            if (notChars.Count() > 0) return NotFound("MovementInstruction must contain only R,L and M");

            Domain.MarsRover result = this._marsUpdateService.UpdateMovement(movement).Result;
            MarsRoverResponse response = new MarsRoverResponse()
            {
                Message = result.Message,
                CurrentPosition = $"({result.CurrentPositionX},{result.CurrentPositionY})"
            };
            return Ok(response);
        }

        [HttpGet("fake")]
        public IActionResult GetFakeMarsRover()
        {
            Domain.MarsRover result = this._marsGetService.GetFake().Result;
            return Ok(result);
        }
    }
}
