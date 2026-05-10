using MediatR;

namespace UserManagement.Features.User.Register
{
    public record RegisterUserCommand(string Email, string username, string Password, string roleName) : IRequest<RegisterUserResult>;

    public class RegisterUserResult
    {
        public bool Succeeded { get; set; }
        public Guid? UserId { get; set; }
        public List<string> Errors { get; set; } = new();
    }
}

