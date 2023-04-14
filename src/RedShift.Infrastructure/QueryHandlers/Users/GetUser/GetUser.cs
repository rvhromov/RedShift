using MediatR;
using RedShift.Infrastructure.EF.ReadModels;

namespace RedShift.Infrastructure.QueryHandlers.Users.GetUser;

public sealed record GetUser(Guid Id) : IRequest<UserReadModel>;
