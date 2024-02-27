namespace API.DTOs.Accounts
{
    public record AccountRequestDto(
        Guid Id,
        string Password,
        int? Otp = 0,
        bool? IsUsed = true,
        bool? IsActive = true);
}
