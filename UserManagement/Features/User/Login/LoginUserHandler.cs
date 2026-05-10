using MediatR;
using Microsoft.AspNetCore.Identity;
using UserManagement.Domain.Entities;
using UserManagement.Infrastructure.Auth;

namespace UserManagement.Features.User.Login
{
    public class LoginUserHandler : IRequestHandler<LoginUserCommand, LoginUserResult>
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtService _jwtService;

        public LoginUserHandler(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, JwtService jwtService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _jwtService = jwtService;
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

            var roles = await _userManager.GetRolesAsync(user);

            var token = _jwtService.GenerateToken(user,roles);

            return new LoginUserResult
            {
                Succeeded = true,
                Token = token
            };
        }
    }
}
