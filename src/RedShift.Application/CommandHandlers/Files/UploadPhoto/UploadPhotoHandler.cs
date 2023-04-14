using Azure.Storage.Sas;
using MediatR;
using Microsoft.Extensions.Configuration;
using RedShift.Application.Dtos.AzureStorage;
using RedShift.Application.Helpers;
using RedShift.Application.Options;
using RedShift.Application.Providers;
using RedShift.Application.Services;

namespace RedShift.Application.CommandHandlers.Files.UploadPhoto;

internal sealed class UploadPhotoHandler : IRequestHandler<UploadPhoto, UploadPhotoPayload>
{
    private readonly IIdentityAccessor _identity;
    private readonly IBlobProvider _blobProvider;
    private readonly UserPhotoBlobOptions _blobOptions;

    public UploadPhotoHandler(IIdentityAccessor identity, IBlobProvider blobProvider, IConfiguration configuration)
    {
        _identity = identity;
        _blobProvider = blobProvider;
        _blobOptions = configuration.GetOptions<UserPhotoBlobOptions>(nameof(UserPhotoBlobOptions));
    }

    public Task<UploadPhotoPayload> Handle(UploadPhoto request, CancellationToken cancellationToken)
    {
        var blobPath = $"{_identity.GetUserId()}/{request.fileName}";

        var sasUrlConfig = new SasUrlDto
        {
            Container = _blobOptions.Container,
            BlobPath = blobPath,
            ExpiresInMin = _blobOptions.SasExpirationInMin,
            Permissions = BlobContainerSasPermissions.Create
        };

        var uploadUrl = _blobProvider.GenerateSasUrl(sasUrlConfig);

        return Task.FromResult(new UploadPhotoPayload(uploadUrl));
    }
}
