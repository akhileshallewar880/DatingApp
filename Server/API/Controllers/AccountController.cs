using System.Security.Cryptography;
using System.Text;
using API.DTO;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class AccountController : BaseApiController
{
    private readonly DataContext context;

    private readonly ITokenService tokenService1;

    private readonly IMapper mapper1;

    public AccountController(DataContext context, ITokenService tokenService, IMapper mapper) 
    {
        this.context = context;
        this.tokenService1 = tokenService;
        this.mapper1 = mapper;
    }

    [HttpPost("register")] // POST : api/account/register
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
        if(await UserExists(registerDto.Username)) return BadRequest("Username already taken");

        var user = mapper1.Map<AppUser>(registerDto);

        using var hmac = new HMACSHA512();

        
      
            user.UserName = registerDto.Username.ToLower();
            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password));
            user.PasswordSalt = hmac.Key;


        context.Users.Add(user);

        await context.SaveChangesAsync();

        return new UserDto 
        {
            Username = user.UserName,
            Token = tokenService1.CreateToken(user),
            KnownAs = user.KnownAs
        };
    }

    private async Task<bool> UserExists(string username)
    {
        return await context.Users.AnyAsync(x => x.UserName == username.ToLower());
    }

    [HttpPost("login")] // POST : api/account/login
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        var user = await context.Users
                  .Include(p => p.Photos)
                  .SingleOrDefaultAsync(x => x.UserName == loginDto.Username.ToLower());

        if(user == null) return Unauthorized("Invalid Username");

        using var hmac = new HMACSHA512(user.PasswordSalt);

        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

        for(int i=0; i < computedHash.Length; i++)
        {
            if(user.PasswordHash[i] != computedHash[i]) return Unauthorized("Invalid Password");
        }

        return new UserDto
        {
            Username = user.UserName,
            Token = tokenService1.CreateToken(user),
            PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
            KnownAs = user.KnownAs
        };
    }
}
