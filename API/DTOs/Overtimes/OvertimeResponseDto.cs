namespace API.DTOs.Overtimes
{
    public record OvertimeResponseDto(
        Guid Id,
        DateTime Date,
        string Reason,
        int TotalHours,
        string Status,
        string Document);
}
