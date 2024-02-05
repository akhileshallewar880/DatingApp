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
}
