using MediatR;

namespace RedShift.Application.CommandHandlers.Files.UploadPhoto;

public sealed record UploadPhoto(string fileName) : IRequest<UploadPhotoPayload>;
