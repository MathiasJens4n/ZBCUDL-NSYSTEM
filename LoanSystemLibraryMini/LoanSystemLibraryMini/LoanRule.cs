using System;

namespace LoanSystemLibraryMini
{
    public class LoanRule
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int LoanLength { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string Note { get; set; }
        public LoanRule ReplacementRule { get; set; }
        public Status Status { get; set; }
    }
}
