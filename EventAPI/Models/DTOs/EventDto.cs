namespace EventAPI.Models.DTOs
{
    public class EventDto
    {   
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Guid OwnerId { get; set; }
        public int HobbyId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Location { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public List<ParticipantDto> Participants { get; set; } = new();

        public List<TaskDto> Tasks { get; set; } = new();

        public List<CommentDto> Comments { get; set; } = new();
        public string Status { get; set; } = string.Empty;
    }
}
