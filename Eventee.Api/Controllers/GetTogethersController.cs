using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Eventee.Api.Data;
using Eventee.Api.Models;
using Eventee.Api.Controllers.Dtos;
using Eventee.Api.Wrappers;
using Eventee.Api.Filters;
using Eventee.Api.Factories;
using AutoMapper;

namespace Eventee.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetTogethersController : ControllerBase
    {
        private readonly EventeeContext _context;

        private readonly IPagedResponseFactory _pagedResponseFactory;

        private readonly IMapper _mapper;

        public GetTogethersController(EventeeContext context, IPagedResponseFactory pagedResponseFactory, IMapper mapper)
        {
            _context = context;
            _pagedResponseFactory = pagedResponseFactory;
            _mapper = mapper;
        }

        // GET: api/GetTogethers
        [HttpGet]
        public async Task<ActionResult<PagedResponse<GetTogetherDto>>> GetGetTogethers([FromQuery] PaginationFilter paginationFilter)
        {
            var dataSlice = await _context.GetTogethers
                .Skip((paginationFilter.PageNumber - 1) * paginationFilter.Count)
                .Take(paginationFilter.Count)
                .ToArrayAsync();

            var dto = _mapper.Map<GetTogetherDto[]>(dataSlice);

            return Ok(_pagedResponseFactory.CreatePagedReponse(dto, paginationFilter, Request.Path.Value ?? ""));
        }

        // GET: api/GetTogethers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Response<GetTogetherDto>>> GetGetTogether(int id)
        {
            var getTogether = await _context.GetTogethers.FindAsync(id);

            if (getTogether == null)
                return NotFound();
            
            var dto = _mapper.Map<GetTogetherDto>(getTogether);

            return Ok(new Response<GetTogetherDto>(dto));
        }

        // POST: api/GetTogethers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Response<GetTogetherDto>>> PostGetTogether(GetTogetherDto getTogetherDto)
        {
            var found = await _context.GetTogethers.FindAsync(getTogetherDto.Id);
            if (found is not null)
                return BadRequest(new Response<string>("GetTogether with the given Id already exists.");

            var model = _mapper.Map<GetTogether>(getTogetherDto);

            _context.GetTogethers.Add(model);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetGetTogether), new { id = getTogetherDto.Id }, getTogetherDto);
        }

        // DELETE: api/GetTogethers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGetTogether(int id)
        {
            var getTogether = await _context.GetTogethers.FindAsync(id);
            if (getTogether is null)
                return NotFound();

            _context.GetTogethers.Remove(getTogether);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
