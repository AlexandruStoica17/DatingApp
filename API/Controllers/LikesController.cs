using System;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper.Configuration.Conventions;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class LikesController(ILikesRepository likesRepository) : BaseApiController
{
    [HttpPost("{targetUserID:int}")]
    public async Task<ActionResult> ToggleLike(int targetUserID)
    {
        var sourceUserId = User.GetUserId();

        if (sourceUserId == targetUserID) return BadRequest("You cannot like yourself");

        var existingLike = await likesRepository.GetUserLike(sourceUserId, targetUserID);

        if (existingLike == null)
        {
            var like = new UserLike
            {
                SourceUserId = sourceUserId,
                TargetUserId = targetUserID
            };
            likesRepository.AddLike(like);
        }
        else
        {
            likesRepository.DeleteLike(existingLike);
        }

        if (await likesRepository.SaveChanges()) return Ok();

        return BadRequest("Failed to update like");
    }

    [HttpGet("list")]

    public async Task<ActionResult<IEnumerable<int>>> GetCurrentUserLikeIds()
    {
        return Ok(await likesRepository.GetCurrentUserLikeIds(User.GetUserId()));
    }

    [HttpGet]

    public async Task<ActionResult<IEnumerable<MemberDto>>> GetUserLikes([FromQuery]LikesParams likesParams)
    {
        likesParams.UserId = User.GetUserId();
        var users = await likesRepository.GetUserLikes(likesParams);
        Response.AddPaginationHeader(users);
        return Ok(users);
    }
    
}
