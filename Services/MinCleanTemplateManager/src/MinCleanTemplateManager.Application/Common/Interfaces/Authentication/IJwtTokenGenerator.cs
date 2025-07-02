using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinCleanTemplateManager.Application.Common.Interfaces.Authentication
{
    public interface IJwtTokenGenerator
    {
        public string GenerateToken(string username, string role);
        public string GenerateRefreshToken();
        public string GenerateTokenFromRefreshToken(string refreshToken);
        public string GenerateToken(Guid userId, string username);
    }
}
