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
    public async Task<ActionResult<IEnumerable<Community>>> GetCommunities()
    {
        return await _context.Communities.Include(c => c.Owner).ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<Community>> CreateCommunity(Community community)
    {
        _context.Communities.Add(community);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetCommunities), new { id = community.Id }, community);
    }
}