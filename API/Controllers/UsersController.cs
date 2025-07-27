using System;
using System.Security.Claims;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

// [ApiController]
// [Route("api/[controller]")]  // /api/users
[Authorize]
public class UsersController(IUserRepository userRepository, IMapper mapper) : BaseApiController
{

    // [AllowAnonymous]
    [HttpGet]

    //sincron vs asincron analogie waiter restaurant
    //sincron = waiter takes order then goes to the chef and waits until done, intre timp rest customers don t get served
    //asincron = wwaiter takes order, goes to chef then takes more orders, when order completed waiter takes the food and delivers
    public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
    {
        var users = await userRepository.GetMembersAsync();

        return Ok(users);
    }

    // [Authorize]
    [HttpGet("{username}")]  // /api/users/id(id adica 1, 2 etc ..) inainte era [HttpGet("{id:int}")] 
    public async Task<ActionResult<MemberDto>> GetUser(string username)
    {
        var user = await userRepository.GetMemberAsync(username);
        if (user == null) return NotFound();

        return user;
    }

    [HttpPut]
    public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
    {
        var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (username == null) return BadRequest("No username found in token");

        var user = await userRepository.GetUSerByUsernameAsync(username);

        if (user == null) return BadRequest("Could not find user");

        mapper.Map(memberUpdateDto, user);
    
        if (await userRepository.SaveAllAsync()) return NoContent();

        return BadRequest("Failed to update the user");
    }
}
