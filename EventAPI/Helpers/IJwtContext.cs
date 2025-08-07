namespace EventAPI.Helpers
{
    public interface IJwtContext
    {
        public Guid UserId { get; }
        public string? UserName { get; }
        public string? Role { get; }
        bool IsAuthenticated { get; }


    }
}
