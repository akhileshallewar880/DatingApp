using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTO;

namespace API.Interfaces
{
    public interface IUserRepository
    {
        void Update(AppUser appUser);

        Task<bool> SaveAllAsync();

        Task<IEnumerable<AppUser>> GetAllUsersAsync();

        Task<AppUser> GetUserByIdAsync(int id);

        Task<AppUser> GetUserByUsernameAsync(string username);

        Task<IEnumerable<MemberDTO>> GetMembersAsync();

        Task<MemberDTO> GetMemberAsync(string usernaeme);
    }
}