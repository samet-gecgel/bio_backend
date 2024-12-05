namespace Bio.Domain.Entities
{
    public class JobCategory
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ICollection<JobPost> JobPosts { get; set; } = new List<JobPost>();
    }
}
