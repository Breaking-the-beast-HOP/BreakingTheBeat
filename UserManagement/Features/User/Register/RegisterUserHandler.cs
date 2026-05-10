using MediatR;
using Microsoft.AspNetCore.Identity;
using UserManagement.Domain.Entities;
using UserManagement.Infrastructure.Persistence;

namespace UserManagement.Features.User.Register
{
    public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, RegisterUserResult>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly UserManagementDbContext _userContext;

        public RegisterUserHandler(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<Guid>> roleManager, UserManagementDbContext dbContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _userContext = dbContext;
        }

        public async Task<RegisterUserResult> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            await using var transaction = await _userContext.Database.BeginTransactionAsync(cancellationToken);

            var user = new ApplicationUser
            {
                Id = Guid.NewGuid(),
                UserName = request.username,
                Email = request.Email
            };

            var result = await _userManager.CreateAsync(user, request.Password); //password valideres og hashes automatisk af Identity

            if (!result.Succeeded)
            {
                return new RegisterUserResult
                {
                    Succeeded = false,
                    Errors = result.Errors.Select(e => TranslateIdentityError(e.Code)).ToList()
                };
            }

            var roleExists = await _roleManager.RoleExistsAsync(request.roleName);

            if (!roleExists)
            {
                return new RegisterUserResult
                {
                    Succeeded = false,
                    Errors = ["Rollen findes ikke."]
                };
            }

            var addRoleResult = await _userManager.AddToRoleAsync(user, request.roleName);

            if (!addRoleResult.Succeeded)
            {
                return new RegisterUserResult
                {
                    Succeeded = false,
                    Errors = addRoleResult.Errors
                        .Select(e => TranslateIdentityError(e.Code))
                        .ToList()
                };
            }

            await transaction.CommitAsync(cancellationToken);

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