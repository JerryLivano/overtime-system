namespace API.DTOs.Overtimes;

public record OvertimeChangeRequestDto(
    Guid Id,
    string Name,
    string Comment);