using Azure.Storage.Sas;

namespace RedShift.Application.Dtos.AzureStorage;

public sealed class SasUrlDto
{
    public string Container { get; set; }
    public string BlobPath { get; set; }
    public int ExpiresInMin { get; set; }
    public BlobContainerSasPermissions Permissions { get; set; }
}
