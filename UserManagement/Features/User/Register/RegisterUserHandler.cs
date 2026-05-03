using MediatR;
using Microsoft.AspNetCore.Identity;
using UserManagement.Domain.Entities;

namespace UserManagement.Features.User.Register
{
    public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, RegisterUserResult>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public RegisterUserHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<RegisterUserResult> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var user = new ApplicationUser
            {
                Id = Guid.NewGuid(),
                UserName = request.Email,
                Email = request.Email
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                return new RegisterUserResult
                {
                    Succeeded = false,
                    Errors = result.Errors.Select(e => TranslateIdentityError(e.Code)).ToList()
                };
            }

            return new RegisterUserResult
            {
                Succeeded = true,
                UserId = user.Id
            };
        }

        private static string TranslateIdentityError(string code)
        {
            return code switch
            {
                "PasswordTooShort" => "Adgangskoden er for kort.",
                "PasswordRequiresNonAlphanumeric" => "Adgangskoden skal indeholde mindst ét specialtegn.",
                "PasswordRequiresDigit" => "Adgangskoden skal indeholde mindst ét tal.",
                "PasswordRequiresLower" => "Adgangskoden skal indeholde mindst ét lille bogstav.",
                "PasswordRequiresUpper" => "Adgangskoden skal indeholde mindst ét stort bogstav.",
                "DuplicateUserName" => "Der findes allerede en bruger med denne email.",
                "DuplicateEmail" => "Der findes allerede en bruger med denne email.",
                "InvalidEmail" => "Email-adressen er ikke gyldig.",
                _ => "Brugeren kunne ikke oprettes."
            };
        }
    }
}