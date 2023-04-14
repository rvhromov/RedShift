namespace RedShift.Infrastructure.Options;

public record SecurityOptions
{
    public int KeySize { get; init; }
    public int Interactions { get; init; }
}
