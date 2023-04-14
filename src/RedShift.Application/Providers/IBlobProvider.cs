using RedShift.Application.Dtos.AzureStorage;

namespace RedShift.Application.Providers;

public interface IBlobProvider
{
    string GenerateSasUrl(SasUrlDto sasUrlConfig);
}
