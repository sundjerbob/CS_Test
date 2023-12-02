using CS_Test.DTOs;

namespace CS_Test.Services
{
    // Service interface responsible for handling employee-related operations.
    public interface IEmployeeService
    {

        // Defines a method to asynchronously retrieve a list of EmployeeDTOs.
        Task<List<EmployeeDTO>> GetEmployeesAsync();

    }
}
