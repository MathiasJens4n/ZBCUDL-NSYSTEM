using System;

namespace LoanSystemLibraryMini
{
    public class Loan
    {
        public int Id { get; set; }
        public DateTime CollectedTime { get; set; }
        public DateTime? ReturnedTime { get; set; }
        public DateTime ReturnDeadline { get; set; }
        public string Note { get; set; }
        public User User { get; set; }
        public Equipment Equipment { get; set; }
        public Status Status { get; set; }
    }
}
