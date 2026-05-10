using global::UserManagement.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace UserManagement.Features.User.ResetPassword 
{ 

    public class ResetUserPasswordHandler : IRequestHandler<ResetUserPasswordCommand>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ResetUserPasswordHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task Handle(ResetUserPasswordCommand request,CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());

            if (user is null)
            {
                throw new Exception("Brugeren blev ikke fundet.");
            }

            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            var result = await _userManager.ResetPasswordAsync(
                user,
                resetToken,
                request.NewPassword);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(error => error.Description));
                throw new Exception(errors);
            }
        }
    }
}
