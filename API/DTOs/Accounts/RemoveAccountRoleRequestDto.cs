namespace API.DTOs.Accounts;

public record RemoveAccountRoleRequestDto(
    Guid AccountId,
    Guid RoleId);