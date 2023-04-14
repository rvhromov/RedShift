using MediatR;
using RedShift.Infrastructure.EF.ReadModels;

namespace RedShift.Infrastructure.QueryHandlers.Users.GetUsers;

public sealed record GetUsers() : IRequest<IQueryable<UserReadModel>>;