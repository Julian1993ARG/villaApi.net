﻿using AutoMapper;
using MagicVilla.Models;
using MagicVilla.Models.Dto;
using MagicVilla.Reposiory.IRepository;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Net;

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
        protected DefaultResponse _response;
        public VillaController(ILogger<VillaController> logger,  IMapper mapper, IVillaRepository villaRepo)
        {
            _logger = logger;
            _mapper = mapper;
            _villaRepository = villaRepo;
            _response = new();
        }


        // Peticiones HTTP
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<DefaultResponse>> GetVillas() {
            try
            {
                var villas = await _villaRepository.GetAll();
                _response.Data = villas;
                _response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                _response.Ok = false;
                _response.ErrorMessage = new List<string>() { ex.ToString() };
            }
            return _response; 
        }

        [HttpGet("id:int", Name = "GetVillaById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<DefaultResponse>> GetVillaById(int id)
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
                var villa = await _villaRepository.Get(v => v.Id == id);
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
        public async Task<ActionResult<DefaultResponse>> PostVilla([FromBody] VillaCreateDto villa)
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
                var existVilla = await _villaRepository.Get(v => v.Name.ToLower() == villa.Name.ToLower());
                if (existVilla != null)
                {
                    _response.Ok = false;
                    _response.ErrorMessage = new List<string>() { "Ya existe una villa con ese nombre" };
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return _response;
                }
                var newVilla = _mapper.Map<Villa>(villa);
                newVilla.CreateAt = DateTime.Now;
                newVilla.UpdateAt = DateTime.Now;
                await _villaRepository.Create(newVilla);
                _response.Data = newVilla;
                _response.StatusCode = HttpStatusCode.Created;

            }
            catch (Exception ex)
            {
                _response.Ok = false;
                _response.ErrorMessage = new List<string>() { ex.ToString() };
            }
            return CreatedAtRoute("GetVillaById", new { id= 0 }, _response);

        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<DefaultResponse> DeleteVillaById(int id)
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
                var villa = await _villaRepository.Get(v => v.Id == id);
                if (villa == null)
                {
                    _response.Ok = false;
                    _response.ErrorMessage = new List<string>() { "Villa no encontrada" };
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return _response;
                }
                var deleteVilla = _mapper.Map<Villa>(villa);
                deleteVilla.Id = id;
                await _villaRepository.Delete(deleteVilla);
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
        public async Task<DefaultResponse> PutVilla(int id, [FromBody] VillaUpdateDto villa)
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
                var existVilla = await _villaRepository.Get(v => v.Id == id, false);
                if (existVilla == null)
                {
                    _response.Ok = false;
                    _response.ErrorMessage = new List<string>() { "Villa no encontrada" };
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return _response;
                }
                var newVilla = _mapper.Map<Villa>(villa);
                newVilla.Id = id;
                await _villaRepository.Update(newVilla);
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

        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<DefaultResponse> PatchVilla(int id, JsonPatchDocument<Villa> patch)
        {
            if (patch == null)
            {
                _response.Ok = false;
                _response.ErrorMessage = new List<string>() { "Debe pasar un JsonPatchDocument" };
                _response.StatusCode = HttpStatusCode.BadRequest;
                return _response;
            }
            try
            {
                var villa = await _villaRepository.Get(v => v.Id == id);
                if (villa == null)
                {
                    _response.Ok = false;
                    _response.ErrorMessage = new List<string>() { "Villa no encontrada" };
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return _response;
                }
                if (!ModelState.IsValid)
                {
                    _response.Ok = false;
                    _response.ErrorMessage = new List<string>() { "Villa no válida" };
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return _response;
                }

                patch.ApplyTo(villa, ModelState);

                await _villaRepository.Update(villa);
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.Data = villa;
            }
            catch (Exception ex)
            {
                _response.Ok = false;
                _response.ErrorMessage = new List<string>() { ex.ToString() };
            }

            return _response;
        }

    }
}

