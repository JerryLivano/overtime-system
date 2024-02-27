namespace API.DTOs.Overtimes;

public record OvertimeDetailResponseDto(
    Guid Id,
    DateTime Date,
    string Reason,
    int TotalHours,
    string Status,
    IEnumerable<OvertimeRequestResponseDto> Requests);