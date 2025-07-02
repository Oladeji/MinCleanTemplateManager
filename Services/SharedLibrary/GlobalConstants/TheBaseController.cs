using CQRSHelper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GlobalConstants
{

    [ApiController]

    public abstract class TheBaseController<T> : ControllerBase where T : TheBaseController<T>
    {
        protected ILogger<T> _logger;
        protected ISender _sender;

        public TheBaseController(ILogger<T> logger, ISender sender)
        {
            _logger = logger;
            _sender = sender;
        }
        protected AppUserInfo GetUserInfo()
        {
            try
            {
                var username = User.Claims.FirstOrDefault(c => c.Type == GlobalConstants.DefaultClaims.UserName)?.Value;
                var email = User.Claims.FirstOrDefault(c => c.Type == GlobalConstants.DefaultClaims.Email)?.Value;
                var Claims = User.Claims;

                return new AppUserInfo
                {
                    Username = username,
                    Email = email,
                    Claims = Claims
                };

            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Error getting user info from the UserClaims ");
                throw new ArgumentNullException(nameof(GetUserInfo));
            }

        }




    }
}
