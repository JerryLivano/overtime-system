namespace API.DTOs.Overtimes;

public record OvertimeRequestResponseDto(
    DateTime Timestamp,
    string Status,
    string Comment);