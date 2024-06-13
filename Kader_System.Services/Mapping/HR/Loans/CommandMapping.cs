

using Kader_System.Domain.DTOs.Request.HR.Loan;

namespace Kader_System.Services.Mapping.HR.Loans
{
    public partial class LoanMapping
    {
        public void SetCommandMapping()
        {
            CreateMap<CreateLoanRequest, HrLoan>();
            CreateMap<UpdateLoanRequest, HrLoan>();
        }
    }
}
