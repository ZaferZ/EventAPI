using System.Security.Claims;

namespace EventAPI.Helpers
{
    public class JwtContext : IJwtContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public JwtContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public Guid UserId =>
            _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier) is { } idClaim
                ? Guid.Parse(idClaim.Value)
                : Guid.Empty;
        public string? UserName =>
            _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Name)?.Value;
        public string? Role =>
            _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Role)?.Value;
        public bool IsAuthenticated=>
            _httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;

    }

}

