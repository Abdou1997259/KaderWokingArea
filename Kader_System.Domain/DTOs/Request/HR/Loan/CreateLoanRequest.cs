namespace Kader_System.Domain.DTOs.Request.HR.Loan
{
    public class CreateLoanRequest
    {
        [Required(ErrorMessage = Annotations.FieldIsRequired)]

        public DateTime LoanDate { get; set; }
        [Required(ErrorMessage = Annotations.FieldIsRequired)]
        public DateTime StartLoanDate { get; set; }

        public DateTime EndDoDate { get; set; }
        [Required(ErrorMessage = Annotations.FieldIsRequired)]
        public DateTime DocumentDate { get; set; }
        [Required(ErrorMessage = Annotations.FieldIsRequired)]
        public short DocumentType { get; set; }
        [Required(ErrorMessage = Annotations.FieldIsRequired)]
        public decimal MonthlyDeducted { get; set; }
        [Required(ErrorMessage = Annotations.FieldIsRequired)]
        public decimal LoanAmount { get; set; }
        public decimal PrevDedcutedAmount { get; set; }
        [Required(ErrorMessage = Annotations.FieldIsRequired)]

        public int EmpolyeeId { get; set; }

        public string Notes { get; set; }

        public int InstallmentCount { get; set; }

        public bool MakePaymentJournal { get; set; }
        public bool IsDeductedFromSalary { get; set; } = true;
    }
}
