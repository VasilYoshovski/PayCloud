using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace PayCloud.Services.Identity.Contracts
{
    public interface ITokenManager
    {
        string GenerateToken(string username, string role, string userId);

        ClaimsPrincipal GetPrincipal(string token);

        bool ValidateToken(string token);
    }
}
