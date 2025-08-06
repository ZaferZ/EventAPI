namespace EventAPI.Models
{
    public class EventCreateDTO
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Guid OwnerId { get; set; }
        public int HobbyId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Location { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public List<Guid> ParticipantIds { get; set; } =[];
        public EventStatus Status { get; set; } = EventStatus.Scheduled;
    }
}
