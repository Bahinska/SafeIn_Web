using SafeIn_mvs_test.Models;
using System.IdentityModel.Tokens.Jwt;

namespace SafeIn_mvs_test.Helper
{
    public class Helper
    {
        public static UserInfo GetTokenInfo(string token)
        {
            var tokenInfo = new Dictionary<string, string>();
            var handler = new JwtSecurityTokenHandler();
            var tokenData = handler.ReadJwtToken(token);
            var claims = tokenData.Claims.ToList();
            foreach (var claim in claims)
            {
                tokenInfo.Add(claim.Type, claim.Value);
            }
            var info = new UserInfo()
            {
                email = tokenInfo["Email"],
                userName = tokenInfo["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"],
                role = tokenInfo["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"],
                company = tokenInfo["Company"]
            };
            return info;
        }
    }
}
