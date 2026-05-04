using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UserManagement.Domain.Entities;

namespace UserManagement.Infrastructure.Persistence
{
    public class UserManagementDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public UserManagementDbContext(DbContextOptions<UserManagementDbContext> options) : base(options)
        {
        }
    }
}