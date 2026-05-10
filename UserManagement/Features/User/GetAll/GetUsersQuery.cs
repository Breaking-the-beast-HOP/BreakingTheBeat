using MediatR;

namespace UserManagement.Features.User.GetAll
{
    public record GetUsersQuery : IRequest<List<UserOverviewDto>>;
}
