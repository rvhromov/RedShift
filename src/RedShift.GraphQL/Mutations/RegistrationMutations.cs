using MediatR;
using RedShift.Application.CommandHandlers.Registration.SignIn;
using RedShift.Application.CommandHandlers.Registration.SignUp;
using RedShift.Domain.Entities;

namespace RedShift.GraphQL.Mutations;

[ExtendObjectType(OperationTypeNames.Mutation)]
public sealed class RegistrationMutations
{
    public async Task<User> SignUpAsync(SignUp input, [Service] IMediator mediator, CancellationToken cancellationToken)
    {
        return await mediator.Send(input, cancellationToken);
    }

    public async Task<SignInPayload> SignInAsync(SignIn input, [Service] IMediator mediator, CancellationToken cancellationToken)
    {
        return await mediator.Send(input, cancellationToken);
    }
}