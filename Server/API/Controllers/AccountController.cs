using System.Security.Cryptography;
using System.Text;
using API.DTO;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class AccountController : BaseApiController
{
    private readonly UserManager<AppUser> userManager1;

    private readonly ITokenService tokenService1;

    private readonly IMapper mapper1;

    public AccountController(UserManager<AppUser> userManager, ITokenService tokenService, IMapper mapper) 
    {
        this.userManager1 = userManager;
        this.tokenService1 = tokenService;
        this.mapper1 = mapper;
    }

    [HttpPost("register")] // POST : api/account/register
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
        if(await UserExists(registerDto.Username)) return BadRequest("Username already taken");

        var user = mapper1.Map<AppUser>(registerDto);

        user.UserName = registerDto.Username.ToLower();

        var result = await userManager1.CreateAsync(user, registerDto.Password);

        if (!result.Succeeded) return BadRequest(result.Errors);

        var roleResult = await userManager1.AddToRoleAsync(user, "Member");

        if (!roleResult.Succeeded) return BadRequest(result.Errors);

        return new UserDto 
        {
            Username = user.UserName,
            Token = await tokenService1.CreateToken(user),
            KnownAs = user.KnownAs,
            Gender = user.Gender
        };
    }

    private async Task<bool> UserExists(string username)
    {
        return await userManager1.Users.AnyAsync(x => x.UserName == username.ToLower());
    }

    [HttpPost("login")] // POST : api/account/login
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        var user = await userManager1.Users
                  .Include(p => p.Photos)
                  .SingleOrDefaultAsync(x => x.UserName == loginDto.Username.ToLower());

        if(user == null) return Unauthorized("Invalid Username");

        var result = await userManager1.CheckPasswordAsync(user, loginDto.Password);

        if (!result) return Unauthorized("Invalid Password");

        return new UserDto
        {
            Username = user.UserName,
            Token = await tokenService1.CreateToken(user),
            PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
            KnownAs = user.KnownAs,
            Gender = user.Gender
        };
    }
}
