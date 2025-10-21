using Microsoft.Extensions.Logging;
using System.Security.Claims;


namespace GlobalConstants
{

    public interface IAuthUserInfoProvider
    {
        AppUserInfo GetUserInfo(ClaimsPrincipal user);
    }

    public class AuthUserInfoProvider : IAuthUserInfoProvider
    {
        private readonly ILogger<IAuthUserInfoProvider> _logger;

        public AuthUserInfoProvider(ILogger<IAuthUserInfoProvider> logger)
        {
            _logger = logger;
        }

        public AppUserInfo GetUserInfo(ClaimsPrincipal user)
        {
            try
            {
                var username = user.Claims.FirstOrDefault(c => c.Type == DefaultClaims.UserName)?.Value;
                var email = user.Claims.FirstOrDefault(c => c.Type == DefaultClaims.Email)?.Value;
                var claims = user.Claims;

                return new AppUserInfo
                {
                    Username = username,
                    Email = email,
                    Claims = claims
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user info from the UserClaims");
                throw new ArgumentNullException(nameof(GetUserInfo), "Failed to retrieve user information.");
            }
        }
    }

}
