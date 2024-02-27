namespace API.DTOs.Accounts;

public record AddAccountRoleRequestDto(
    Guid AccountId,
    Guid RoleId);