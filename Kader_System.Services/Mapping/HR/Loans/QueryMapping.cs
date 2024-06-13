


using Kader_System.Domain.DTOs.Response.HR.Loan;

namespace Kader_System.Services.Mapping.HR.Loans
{
    public partial class LoanMapping
    {
        public void SetQuery()
        {
            CreateMap<HrLoan, GetLoanByIdResponse>();
        }
    }
}
