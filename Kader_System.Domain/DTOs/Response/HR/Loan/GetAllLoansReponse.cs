namespace Kader_System.Domain.DTOs.Response.HR.Loan
{
    public class GetAllLoansReponse : PaginationData<LoanData>
    {
    }
    public class LoanData
    {
        public int Id { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime StartLoanDate { get; set; }
        public DateTime EndDoDate { get; set; }
        public DateTime DocumentDate { get; set; }
        public short DocumentType { get; set; }
        public decimal MonthlyDeducted { get; set; }
        public decimal LoanAmount { get; set; }
        public decimal PrevDedcutedAmount { get; set; }
        public int EmpolyeeId { get; set; }
        public string Notes { get; set; }

        public int InstallmentCount { get; set; }
        public bool MakePaymentJournal { get; set; }
        public bool IsDeductedFromSalary { get; set; }
    }
}
