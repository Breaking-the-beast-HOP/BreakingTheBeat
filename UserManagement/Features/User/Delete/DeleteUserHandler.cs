using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserManagement.Domain.Entities;
using UserManagement.Infrastructure.Persistence;

namespace UserManagement.Features.User.Delete;

public class DeleteUserHandler : IRequestHandler<DeleteUserCommand>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly UserManagementDbContext _dbContext;

    public DeleteUserHandler(UserManager<ApplicationUser> userManager, UserManagementDbContext dbContext)
    {
        _userManager = userManager;
        _dbContext = dbContext;
    }

    public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        await using var transaction =
            await _dbContext.Database.BeginTransactionAsync(cancellationToken);

        var user = await _userManager.FindByIdAsync(request.UserId.ToString());

        if (user is null)
            throw new Exception("Brugeren blev ikke fundet.");

        var roles = await _userManager.GetRolesAsync(user);

        if (roles.Any())
        {
            var removeRolesResult = await _userManager.RemoveFromRolesAsync(user, roles);

            if (!removeRolesResult.Succeeded)
                throw new Exception(string.Join(", ", removeRolesResult.Errors.Select(e => e.Description)));
        }

        var deleteResult = await _userManager.DeleteAsync(user);

        if (!deleteResult.Succeeded)
            throw new Exception(string.Join(", ", deleteResult.Errors.Select(e => e.Description)));

        await transaction.CommitAsync(cancellationToken);
    }
}