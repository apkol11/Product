using Business.Interfaces.Handler;
using Domain.Request;
using Domain.Response;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    /// <summary>
    /// API controller for managing colour operations.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ColourController : ControllerBase
    {
        private readonly IColourHandler _colourHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="ColourController"/> class.
        /// </summary>
        /// <param name="colourHandler">The colour handler instance.</param>
        public ColourController(IColourHandler colourHandler)
        {
            _colourHandler = colourHandler;
        }

        /// <summary>
        /// Creates a new colour.
        /// </summary>
        /// <param name="colourRequest">The colour creation request.</param>
        /// <returns>A standardized response containing the created colour's identifier and location.</returns>
        /// <response code="201">Colour created successfully.</response>
        /// <response code="400">Invalid request data.</response>
        [HttpPost]
        [ProducesResponseType(typeof(CreatedResponse), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromBody] ColourRequest colourRequest)
        {
            var id = await _colourHandler.AddColour(colourRequest);
            
            var response = new CreatedResponse
            {
                Id = id,
                Message = "Colour created successfully",
                Location = Url.Action(nameof(Get), new { id = id })
            };

            return CreatedAtAction(nameof(Get), new { id = id }, response);
        }

        /// <summary>
        /// Retrieves all colours.
        /// </summary>
        /// <returns>A collection of all colours.</returns>
        /// <response code="200">Returns the list of colours.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Domain.EntityModel.Colour>), 200)]
        public async Task<ActionResult<IEnumerable<Domain.EntityModel.Colour>>> Get()
        {
            var colours = await _colourHandler.GetAllColours();
            return Ok(colours);
        }
    }
}
    