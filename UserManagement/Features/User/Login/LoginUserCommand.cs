using MediatR;

namespace UserManagement.Features.User.Login
{
    public record LoginUserCommand( string Email, string Password ) : IRequest<LoginUserResult>;

    public class LoginUserResult
    {
        public bool Succeeded { get; set; }
        public string? Token { get; set; }
        public List<string> Errors { get; set; } = new();
    }
}