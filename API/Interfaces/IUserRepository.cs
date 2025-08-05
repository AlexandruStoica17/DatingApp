using System;
using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces;

public interface IUserRepository
{


    void Update(AppUser user);
   
    Task<IEnumerable<AppUser>> GetUsersAsync(); //conventional when giving a task to finish with Async
    Task<AppUser?> GetUSerByIdAsync(int id);
    Task<AppUser?> GetUSerByUsernameAsync(string username);
    Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams);
    Task<MemberDto?> GetMemberAsync(string username);
    
}
