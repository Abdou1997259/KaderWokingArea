namespace Kader_System.Services.IServices.Trans;

public interface ITransVacationService
{
    Task<Response<IEnumerable<SelectListOfTransVacationResponse>>> ListOfTransVacationsAsync(string lang);
    Task<Response<GetAllTransVacationResponse>> GetAllTransVacationsAsync(string lang, GetAllFilterationForTransVacationRequest model);
    Task<Response<CreateTransVacationRequest>> CreateTransVacationAsync(CreateTransVacationRequest model);
    Task<Response<GetTransVacationById>> GetTransVacationByIdAsync(int id);
    Task<Response<GetTransVacationById>> UpdateTransVacationAsync(int id, CreateTransVacationRequest model);
    Task<Response<string>> UpdateActiveOrNotTransVacationAsync(int id);
    Task<Response<string>> DeleteTransVacationAsync(int id);
}
