
using CS_Test.DTOs;

namespace CS_Test.Services
{
    // Service interface responsible for generating pie chart .png resource 
    public interface IChartService
    {
        // Method that creates pie chart resource using a list of EmployeeDTOs as chart data model 
        byte[] GenerateEmployeePieChart(List<EmployeeDTO> employees);
    }
}
