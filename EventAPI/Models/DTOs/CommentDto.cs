namespace EventAPI.Models.DTOs
{
    public class CommentDto
    {
            public int Id { get; set; }
            public string Content { get; set; } = string.Empty;
            public DateTime CreatedAt { get; set; }
            public DateTime? UpdatedAt { get; set; }
            public Guid UserId { get; set; }
    }
}
