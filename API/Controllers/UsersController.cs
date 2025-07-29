using System;
using System.Security.Claims;
using API.Data;
using API.Data.Migrations;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

// [ApiController]
// [Route("api/[controller]")]  // /api/users
[Authorize]
public class UsersController(IUserRepository userRepository, IMapper mapper, IPhotoService photoService) : BaseApiController
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
        // var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        // if (username == null) return BadRequest("No username found in token");

        // var user = await userRepository.GetUSerByUsernameAsync(username);
        var user = await userRepository.GetUSerByUsernameAsync(User.GetUsername());

        if (user == null) return BadRequest("Could not find user");

        mapper.Map(memberUpdateDto, user);

        if (await userRepository.SaveAllAsync()) return NoContent();

        return BadRequest("Failed to update the user");
    }

    [HttpPost("add-photo")]
    public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
    {
        var user = await userRepository.GetUSerByUsernameAsync(User.GetUsername());

        if (user == null) return BadRequest("Cannot update user");

        var result = await photoService.AddPhotoAsync(file);

        if (result.Error != null) return BadRequest(result.Error.Message);

        var photo = new Photo
        {
            Url = result.SecureUrl.AbsoluteUri,
            PublicId = result.PublicId
        };

        user.Photos.Add(photo);

        if (await userRepository.SaveAllAsync())
            return CreatedAtAction(nameof(GetUser), new { username = user.UserName }, mapper.Map<PhotoDto>(photo));

        return BadRequest("Problem adding photo");

    }

    [HttpPut("set-main-photo/{photoId:int}")] //we're updateing a resource so we're using httput

    public async Task<ActionResult> SetMainPhoto(int photoId)
    {
        var user = await userRepository.GetUSerByUsernameAsync(User.GetUsername());

        if (user == null) return BadRequest("Could not find user");

        var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

        if (photo == null || photo.IsMain) return BadRequest("Cannot use this as main photo");

        var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);

        if (currentMain != null) currentMain.IsMain = false;

        photo.IsMain = true;

        if (await userRepository.SaveAllAsync()) return NoContent();

        return BadRequest("Problem setting main photo");
    }

    [HttpDelete("delete-photo/{photoId:int}")]

    public async Task<ActionResult> DeletePhoto(int photoId)
    {
        var user = await userRepository.GetUSerByUsernameAsync(User.GetUsername());

        if (user == null) return BadRequest("User not found");

        var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

        if (photo == null || photo.IsMain) return BadRequest("This photo can not be deleted");

        if (photo.PublicId != null)
        {
            var result = await photoService.DeletePhotoAsync(photo.PublicId);
            if (result.Error != null) return BadRequest(result.Error.Message);
        }

        user.Photos.Remove(photo);
        if (await userRepository.SaveAllAsync()) return Ok();

        return BadRequest("Problem deleting photo");
    }

}
