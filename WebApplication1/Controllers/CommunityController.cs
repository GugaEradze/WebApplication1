using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class CommunityController : ControllerBase
{
    private readonly AppDbContext _context;

    public CommunityController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetCommunities(
    int pageNumber = 1,
    int pageSize = 10,
    bool? isAscending = true,
    string? sortKey = "id",
    string? searchKey = null)
    {
        if (pageSize > 50) return BadRequest("Page size cannot exceed 50.");

        var query = _context.Communities.AsQueryable();

        if (!string.IsNullOrEmpty(searchKey))
        {
            query = query.Where(c => c.Name.Contains(searchKey) || c.Description.Contains(searchKey));
        }

        query = sortKey switch
        {
            "createdAt" => isAscending.GetValueOrDefault(true)
                ? query.OrderBy(c => c.CreatedAt)
                : query.OrderByDescending(c => c.CreatedAt),
            "postsCount" => isAscending.GetValueOrDefault(true)
                ? query.OrderBy(c => c.Posts.Count)
                : query.OrderByDescending(c => c.Posts.Count),
            "subscribersCount" => isAscending.GetValueOrDefault(true)
                ? query.OrderBy(c => c.UserSubscribers.Count)
                : query.OrderByDescending(c => c.UserSubscribers.Count),
            _ => isAscending.GetValueOrDefault(true)
                ? query.OrderBy(c => c.Id)
                : query.OrderByDescending(c => c.Id)
        };

        var totalItems = await query.CountAsync();
        var communities = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var response = new
        {
            TotalItems = totalItems,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize),
            Data = communities
        };

        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<Community>> CreateCommunity(Community community)
    {
        _context.Communities.Add(community);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetCommunities), new { id = community.Id }, community);
    }
}