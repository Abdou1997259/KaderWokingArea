using Kader_System.Domain.DTOs.Request.HR.Loan;
using Kader_System.Domain.DTOs.Response.HR.Loan;

namespace Kader_System.Services.IServices.HR
{
    public interface ILoanService
    {
        Task<Response<IEnumerable<ListOfLoansReponse>>> ListOfLoansAsync(string lang);
        Task<Response<GetAllLoansReponse>> GetAllLoansAsync(string lang, GetAllFiterationForLoansRequest filter, string host);
        Task<Response<CreateLoanRequest>> CreateLoanAsync(CreateLoanRequest loan);
        Task<Response<UpdateLoanRequest>> UpdateLoanAsync(int id, UpdateLoanRequest loan);
        Task<Response<GetLoanByIdResponse>> GetLoanByIdAsync(int id);
        Task<Response<GetLoanByIdResponse>> RestoreLoanAsync(int id);
        Task<Response<string>> UpdateActiveOrNotLoanAsync(int id);
        Task<Response<string>> DeleteLoanAsync(int id);

    }
}
