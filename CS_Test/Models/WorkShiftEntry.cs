namespace CS_Test.Models
{ 
    
    // Represents a work shift entry with details such as employee name, start and end times, notes, and deletion information.
    public class WorkShiftEntry
    {
        // Unique identifier for the work shift entry.
        public Guid Id { get; set; }

        // Name of the employee associated with the work shift.
        public string EmployeeName { get; set; }

        // UTC timestamp indicating the start time of the work shift.
        public DateTime StarTimeUtc { get; set; }

        // UTC timestamp indicating the end time of the work shift.
        public DateTime EndTimeUtc { get; set; }

        // Additional notes or details about the work shift entry.
        public string EntryNotes { get; set; }

        // UTC timestamp indicating when the work shift entry was deleted (nullable if not deleted).
        public DateTime? DeletedOn { get; set; }
    }

}
