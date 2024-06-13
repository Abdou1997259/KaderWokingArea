namespace Kader_System.Domain.DTOs.Request.HR.Loan
{
    public class GetAllFiterationForLoansRequest : PaginationRequest
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }

    }
}
