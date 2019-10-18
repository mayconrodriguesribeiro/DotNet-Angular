using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProAgil.Domain;
using ProAgil.Repository;
using AutoMapper;
using System.Collections.Generic;
using ProAgilApi.Dtos;

namespace ProAgilApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventoController : ControllerBase
    {
        private readonly IProAgilRepository _rep;
        private readonly IMapper _mapper;

        public EventoController(IProAgilRepository rep, IMapper mapper)
        {
            _mapper = mapper;
            _rep = rep;
        }
        // GET 
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                 var eventos = await _rep.GetAllEventoAsync(true);   
                 var results = _mapper.Map<IEnumerable<EventoDto>>(eventos);
                 return Ok(results);
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco Dados Falhou");
            }
    
        }

        // GET 
        [HttpGet("{EventoId}")]
        public async Task<IActionResult> Get(int EventoId)
        {
            try
            {
                 var evento = await _rep.GetEventoAsyncById(EventoId, true); 
                 var results = _mapper.Map<EventoDto>(evento);  
                 return Ok(results);
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco Dados Falhou");
            }
        }

        [HttpGet("getByTema/{tema}")]
        public async Task<IActionResult> Get(string tema)
        {
            try
            {
                 var evento = await _rep.GetAllEventoAsyncByTema(tema, true);
                 var results = _mapper.Map<EventoDto>(evento);  
                 return Ok(results);
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco Dados Falhou");
            }
        }

        // POST 
        [HttpPost]
        public async Task<IActionResult> Post(EventoDto model)
        {
            try
            {
                var evento = _mapper.Map<Evento>(model);

                _rep.Add(model);

                if(await _rep.SaveChangesAsync())
                {
                    return Created($"/api/evento/{model.Id}", _mapper.Map<EventoDto>(evento));
                }
            }
            catch (System.Exception)
            {
                
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco Dados Falhou");
            }

            return BadRequest();
        }

        // PUT api/values/5
        [HttpPut("{EventoId}")]
        public async Task<IActionResult> Put(int EventoId, EventoDto model)
        {
            try
            {
                var evento = await _rep.GetEventoAsyncById(EventoId, false);
                
                if(evento == null) return NotFound();

                _mapper.Map(model, evento);
                
                _rep.Update(evento);

                if(await _rep.SaveChangesAsync())
                {
                    return Created($"/api/evento/{model.Id}", _mapper.Map<EventoDto>(model));
                }
            }
            catch (System.Exception)
            {
                
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco Dados Falhou");
            }

            return BadRequest();
        }

        // DELETE api/values/5
        [HttpDelete("{EventoId}")]
        public async Task<IActionResult> Delete(int EventoId)
        {
            try
            {
                var evento = await _rep.GetEventoAsyncById(EventoId, false);
                
                if(evento == null) return NotFound();

                _rep.Delete(evento);

                if(await _rep.SaveChangesAsync())
                {
                    return Ok();
                }
            }
            catch (System.Exception)
            {
                
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco Dados Falhou");
            }

            return BadRequest();
        }

    }
}
