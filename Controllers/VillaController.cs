using MagicVilla.Data;
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
        public async Task<ActionResult<IEnumerable<DBVillaDto>>> GetVillas() {
            return Ok(await _villasServices.GetVillas()); 
        }

        [HttpGet("id:int", Name = "GetVillaById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<DBVillaDto>> GetVillaById(int id)
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
        public async Task<ActionResult<DBVillaDto>> PostVilla([FromBody] DBVillaDto villa)
        {
            if (villa == null) return BadRequest();
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var existVilla = await _villasServices.GetVillaByName(villa.Name);
            if (existVilla != null) return BadRequest("Ya existe una villa con ese nombre");

            Villa model = new(villa.Name, villa.Details, villa.Occupants, villa.Rate, villa.SquareMeter, villa.ImageUrl, villa.Amenity);
           
            await _villasServices.CreateVilla(model);

            return CreatedAtRoute("GetVillaById", new { id = villa.Id }, villa);

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
        public async Task<IActionResult> PutVilla(int id, [FromBody] DBVillaDto villa)
        {
            if (id == 0 || id.GetType() != typeof(int)) return BadRequest("Es necesario pasar un id");
            if (villa == null) return BadRequest();
            if (id != villa.Id) return BadRequest("El id de la villa no coincide con el id de la url");

            var existVilla = await _villasServices.GetVillaById(id);
            if (existVilla == null) return NotFound("Villa no encontrada");

            Villa model = new(villa.Id,villa.Name, villa.Details, villa.Occupants, villa.Rate, villa.SquareMeter, villa.ImageUrl, villa.Amenity);
            
            await _villasServices.UpdateVilla(model);
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

            Villa model = new(villa.Id, villa.Name, villa.Details, villa.Occupants, villa.Rate, villa.SquareMeter, villa.ImageUrl, villa.Amenity);

            patch.ApplyTo(model, ModelState);

            await _villasServices.UpdateVilla(model);

            if (!ModelState.IsValid) return BadRequest(ModelState);
            return NoContent();
        }

    }
}



//[HttpGet]
//public ActionResult<IEnumerable<VillaDto>> GetVillas()
//{
//    _logger.LogInformation("GetVillas");
//    return Ok(VillaStore.VillaList);
//}

//[HttpGet("id:int", Name = "GetVillaById")]
//[ProducesResponseType(StatusCodes.Status200OK)]
//[ProducesResponseType(StatusCodes.Status400BadRequest)]
//[ProducesResponseType(StatusCodes.Status404NotFound)]
//public ActionResult<VillaDto> GetVillaById(int id)
//{
//    if (id == 0 || id.GetType() != typeof(int)) return BadRequest();

//    var villa = VillaStore.GetVillaById(id);
//    if (villa == null) return NotFound("Villa no encontrada");

//    return Ok(villa);
//}

//[HttpGet("name:string", Name = "GetVillaByName")]
//[ProducesResponseType(StatusCodes.Status400BadRequest)]
//[ProducesResponseType(StatusCodes.Status404NotFound)]
//public ActionResult<VillaDto> GetVillaByName(string name)
//{
//    if (name.GetType() != typeof(string)) return BadRequest();
//    if (string.IsNullOrEmpty(name)) return NotFound("Es necesario pasar un nombre");

//    var existVilla = VillaStore.GetVillaByName(name);
//    if (existVilla == null) return NotFound("Villa no encontrada");

//    return Ok(existVilla);
//}

//[HttpPost]
//[ProducesResponseType(StatusCodes.Status201Created)]
//[ProducesResponseType(StatusCodes.Status400BadRequest)]
//[ProducesResponseType(StatusCodes.Status500InternalServerError)]
//public ActionResult<VillaDto> PostVilla([FromBody] VillaDto villa)
//{
//    if (villa == null) return BadRequest();
//    if (villa.Id > 0) return StatusCode(StatusCodes.Status500InternalServerError, "No se puede crear la villa, el id debe ser ");
//    if (!ModelState.IsValid) return BadRequest(ModelState);

//    var existVilla = VillaStore.VillaList.FirstOrDefault(v => v.Name.ToLower() == villa.Name.ToLower());
//    if (existVilla != null) return BadRequest("Ya existe una villa con ese nombre");

//    int nextId = VillaStore.VillaList.OrderByDescending(v => v.Id).First().Id + 1;

//    villa.Id = nextId;
//    VillaStore.VillaList.Add(villa);

//    return CreatedAtRoute("GetVillaById", new { id = villa.Id }, villa);

//}

//[HttpDelete("{id:int}")]
//[ProducesResponseType(StatusCodes.Status204NoContent)]
//[ProducesResponseType(StatusCodes.Status400BadRequest)]
//[ProducesResponseType(StatusCodes.Status404NotFound)]

//public IActionResult DeleteVillaById(int id)
//{
//    if (id == 0 || id.GetType() != typeof(int)) return BadRequest("Es necesario pasar un id");
//    var villa = VillaStore.GetVillaById(id);
//    if (villa == null) return NotFound("Villa no encontrada");

//    VillaStore.VillaList.Remove(villa);
//    return NoContent();
//}

//[HttpPut("{id:int}")]
//[ProducesResponseType(StatusCodes.Status204NoContent)]
//[ProducesResponseType(StatusCodes.Status400BadRequest)]
//[ProducesResponseType(StatusCodes.Status404NotFound)]
//public IActionResult PutVilla(int id, [FromBody] VillaDto villa)
//{
//    if (id == 0 || id.GetType() != typeof(int)) return BadRequest("Es necesario pasar un id");
//    if (villa == null) return BadRequest();
//    if (id != villa.Id) return BadRequest("El id de la villa no coincide con el id de la url");

//    var existVilla = VillaStore.GetVillaById(id);
//    if (existVilla == null) return NotFound("Villa no encontrada");

//    VillaStore.VillaList.Remove(existVilla);

//    VillaStore.VillaList.Add(villa);
//    return NoContent();
//}

//[HttpPatch("{id:int}")]
//[ProducesResponseType(StatusCodes.Status204NoContent)]
//[ProducesResponseType(StatusCodes.Status400BadRequest)]
//[ProducesResponseType(StatusCodes.Status404NotFound)]
//public IActionResult PatchVilla(int id, JsonPatchDocument<VillaDto> villaDto)
//{
//    if (villaDto == null) return BadRequest();

//    var villa = VillaStore.GetVillaById(id);
//    if (villa == null) return BadRequest();

//    villaDto.ApplyTo(villa, ModelState);

//    if (!ModelState.IsValid) return BadRequest(ModelState);
//    return NoContent();
//}