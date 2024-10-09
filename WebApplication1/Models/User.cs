public class User
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public ICollection<Community> OwnedCommunities { get; set; }
    public ICollection<Community> SubscribedCommunities { get; set; }
}