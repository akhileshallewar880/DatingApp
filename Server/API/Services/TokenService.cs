using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Controllers;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace API;

public class TokenService : ITokenService
{
    private readonly SymmetricSecurityKey key;

    private readonly UserManager<AppUser> userManager1;

    public TokenService(IConfiguration config, UserManager<AppUser> userManager)
    {
        key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
        userManager1 = userManager;
    }
    public async Task<string> CreateToken(AppUser user)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)

        };

        var roles = await userManager1.GetRolesAsync(user);

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var creds = new SigningCredentials(this.key, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(7),
            SigningCredentials = creds
        };

        var tokenHandeler = new JwtSecurityTokenHandler();

        var token = tokenHandeler.CreateToken(tokenDescriptor);

        return tokenHandeler.WriteToken(token);


    }
}
