using CS_Test.Models;

namespace CS_Test.Repositories
{
    // This class serves as a repository for the application.
    public class ShiftCheckInRegistry : IShiftCheckInRegistry
    {
        // URL for data retrieval via API.
        private readonly string _apiUrl;

        public ShiftCheckInRegistry(string apiUrl)
        {
            _apiUrl = apiUrl;
        }

        // Asynchronous method for data retrieval.
        // When awaiting data, the execution thread is released from the current context,
        // allowing the application to remain responsive and minimizing time loss during network operations.
        // Control is yielded back to the caller, and the thread can be utilized for other tasks.
        // Once the await is completed, the remainder of the method is scheduled for execution within its original context.
        public async Task<List<WorkShiftEntry>> GetAllShiftEntries()
        {
            using var httpClient = new HttpClient();

            try
            {
                // Perform an HTTP GET action using HttpClient.
                var response = await httpClient.GetAsync(_apiUrl);

                // Throw an exception if the status code is other than "200 OK". 
                response.EnsureSuccessStatusCode();

                // Read the raw text from the body of the HTTP response.
                var responseBody = await response.Content.ReadAsStringAsync();

                if (responseBody != null)
                {
                    // Deserialize the response body, expecting a string representation of a serialized list object in JSON format.
                    var workedShifts = Newtonsoft.Json.JsonConvert.DeserializeObject<List<WorkShiftEntry>>(responseBody);

                    if (workedShifts != null)
                    {
                        // Return the unpacked response data.
                        return workedShifts;
                    }
                }
                // If, for some reason, we couldn't deserialize the response body, throw an exception. 
                throw new Exception("Data could not be fetched from the API.");
            }
            finally
            {
                // Ensure that the HttpClient is always disposed.
                httpClient.Dispose();
            }
        }
    }
}
