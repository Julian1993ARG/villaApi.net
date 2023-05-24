using MagicVilla.Models;
using MagicVilla.Models.Dto;
using MagicVilla.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaController : ControllerBase
    {
        private readonly ILogger<VillaController> _logger;
        private readonly VillasServices _villasServices;
        public VillaController(ILogger<VillaController> logger, VillasServices villasServices)
        {
            _logger = logger;
            _villasServices = villasServices;
        }


        // Peticiones HTTP
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<VillaDto>>> GetVillas() {
            return Ok(await _villasServices.GetVillas()); 
        }

        [HttpGet("id:int", Name = "GetVillaById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<VillaDto>> GetVillaById(int id)
        {
            if (id == 0 || id.GetType() != typeof(int)) return BadRequest();

            var villa = await _villasServices.GetVillaById(id);
            if (villa == null) return NotFound("Villa no encontrada");

            return Ok(villa);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<VillaCreateDto>> PostVilla([FromBody] VillaCreateDto villa)
        {
            if (villa == null) return BadRequest();
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var existVilla = await _villasServices.GetVillaByName(villa.Name);
            if (existVilla != null) return BadRequest("Ya existe una villa con ese nombre");

            int id = await _villasServices.CreateVilla(villa);

            return CreatedAtRoute("GetVillaById", new { id }, villa);

        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteVillaById(int id)
        {
            if (id == 0 || id.GetType() != typeof(int)) return BadRequest("Es necesario pasar un id");
            var villa = await _villasServices.GetVillaById(id);
            if (villa == null) return NotFound("Villa no encontrada");

            await _villasServices.DeleteVilla(villa);
            return NoContent();
        }

        [HttpPut("id:int")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutVilla(int id, [FromBody] VillaUpdateDto villa)
        {
            if (id == 0 || id.GetType() != typeof(int)) return BadRequest("Es necesario pasar un id");
            if (villa == null) return BadRequest();

            var existVilla = await _villasServices.GetVillaById(id);
            if (existVilla == null) return NotFound("Villa no encontrada");

            await _villasServices.UpdateVilla(existVilla.Id, villa);
            return NoContent();
        }

        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PatchVilla(int id, JsonPatchDocument<Villa> patch)
        {
            if (patch == null) return BadRequest();

            var villa = await _villasServices.GetVillaById(id);
            if (villa == null) return BadRequest();

            var model = _villasServices.RetVilla(villa);
            patch.ApplyTo(model, ModelState);
          
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _villasServices.UpdateVilla(model);

            return NoContent();
        }

    }
}

