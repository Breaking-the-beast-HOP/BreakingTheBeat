using Gateway.Controllers.UserManagement;
using MediatR;
using Microsoft.AspNetCore.Identity;
using UserManagement.Domain.Entities;

namespace UserManagement.Features.User.Update;

public class UpdateUserHandler : IRequestHandler<UpdateUserCommand>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UpdateUserHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.Id.ToString());

        if (user is null)
        {
            throw new Exception("Bruger blev ikke fundet.");
        }

        user.Email = request.Email;
        user.UserName = request.UserName;

        var updateResult = await _userManager.UpdateAsync(user);

        if (!updateResult.Succeeded)
        {
            throw new Exception(
                string.Join(", ", updateResult.Errors.Select(x => x.Description)));
        }

        var currentRoles = await _userManager.GetRolesAsync(user);

        if (currentRoles.Any())
        {
            var removeRolesResult =
                await _userManager.RemoveFromRolesAsync(user, currentRoles);

            if (!removeRolesResult.Succeeded)
            {
                throw new Exception(
                    string.Join(", ", removeRolesResult.Errors.Select(x => x.Description)));
            }
        }

        var addRoleResult =
            await _userManager.AddToRoleAsync(user, request.Role);

        if (!addRoleResult.Succeeded)
        {
            throw new Exception(
                string.Join(", ", addRoleResult.Errors.Select(x => x.Description)));
        }
    }
}