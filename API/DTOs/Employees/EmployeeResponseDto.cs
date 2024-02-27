namespace API.DTOs.Employees
{
    public record EmployeeResponseDto(
        Guid Id,
        string Nik,
        string FirstName,
        string LastName,
        DateTime JoinedDate,
        int Salary,
        string Email,
        string Position,
        string Department,
        Guid? ManagerId);
}
