using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Eventee.Api.Data;
using AutoMapper;
using Eventee.Api.Controllers.Dtos;
using Eventee.Api.Filters;
using Eventee.Api.Wrappers;
using Eventee.Api.Factories;

namespace Eventee.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly EventeeContext _context;

    private readonly IPagedResponseFactory _pagedResponseFactory;

    private readonly IMapper _mapper;

    public UsersController(EventeeContext context, IPagedResponseFactory pagedResponseFactory, IMapper mapper)
    {
        _context = context;

        _pagedResponseFactory = pagedResponseFactory;

        _mapper = mapper;
    }

    // GET: api/Users
    [HttpGet]
    public async Task<ActionResult<PagedResponse<UserDto>>> GetUsers([FromQuery] PaginationFilter paginationFilter)
    {
        var dataSlice = await _context.Users
            .Skip((paginationFilter.PageNumber - 1) * paginationFilter.Count)
            .Take(paginationFilter.Count)
            .ToArrayAsync();

        var dto = _mapper.Map<UserDto[]>(dataSlice);

        return Ok(_pagedResponseFactory.CreatePagedReponse(dto, paginationFilter, Request.Path.Value ?? ""));
    }

    // GET: api/Users/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Response<UserDto>>> GetUser(string id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user is null)
            return NotFound();

        var dto = _mapper.Map<UserDto>(user);

        return Ok(new Response<UserDto>(dto));
    }

    [HttpGet("{id}/hostings")]
    public async Task<ActionResult<PagedResponse<GetTogetherDto>>> GetUserAllHostings(string id, [FromQuery] PaginationFilter paginationFilter)
    {
        var user = await _context.Users.FindAsync(id);
        if (user is null)
            return NotFound(new Response<string>("User not found."));

        var dataSlice = await _context.GetTogethers
            .Include(e => e.Hoster)
            .Where(e => e.Hoster.Id == id)
            .Skip((paginationFilter.PageNumber - 1) * paginationFilter.Count)
            .Take(paginationFilter.Count)
            .ToArrayAsync();
        if (dataSlice is null)
            return NotFound();

        var dto = _mapper.Map<ICollection<GetTogetherDto>>(dataSlice);

        return Ok(_pagedResponseFactory.CreatePagedReponse(dto, paginationFilter, Request.Path.Value ?? ""));
    }

    [HttpGet("{userId}/hostings/{getTogetherId}")]
    public async Task<ActionResult<Response<GetTogetherDto>>> GetUserHostingsById(string userId, int getTogetherId)
    {
        var user = await _context.Users
            .Where(u => u.Id == userId)
            .Include(u => u.HostedGetTogethers)
            .FirstOrDefaultAsync();
        if (user is null)
            return NotFound(new Response<string>("User not found."));

        var getTogether = user.HostedGetTogethers
            .Where(e => e.Id == getTogetherId)
            .FirstOrDefault();
        if (getTogether is null)
            return NotFound(new Response<string>("User does not host this get together."));

        var dto = _mapper.Map<GetTogetherDto>(getTogether);

        return Ok(new Response<GetTogetherDto>(dto));
    }

    [HttpGet("{id}/subscriptions")]
    public async Task<ActionResult<PagedResponse<GetTogetherDto>>> GetUserAllSubscriptions(string id, [FromQuery] PaginationFilter paginationFilter)
    {
        var user = await _context.Users.FindAsync(id);
        if (user is null)
            return NotFound(new Response<string>("User not found."));

        var dataSlice = await _context.GetTogethers
            .Include(e => e.Subscribers)
            .Where(e => e.Subscribers
                .Any(s => s.Id == id))
            .Skip((paginationFilter.PageNumber - 1) * paginationFilter.Count)
            .Take(paginationFilter.Count)
            .ToArrayAsync();
        if (dataSlice is null)
            return NotFound();

        var dto = _mapper.Map<ICollection<GetTogetherDto>>(dataSlice);

        return Ok(_pagedResponseFactory.CreatePagedReponse(dto, paginationFilter, Request.Path.Value ?? ""));
    }

    [HttpGet("{userId}/subscriptions/{getTogetherId}")]
    public async Task<ActionResult<Response<GetTogetherDto>>> GetUserSubscriptionById(string userId, int getTogetherId)
    {
        var user = await _context.Users
            .Where(u => u.Id == userId)
            .Include(u => u.HostedGetTogethers)
            .FirstOrDefaultAsync();
        if (user is null)
            return NotFound(new Response<string>("User not found."));

        var getTogether = user.HostedGetTogethers
            .Where(e => e.Id == getTogetherId)
            .FirstOrDefault();
        if (getTogether is null)
            return NotFound(new Response<string>("User has not been subscribed to this get together."));

        var dto = _mapper.Map<GetTogetherDto>(getTogether);

        return Ok(new Response<GetTogetherDto>(dto));
    }
}
