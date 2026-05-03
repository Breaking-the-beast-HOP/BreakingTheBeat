using MediatR;
using Microsoft.AspNetCore.Identity;
using UserManagement.Domain.Entities;

namespace UserManagement.Features.User.Login
{
    public class LoginUserHandler : IRequestHandler<LoginUserCommand, LoginUserResult>
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public LoginUserHandler( SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task<LoginUserResult> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                return new LoginUserResult
                {
                    Succeeded = false,
                    Errors = new() { "Forkert email eller adgangskode." }
                };
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

            if (!result.Succeeded)
            {
                return new LoginUserResult
                {
                    Succeeded = false,
                    Errors = new() { "Forkert email eller adgangskode." }
                };
            }

           
            return new LoginUserResult
            {
                Succeeded = true,
                Token = "TEMP_TOKEN"
            };
        }
    }
}
