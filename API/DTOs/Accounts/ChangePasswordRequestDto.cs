namespace API.DTOs.Accounts
{
    public record ChangePasswordRequestDto(
        string Email,
        int Otp,
        string NewPassword,
        string ConfirmPassword);
}
