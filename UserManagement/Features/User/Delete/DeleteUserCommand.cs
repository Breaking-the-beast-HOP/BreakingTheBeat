using MediatR;

namespace UserManagement.Features.User.Delete
{
    public record DeleteUserCommand(Guid UserId) : IRequest;
}