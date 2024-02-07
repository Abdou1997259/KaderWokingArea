namespace Kader_System.Services.IServices.Trans;

public interface ITransDeductionService
{
    Task<Response<IEnumerable<SelectListOfTransDeductionResponse>>> ListOfTransDeductionsAsync(string lang);
    Task<Response<GetAllTransDeductionResponse>> GetAllTransDeductionsAsync(string lang, GetAllFilterationForTransDeductionRequest model, string host);
    Task<Response<CreateTransDeductionRequest>> CreateTransDeductionAsync(CreateTransDeductionRequest model);
    Task<Response<GetTransDeductionById>> GetTransDeductionByIdAsync(int id);
    Task<Response<GetTransDeductionById>> UpdateTransDeductionAsync(int id, CreateTransDeductionRequest model);
    Task<Response<string>> UpdateActiveOrNotTransDeductionAsync(int id);
    Task<Response<string>> DeleteTransDeductionAsync(int id);
}
