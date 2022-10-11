using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIIII.DTOs;
using APIIII.Entities;

namespace APIIII.Interfaces
{
    public interface IUserRepository
    {
        void Update(AppUser user);
        Task<bool> SaveAllAsync();
        Task<IEnumerable<AppUser>> GetUsersAsync();
        Task<AppUser> GetUserByIdAsync(int id);
        Task<AppUser> GetUserByUsernameAsync(string username);
        Task<MemberDto> GetMemberAsync(string username);
        Task<IEnumerable<MemberDto>> GetMembersAsync();
    }
}