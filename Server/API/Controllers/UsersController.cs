using System.Security.Claims;
using API.DTO;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[Authorize]
public class  UsersController : BaseApiController
{
    private readonly IUserRepository userRepository1;

    private readonly IMapper mapper1;
    public UsersController(IUserRepository  userRepository, IMapper mapper)
    {
        userRepository1 = userRepository;

        mapper1 = mapper;
    }

    [AllowAnonymous]
    [HttpGet]  // api/users
    public async Task<ActionResult<IEnumerable<MemberDTO>>> GetUsers()
    {
        var users = await userRepository1.GetMembersAsync();

        return Ok(users);
    }

    [HttpGet("{username}")] //api/users/1
    public async Task<ActionResult<MemberDTO>> GetUser(string username)
    {
        return await userRepository1.GetMemberAsync(username);

    }

    [HttpPut]
    public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
    {
        var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var user = await userRepository1.GetUserByUsernameAsync(username);

        if(user == null) return NotFound();

        mapper1.Map(memberUpdateDto, user);

        if(await userRepository1.SaveAllAsync()) return NoContent();

        return BadRequest("Failed to update user");

    }
}
