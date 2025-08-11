namespace EventAPI.Models
{
    public class EventUpdateDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Guid OwnerId { get; set; }
        public int HobbyId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Location { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public List<UserDTO>? Participants { get; set; }
        public EventStatus Status { get; set; } = EventStatus.Scheduled;
    }
}
