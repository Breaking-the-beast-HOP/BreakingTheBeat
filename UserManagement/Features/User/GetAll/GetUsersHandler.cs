using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserManagement.Domain.Entities;

namespace UserManagement.Features.User.GetAll
{
    public class GetUsersHandler : IRequestHandler<GetUsersQuery, List<UserOverviewDto>>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public GetUsersHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<List<UserOverviewDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _userManager.Users
                .OrderBy(u => u.UserName)
                .ToListAsync(cancellationToken);

            var result = new List<UserOverviewDto>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                result.Add(new UserOverviewDto
                {
                    Id = user.Id,
                    Email = user.Email ?? string.Empty,
                    UserName = user.UserName ?? string.Empty,
                    Role = roles.FirstOrDefault() ?? "-"
                });
            }

            return result;
        }
    }
}
