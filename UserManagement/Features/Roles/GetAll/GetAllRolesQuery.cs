using MediatR;

namespace UserManagement.Features.Roles.GetAll
{
    public record GetRolesQuery : IRequest<List<RoleDto>>;
}
