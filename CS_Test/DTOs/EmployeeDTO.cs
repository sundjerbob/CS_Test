namespace CS_Test.DTOs
{
    // Data Transfer Object (DTO) representing an employee for client-side presentation purposes.
    public class EmployeeDTO
    {
        // String value of a full name of the employee.
        public string? FullName { get; set; }

        // Total number of hours the employee has worked.
        public long TotalWorkHours { get; set; }
    }
}
