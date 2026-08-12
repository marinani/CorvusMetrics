namespace Corvus.Application.Dtos;

public sealed record AcquisitionChannelDto(
    Guid Id,
    string Name,
    string Color,
    bool IsActive,
    DateTime CreatedAtUtc,
    DateTime? UpdatedAtUtc);