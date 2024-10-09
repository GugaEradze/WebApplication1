public class Post
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public int? CommunityId { get; set; }
    public Community Community { get; set; }
}