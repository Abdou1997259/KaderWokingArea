namespace Kader_System.Services.IServices.HR;

public interface IContractService
{
    Task<Response<IEnumerable<ListOfContractsResponse>>> ListOfContractsAsync(string lang);
    Task<Response<GetAllContractsResponse>> GetAllContractAsync(string lang, GetAlFilterationForContractRequest model);
    Task<Response<CreateContractRequest>> CreateContractAsync(CreateContractRequest model);
    Task<Response<GetContractByIdResponse>> GetContractByIdAsync(int id);
    Task<Response<CreateContractRequest>> UpdateContractAsync(int id, CreateContractRequest model);
    Task<Response<string>> UpdateActiveOrNotContractAsync(int id);
    Task<Response<string>> DeleteContractAsync(int id);
}
