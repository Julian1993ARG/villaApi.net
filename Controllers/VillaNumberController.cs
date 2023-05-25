using AutoMapper;
using MagicVilla.Models;
using MagicVilla.Models.Dto;
using MagicVilla.Reposiory.IRepository;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MagicVilla.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaNumberController : ControllerBase
    {
        private readonly ILogger<VillaController> _logger;
        private readonly IMapper _mapper;
        private readonly IVillaRepository _villaRepository;
        private readonly IVillaNumberRepository _villaNumberRepository;
        protected DefaultResponse _response;
        public VillaNumberController(ILogger<VillaController> logger,  IMapper mapper, IVillaRepository villaRepo, IVillaNumberRepository villaNumberRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _villaRepository = villaRepo;
            _response = new();
            _villaNumberRepository = villaNumberRepository;
        }


        // Peticiones HTTP
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<DefaultResponse>> GetNumberVillas() {
            try
            {
                var villas = await _villaNumberRepository.GetAll();
                _response.Data = _mapper.Map<IEnumerable<VillaNumberDto>>(villas);
                _response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                _response.Ok = false;
                _response.ErrorMessage = new List<string>() { ex.ToString() };
            }
            return _response; 
        }

        [HttpGet("id:int", Name = "GetVillaNumberById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<DefaultResponse>> GetNumberVillaById(int id)
        {
                if (id == 0 || id.GetType() != typeof(int))
            {
                _response.Ok = false;
                _response.ErrorMessage = new List<string>() { "Es necesario pasar un id" };
                _response.StatusCode = HttpStatusCode.BadRequest;
                return _response;
            }
            try
            {
                var villa = await _villaNumberRepository.Get(v => v.VillaNo == id);
                if (villa == null)
                {
                    _response.Ok = false;
                    _response.ErrorMessage = new List<string>() { "Villa no encontrada" };
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return _response;
                }

                _response.Data = villa;
                _response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                _response.Ok = false;
                _response.ErrorMessage = new List<string>() { ex.ToString() };
            }

            return _response;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<DefaultResponse>> PostVillaNumber([FromBody] VillaNumberCreateDto villa)
        {
            if (villa == null)  {
                _response.Ok = false;
                _response.ErrorMessage = new List<string>() { "Debe pasar una villa" };
                _response.StatusCode = HttpStatusCode.BadRequest;
                return _response;
            }
            if (!ModelState.IsValid)
            {
                _response.Ok = false;
                _response.ErrorMessage = new List<string>() { "Villa no válida" };
                _response.StatusCode = HttpStatusCode.BadRequest;
                return _response;
            }

            try
            {
                var existNumberVilla = await _villaNumberRepository.Get(v => v.VillaNo == villa.VillaNo);
                if (existNumberVilla != null)
                {
                    _response.Ok = false;
                    _response.ErrorMessage = new List<string>() { "Ya existe una villa con ese nombre" };
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return _response;
                }
                var existVillaFather = await _villaRepository.Get(v => v.Id == villa.VillaId);
                if (existVillaFather == null)
                {
                    _response.Ok = false;
                    _response.ErrorMessage = new List<string>() { "No existe la villa padre" };
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return _response;
                }

                var newVilla = _mapper.Map<VillaNumber>(villa);
                newVilla.CreateAt = DateTime.Now;
                newVilla.UpdateAt = DateTime.Now;
                await _villaNumberRepository.Create(newVilla);
                _response.Data = newVilla;
                _response.StatusCode = HttpStatusCode.Created;

            }
            catch (Exception ex)
            {
                _response.Ok = false;
                _response.ErrorMessage = new List<string>() { ex.ToString() };
            }
            return CreatedAtRoute("GetVillaNumberById", new { id= 0 }, _response);

        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<DefaultResponse> DeleteVillaNumberById(int id)
        {
            if (id == 0 || id.GetType() != typeof(int))
            {
                _response.Ok = false;
                _response.ErrorMessage = new List<string>() { "Es necesario pasar un id" };
                _response.StatusCode = HttpStatusCode.BadRequest;
                return _response;
            }
            try
            {
                var villa = await _villaNumberRepository.Get(v => v.VillaNo == id);
                if (villa == null)
                {
                    _response.Ok = false;
                    _response.ErrorMessage = new List<string>() { "Villa no encontrada" };
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return _response;
                }
               
                var deleteVilla = _mapper.Map<VillaNumber>(villa);
                deleteVilla.VillaNo = id;
                await _villaNumberRepository.Delete(deleteVilla);
                _response.StatusCode = HttpStatusCode.NoContent;
            }
            catch (Exception ex)
            {
                _response.Ok = false;
                _response.ErrorMessage = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpPut("id:int")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<DefaultResponse> PutNumberVilla(int id, [FromBody] VillaNumberUpdateDto villa)
        {
            if (id == 0 || id.GetType() != typeof(int))
            {
                _response.Ok = false;
                _response.ErrorMessage = new List<string>() { "Es necesario pasar un id" };
                _response.StatusCode = HttpStatusCode.BadRequest;
                return _response;
            }
            if (villa == null)
            {
                _response.Ok = false;
                _response.ErrorMessage = new List<string>() { "Debe pasar una villa" };
                _response.StatusCode = HttpStatusCode.BadRequest;
                return _response;
            }

            try
            {
                var existVilla = await _villaNumberRepository.Get(v => v.VillaNo == id, false);
                if (existVilla == null)
                {
                    _response.Ok = false;
                    _response.ErrorMessage = new List<string>() { "Villa no encontrada" };
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return _response;
                }
                var existVillaFather = await _villaRepository.Get(v => v.Id == villa.VillaId);
                if (existVillaFather == null)
                {
                    _response.Ok = false;
                    _response.ErrorMessage = new List<string>() { "No existe la villa padre" };
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return _response;
                }
                var newVilla = _mapper.Map<VillaNumber>(villa);
                newVilla.VillaNo = id;
                await _villaNumberRepository.Update(newVilla);
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.Data = newVilla;
            }
            catch (Exception ex)
            {
                _response.Ok = false;
                _response.ErrorMessage = new List<string>() { ex.ToString() }; ;
            }
            return _response;
        }

    }
}

