namespace API.DTOs.Accounts;

public record AccountRoleResponseDto(
    Guid Id,
    Guid AccountId,
    Guid RoleId);