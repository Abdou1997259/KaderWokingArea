
using Kader_System.Domain.DTOs.Request.Trans;

namespace Kader_System.Services.IServices.Trans;

public interface ITransAllowanceService
{
    Task<Response<IEnumerable<SelectListForTransAllowancesResponse>>> ListOfTransAllowancesAsync(string lang);
    Task<Response<TransAllowanceGetAllResponse>> GetAllTransAllowancesAsync(string lang, GetAllFilterationAllowanceRequest model, string host);
    Task<Response<CreateTransAllowanceRequest>> CreateTransAllowanceAsync(CreateTransAllowanceRequest model);
    Task<Response<TransactionAllowanceGetByIdResponse>> GetTransAllowanceByIdAsync(int id);
    Task<Response<TransactionAllowanceGetByIdResponse>> UpdateTransAllowanceAsync(int id, CreateTransAllowanceRequest model);
    Task<Response<string>> UpdateActiveOrNotTransAllowanceAsync(int id);
    Task<Response<string>> DeleteTransAllowanceAsync(int id);
}
