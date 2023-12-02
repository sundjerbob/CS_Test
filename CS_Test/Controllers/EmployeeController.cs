using CS_Test.Services;
using Microsoft.AspNetCore.Mvc;

namespace CS_Test.Controllers
{
    // Controller responsible for handling employee-related views and interactions.
    // Injects the IEmployeeService interface for employee-related data retrieval.
    public class EmployeeController : Controller
    {
        // Reference to the employee service.
        private readonly IEmployeeService _employeeService;

        // Constructor for initializing the controller with the employee service.
        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService ?? throw new ArgumentNullException(nameof(employeeService));
        }

        // Action method for rendering the employee index view asynchronously.
        public async Task<IActionResult> Index()
        {
            // Retrieve the list of EmployeeDTOs asynchronously from the service.
            var listDTO = await _employeeService.GetEmployeesAsync();

            // Return the view with the retrieved list of EmployeeDTOs.
            return View(listDTO);
        }
    }

}
