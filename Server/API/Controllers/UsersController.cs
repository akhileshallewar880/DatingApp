﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[Authorize]
public class  UsersController : BaseApiController
{
    private readonly DataContext context;
    public UsersController(DataContext context)
    {
        this.context = context;
    }

    [AllowAnonymous]
    [HttpGet]  // api/users
    public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
    {
        var users = await this.context.Users.ToListAsync();

        return users;
    }

    [HttpGet("{id}")] //api/users/1
    public async Task<ActionResult<AppUser>> GetUser(int id)
    {
        var user = await this.context.Users.FindAsync(id);

        return user;
    }
}
