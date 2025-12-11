namespace Library.Application.DTOs.Loan
{
    public class UpdateLoanDto
    {
        public string? StudentName { get; set; }
        public DateTime? ReturnDate { get; set; }
        public string? Status { get; set; }
    }
}
