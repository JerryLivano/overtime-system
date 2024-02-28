namespace API.DTOs.Accounts
{
    public record AccountResponseDto(
        Guid Id,
        string Password,
        int Otp,
        DateTime Expired,
        bool IsUsed,
        bool IsActive,
        List<string> Roles);
}
