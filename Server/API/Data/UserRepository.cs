using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTO;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext context1;

        private readonly IMapper mapper1;

        public UserRepository(DataContext context, IMapper mapper)
        {
            context1 = context;
            mapper1 = mapper;
        }
        public async Task<IEnumerable<AppUser>> GetAllUsersAsync()
        {
            return await context1.Users
                        .Include(p => p.Photos)
                        .ToListAsync();
        }

        public Task<MemberDTO> GetMemberAsync(string usernaeme)
        {
            return context1.Users
                   .Where(x => x.UserName == usernaeme)
                   .ProjectTo<MemberDTO>(mapper1.ConfigurationProvider)
                   .SingleOrDefaultAsync();
        }

        public async Task<PagedList<MemberDTO>> GetMembersAsync(UserParams userParams)
        {
            var query = context1.Users.AsQueryable();

            query = query.Where(u => u.UserName != userParams.CurrentUsername);
            query = query.Where(u => u.Gender == userParams.Gender);

            var minDob = DateOnly.FromDateTime(DateTime.Today.AddYears(-userParams.MaxAge - 1));
            var maxDob = DateOnly.FromDateTime(DateTime.Today.AddYears(-userParams.MinAge));

            query = query.Where(u => u.DateOfBirth >= minDob && u.DateOfBirth <= maxDob);

            query = userParams.OrderBy switch
            {
                "created" => query.OrderByDescending(u => u.Created),
                _ => query.OrderByDescending(u => u.LastActive)
            };

            return await PagedList<MemberDTO>.CreateAsync(
                query.AsNoTracking().ProjectTo<MemberDTO>(mapper1.ConfigurationProvider),
                userParams.PageNumber,
                userParams.PageSize
            );
        }

        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await context1.Users.FindAsync(id);
        }

        public async Task<AppUser> GetUserByUsernameAsync(string username)
        {
            return await context1.Users
                        .Include(p => p.Photos)
                        .SingleOrDefaultAsync(x => x.UserName == username);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await context1.SaveChangesAsync() > 0;
        }

        public void Update(AppUser appUser)
        {
            context1.Entry(appUser).State = EntityState.Modified;
        }
    }
}