using CS_Test.Models;

namespace CS_Test.Repositories
{
    // Interface listing methods for providing data required by the application model.
    public interface IShiftCheckInRegistry
    {
        // Asynchronous method for retrieving a list of work shift entries.
        Task<List<WorkShiftEntry>> GetAllShiftEntries();
    }

}