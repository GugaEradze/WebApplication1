using Microsoft.Extensions.Hosting;

public class Community
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public ICollection<Post> Posts { get; set; } = new List<Post>();
    public ICollection<User> UserSubscribers { get; set; } = new List<User>();
}