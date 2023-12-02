// ChartController.cs
using CS_Test.Services;
using Microsoft.AspNetCore.Mvc;

namespace CS_Test.Controllers
{
    public class ChartController : Controller
    {
        private readonly IChartService _chartService;
        private readonly IEmployeeService _employeeService;

        public ChartController(IChartService chartService, IEmployeeService employeeService)
        {
            _chartService = chartService;
            _employeeService = employeeService;
        }

        public async Task<IActionResult>? GetPieChart()
        {
            // Get employees from the employee service
            var employees = await _employeeService.GetEmployeesAsync();


            // Return the chart as an image
            return File(_chartService.GenerateEmployeePieChart(employees), "image/png", "employees_worktime_chart.png");

        }
    }
}
