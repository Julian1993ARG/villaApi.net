using AutoMapper;
using MagicVilla.Models;
using MagicVilla.Models.Dto;
using MagicVilla.Reposiory.IRepository;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla.Controllers
{
    // Continuar video desde 3:03:00
    [Route("api/[controller]")]
    [ApiController]
    public class VillaController : ControllerBase
    {
        private readonly ILogger<VillaController> _logger;
        private readonly IMapper _mapper;
        private readonly IVillaRepository _villaRepository;
        public VillaController(ILogger<VillaController> logger,  IMapper mapper, IVillaRepository villaRepo)
        {
            _logger = logger;
            _mapper = mapper;
            _villaRepository = villaRepo;
        }


        // Peticiones HTTP
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<VillaDto>>> GetVillas() {
            var villas = await _villaRepository.GetAll();
            return Ok(_mapper.Map<IEnumerable<VillaDto>>(villas)); 
        }

        [HttpGet("id:int", Name = "GetVillaById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<VillaDto>> GetVillaById(int id)
        {
            if (id == 0 || id.GetType() != typeof(int)) return BadRequest();

            var villa = await _villaRepository.Get(v => v.Id == id);
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

            var existVilla = await _villaRepository.Get(v=>v.Name.ToLower() == villa.Name.ToLower());
            if (existVilla != null) return BadRequest("Ya existe una villa con ese nombre");
            var newVilla = _mapper.Map<Villa>(villa);
            newVilla.CreateAt = DateTime.Now;
            
            await _villaRepository.Create(newVilla);

            return CreatedAtRoute("GetVillaById", new { newVilla.Id }, villa);

        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteVillaById(int id)
        {
            if (id == 0 || id.GetType() != typeof(int)) return BadRequest("Es necesario pasar un id");
            var villa =  await _villaRepository.Get(v => v.Id == id);
            if (villa == null) return NotFound("Villa no encontrada");
            var deleteVilla = _mapper.Map<Villa>(villa);
            deleteVilla.Id = id;

            await _villaRepository.Delete(deleteVilla);
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

            var existVilla = await _villaRepository.Get(v => v.Id == id);
            if (existVilla == null) return NotFound("Villa no encontrada");
            var newVilla = _mapper.Map<Villa>(villa);
            newVilla.Id = id;
            await _villaRepository.Update(newVilla);
            return NoContent();
        }

        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PatchVilla(int id, JsonPatchDocument<Villa> patch)
        {
            if (patch == null) return BadRequest();
            var villa = await _villaRepository.Get(v => v.Id == id);
            if (villa == null) return BadRequest("Villa no encontrada");

            patch.ApplyTo(villa, ModelState);
            
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _villaRepository.Update(villa);

            return NoContent();
        }

    }
}

