using HotChocolate.Authorization;
using MediatR;
using RedShift.Application.CommandHandlers.Files.UploadPhoto;

namespace RedShift.GraphQL.Mutations;

[ExtendObjectType(OperationTypeNames.Mutation)]
public sealed class FileMutations
{
    [Authorize]
    public async Task<UploadPhotoPayload> UploadPhotoAsync(
        UploadPhoto input,
        [Service] IMediator mediator, 
        CancellationToken cancellationToken)
    {
        return await mediator.Send(input, cancellationToken);
    }
}