using System.Security.Claims;

namespace GlobalConstants
{
    public class AppUserInfo
    {
        public string? Username { get; set; }
        public string? Email { get; set; }
        public IEnumerable<Claim>? Claims { get; set; }
    }


}
