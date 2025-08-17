namespace EventAPI.Models.DTOs
{
    public class ParticipantDto
    {
        public Guid Id { get; set; }   
        public string Username { get; set; } = string.Empty;
    }
}
