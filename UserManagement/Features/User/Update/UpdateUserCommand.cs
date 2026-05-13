using MediatR;

namespace Gateway.Controllers.UserManagement
{
    public record UpdateUserCommand(Guid Id, string Email, string UserName, string Role) : IRequest;
}