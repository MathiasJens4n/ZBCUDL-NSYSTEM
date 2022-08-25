using System;

namespace LoanSystemLibraryMini
{
    public class Equipment
    {
        public int Id { get; set; }
        public string Identification { get; set; }
        public DateTime? LastMaintenance { get; set; }
        public string Note { get; set; }
        public string WorkNote { get; set; }
        public Department BelongingDepartment { get; set; }
        public Department CurrentDepartment { get; set; }
        public Model Model { get; set; }
        public Status Status { get; set; }
    }
}
