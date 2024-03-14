namespace API.DTOs.Overtimes
{
    public record OvertimeRequestDto(
        Guid AccountId,
        DateTime Date,
        string Reason,
        int TotalHours);
}
