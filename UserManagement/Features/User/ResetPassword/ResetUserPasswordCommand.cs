using MediatR;

namespace UserManagement.Features.User.ResetPassword
{
    public record ResetUserPasswordCommand(Guid UserId, string NewPassword) : IRequest;
}
