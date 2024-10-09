using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class PostController : ControllerBase
{
    private readonly AppDbContext _context;

    public PostController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<ActionResult<Post>> CreatePost(Post post)
    {
        if (post.CommunityId.HasValue)
        {
            var community = await _context.Communities.FindAsync(post.CommunityId.Value);
            if (community == null)
                return BadRequest("Community not found");

            post.Community = community;
        }

        _context.Posts.Add(post);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(CreatePost), new { id = post.Id }, post);
    }
}