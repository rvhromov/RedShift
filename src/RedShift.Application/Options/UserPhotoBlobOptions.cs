namespace RedShift.Application.Options;

public record UserPhotoBlobOptions
{
    public string Container { get; init; }
    public int SasExpirationInMin { get; init; }
}
