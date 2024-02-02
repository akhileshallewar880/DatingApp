using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API;

public class BuggyController : BaseApiController
{
    private readonly DataContext context1;
    public BuggyController (DataContext context) 
    {
        this.context1 = context;
    }

    [Authorize]
    [HttpGet("auth")]
    public ActionResult<string> GetResult()
    {
        return "secret text";
    }

    [HttpGet("not-found")]
    public ActionResult<AppUser> GetNotFound()
    {
        var thing = context1.Users.Find(-1);

        if(thing == null) return NotFound();

        return thing;
    }

    [HttpGet("server-error")]
    public ActionResult<string> GetServerError()
    {
        var thing = context1.Users.Find(-1);

        var thingReturn = thing.ToString();

        return thingReturn;
    }

    [HttpGet("bad-request")]
    public ActionResult<string> GetBadRequest()
    {
        return BadRequest("Bad Request");
    }
}
