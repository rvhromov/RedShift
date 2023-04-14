using Azure.Storage;
using Azure.Storage.Sas;
using Microsoft.Extensions.Configuration;
using RedShift.Application.Dtos.AzureStorage;
using RedShift.Application.Helpers;
using RedShift.Application.Providers;
using RedShift.Infrastructure.Options;

namespace RedShift.Infrastructure.Providers;

internal sealed class BlobProvider : IBlobProvider
{
    private readonly AzureStorageOptions _storageOptions;

    public BlobProvider(IConfiguration configuration)
    {
        _storageOptions = configuration.GetOptions<AzureStorageOptions>(nameof(AzureStorageOptions));
    }

    public string GenerateSasUrl(SasUrlDto sasUrlConfig)
    {
        var storageCreds = new StorageSharedKeyCredential(_storageOptions.AccountName, _storageOptions.AccountKey);
        var sasBuilder = new BlobSasBuilder
        {
            BlobName = sasUrlSetup.BlobPath,
            BlobContainerName = sasUrlSetup.Container,
            ExpiresOn = DateTime.UtcNow.AddMinutes(sasUrlSetup.ExpiresInMin)
        };

        sasBuilder.SetPermissions(sasUrlSetup.Permissions);

        var sasToken = sasBuilder
            .ToSasQueryParameters(storageCreds)
            .ToString();

        return $"{_storageOptions.BaseUrl}/{_storageOptions.AccountName}/{sasUrlSetup.Container}/{sasUrlSetup.BlobPath}?{sasToken}";
    }
}