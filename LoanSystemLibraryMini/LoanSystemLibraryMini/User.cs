namespace LoanSystemLibraryMini
{
    public class User
    {
        public int Id { get; set; }
        public string UniLogin { get; set; }
        public string Name { get; set; }
        public string MobileNumber { get; set; }
        public string Note { get; set; }
        public LoanRule LoanRule { get; set; }
        public Status Status { get; set; }
    }
}
