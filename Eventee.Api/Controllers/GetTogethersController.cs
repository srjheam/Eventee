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
                return Conflict(new Response<string>("GetTogether with the given Id already exists."));

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

        [HttpGet("{id}/hoster")]
        public async Task<ActionResult<Response<UserDto>>> GetGetTogetherHoster(int id)
        {
            var getTogether = await _context.GetTogethers
                .Where(e => e.Id == id)
                .Include(e => e.Hoster)
                .FirstOrDefaultAsync();
            if (getTogether is null)
                return NotFound();

            if (getTogether is null)
                return NotFound();

            var dto = _mapper.Map<UserDto>(getTogether.Hoster);

            return Ok(new Response<UserDto>(dto));
        }

        [HttpGet("{id}/subscribers")]
        public async Task<ActionResult<PagedResponse<UserDto>>> GetGetTogethersSubscribers(int id, [FromQuery] PaginationFilter paginationFilter)
        {
            var dataSlice = await _context.GetTogethers
                .Where(e => e.Id == id)
                .Include(e => e.Subscribers)
                .Select(e => e.Subscribers)
                .Skip((paginationFilter.PageNumber - 1) * paginationFilter.Count)
                .Take(paginationFilter.Count)
                .FirstOrDefaultAsync();
            if (dataSlice is null)
                return NotFound();

            var dto = _mapper.Map<ICollection<UserDto>>(dataSlice);

            return Ok(_pagedResponseFactory.CreatePagedReponse(dto, paginationFilter, Request.Path.Value ?? ""));
        }

        [HttpGet("{getTogetherId}/subscribers/{subscriberId}")]
        public async Task<ActionResult<Response<UserDto>>> GetGetTogetherSubscriber(int getTogetherId, string subscriberId)
        {
            var getTogether = await _context.GetTogethers
                .Where(u => u.Id == getTogetherId)
                .Include(u => u.Subscribers)
                .FirstOrDefaultAsync();
            if (getTogether is null)
                return NotFound(new Response<string>("Get Together not found."));
            
            var user = getTogether.Subscribers
                .Where(s => s.Id == subscriberId)
                .FirstOrDefault();
            if (user is null)
                return NotFound(new Response<string>("User not found."));

            var dto = _mapper.Map<UserDto>(user);

            return Ok(new Response<UserDto>(dto));
        }

        /// <summary>
        /// Subscribe a user to the get together
        /// </summary>
        [HttpPost("{getTogetherId}/subscribers/{subscriberId}")]
        public async Task<IActionResult> PostGetTogetherSubscriber(int getTogetherId, string subscriberId)
        {
            var getTogether = await _context.GetTogethers
                .Where(e => e.Id == getTogetherId)
                .Include(e => e.Hoster)
                .FirstOrDefaultAsync();
            if (getTogether is null)
                return NotFound(new Response<string>("Get Together not found."));

            if (getTogether.Hoster.Id == subscriberId)
                return Conflict(new Response<string>("Cannot subscribe a hoster to its own get together."));

            var user = await _context.Users
                .Where(u => u.Id == subscriberId)
                .Include(u => u.SubscribedGetTogethers)
                .FirstOrDefaultAsync();
            if (user is null)
                return NotFound(new Response<string>("User not found."));

            if (user.SubscribedGetTogethers
                .Where(e => e.Id == getTogetherId)
                .FirstOrDefault() is not null)
                return Conflict(new Response<string>("User is already subscribed to the get together."));

            user.SubscribedGetTogethers.Add(getTogether);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Unsubscribe a user to the get together
        /// </summary>
        [HttpDelete("{getTogetherId}/subscribers/{subscriberId}")]
        public async Task<IActionResult> DeleteGetTogetherSubscriber(int getTogetherId, string subscriberId)
        {
            var getTogether = await _context.GetTogethers
                .Where(e => e.Id == getTogetherId)
                .Include(e => e.Subscribers)
                .Include(e => e.Hoster)
                .FirstOrDefaultAsync();
            if (getTogether is null)
                return NotFound(new Response<string>("Get Together not found."));

            var user = await _context.Users
                .Where(u => u.Id == subscriberId)
                .Include(u => u.SubscribedGetTogethers)
                .FirstOrDefaultAsync();
            if (user is null)
                return NotFound(new Response<string>("User not found."));

            if (user.SubscribedGetTogethers
                .Where(e => e.Id == getTogetherId)
                .FirstOrDefault() is not null)
                return Conflict(new Response<string>("User was never subscribed to the get together."));

            user.SubscribedGetTogethers.Remove(getTogether);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
