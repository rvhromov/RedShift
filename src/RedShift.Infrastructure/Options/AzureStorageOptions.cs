namespace RedShift.Infrastructure.Options;

public record AzureStorageOptions
{
    public string AccountName { get; init; }
    public string AccountKey { get; init; }
    public string BaseUrl { get; init; }
}
