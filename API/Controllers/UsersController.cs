using System;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

// [ApiController]
// [Route("api/[controller]")]  // /api/users

public class UsersController(DataContext context) : BaseApiController
{

    [AllowAnonymous]
    [HttpGet]

    //sincron vs asincron analogie waiter restaurant
    //sincron = waiter takes order then goes to the chef and waits until done, intre timp rest customers don t get served
    //asincron = wwaiter takes order, goes to chef then takes more orders, when order completed waiter takes the food and delivers
    public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
    {
        var users = await context.Users.ToListAsync();

        return users; //puteam si Ok(users) pt ca tot ok e
    }

    [Authorize]
    [HttpGet("{id:int}")]  // /api/users/id(id adica 1, 2 etc ..)
    public async Task<ActionResult<AppUser>> GetUser(int id)
    {
        var user = await context.Users.FindAsync(id);
        if (User == null) return NotFound();

        return user; //puteam si Ok(users) pt ca tot ok e
    }
}
