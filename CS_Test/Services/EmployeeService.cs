using CS_Test.DTOs;
using CS_Test.Models;
using CS_Test.Repositories;

namespace CS_Test.Services
{
    // Service class responsible for handling business logic related to employee data.
    public class EmployeeService : IEmployeeService
    {
        // Data repository.
        private readonly IShiftCheckInRegistry _shiftCheckInRegistry;



        // Constructor to inject dependencies.
        public EmployeeService(IShiftCheckInRegistry shiftCheckInRegistry)
        {
            // Throw an ArgumentNullException if constructor argument is null, ensuring the required dependency is provided.
            _shiftCheckInRegistry = shiftCheckInRegistry ?? throw new ArgumentNullException(nameof(shiftCheckInRegistry));
        }



        // Retrieve a list of employees with their total work hours.
        public async Task<List<EmployeeDTO>> GetEmployeesAsync()
        {
            // Get work shift entries from the repository
            List<WorkShiftEntry> shiftEntries = await _shiftCheckInRegistry.GetAllShiftEntries();

            // Calculate total work time for each employee
            Dictionary<String, long> employeesTotalWorkTime = CalculateEmployeesWorkTime(shiftEntries);

            // Build a list of EmployeeDTO objects
            List<EmployeeDTO> employees = BuildEmployeesDTOList(employeesTotalWorkTime);

            // Order the list by total work hours
            employees = employees.OrderBy(employee => employee.TotalWorkHours).ToList();

            return employees;
        }



        // Build a list of EmployeeDTO objects.
        private static List<EmployeeDTO> BuildEmployeesDTOList(Dictionary<String, long> employeesTotalWorkTime)
        {
            List<EmployeeDTO> employees = new();

            // Iterate through each employee and create an EmployeeDTO object
            foreach (var employee in employeesTotalWorkTime.Keys)
            {
                employees.Add(
                    new EmployeeDTO
                    {
                        // Prettify employee name
                        FullName = CapitalizeFirstLetters(employee),
                        
                        // Calculate the whole number of hours ("full" hours) out of the sum of minutes
                        TotalWorkHours = employeesTotalWorkTime[employee] / 60
                    }
                );
            }

            return employees;
        }



        // Calculate total work hours for each employee.
        private static Dictionary<String, long> CalculateEmployeesWorkTime(List<WorkShiftEntry> checkedInShifts)
        {
            Dictionary<String, long> employeesTotalWorkTime = [];

            // Iterate through each work shift entry
            foreach (WorkShiftEntry shift in checkedInShifts)
            {
                // Skip invalid entries with no name or defined DeletedOn attribute
                if (string.IsNullOrEmpty(shift.EmployeeName) || shift.DeletedOn.HasValue)
                {
                    continue;
                }

                // Calculate work shift duration in minutes
                var workShiftMinutes = CalculateTimeWindowInMinutes(shift.StarTimeUtc, shift.EndTimeUtc);

                // Perform Normalization of string format for employees names to represent their semantic values, by ignoring lead/trail white-spaces and letter capitalization.
                // Result of this action is that the name: "Nina Petrov" and name: " nina Petrov " is equivalent in terms of referring to the same employee. 
                var employeeName = shift.EmployeeName.Trim().ToLower();

                // Update total work hours for each employee
                if (!employeesTotalWorkTime.ContainsKey(employeeName))
                {
                    employeesTotalWorkTime[employeeName] = workShiftMinutes;
                }
                else
                {
                    employeesTotalWorkTime[employeeName] += workShiftMinutes;
                }
            }

            return employeesTotalWorkTime;
        }



        // Helper method to calculate the time window duration in minutes
        private static long CalculateTimeWindowInMinutes(DateTime startTime, DateTime endTime)
        {
            // Check if end time is before start time, in which case it's an invalid time window.
            if (endTime < startTime)
            {
                return 0; // If the time window is invalid it is assigned with a duration value of 0 minutes.
            }

            // Calculate and return time window as a *whole* number of minutes
            TimeSpan timeWindow = endTime - startTime;
            return (long) Math.Floor(timeWindow.TotalMinutes);
        }



        // Helper method to capitalize the first letter of each word in a string.
        // This is employed to rewrite previously normalized names in a grammatically correct manner.
        private static string CapitalizeFirstLetters(string input)
        {
            // Check if the input is not empty
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            // Convert the first character to uppercase
            char[] charArray = input.ToCharArray();
            charArray[0] = char.ToUpper(charArray[0]);

            // Iterate through the characters starting from the second character
            for (int i = 1; i < charArray.Length; i++)
            {
                // Check if the current character is not a space and the previous character was a space. - (brains of this method)
                if (charArray[i - 1] == ' ' && charArray[i] != ' ')
                {
                    // Capitalize the current character
                    charArray[i] = char.ToUpper(charArray[i]);
                }
            }

            // Convert the char array back to a string
            return new(charArray);
        }
    }
}
