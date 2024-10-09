using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly AppDbContext _context;

    public UserController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost("{userId}/JoinCommunity/{communityId}")]
    public async Task<IActionResult> JoinCommunity(int userId, int communityId)
    {
        var user = await _context.Users.Include(u => u.SubscribedCommunities)
                                       .FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null)
            return NotFound();

        var community = await _context.Communities.FindAsync(communityId);
        if (community == null)
            return NotFound();

        user.SubscribedCommunities.Add(community);
        await _context.SaveChangesAsync();
        return Ok();
    }
}