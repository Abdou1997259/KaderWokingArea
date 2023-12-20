namespace Kader_System.Services.IServices.HR;

public interface ICompanyService
{
    Task<Response<IEnumerable<HrListOfCompaniesResponse>>> ListOfCompaniesAsync(string lang);
    Task<Response<HrGetAllCompaniesResponse>> GetAllCompaniesAsync(string lang, HrGetAllFiltrationsForCompaniesRequest model);
    Task<Response<HrCreateCompanyRequest>> CreateCompanyAsync(HrCreateCompanyRequest model);
    Task<Response<HrGetCompanyByIdResponse>> GetCompanyByIdAsync(int id);
    Task<Response<HrUpdateCompanyRequest>> UpdateCompanyAsync(int id, HrUpdateCompanyRequest model);
    Task<Response<string>> UpdateActiveOrNotCompanyAsync(int id);
    Task<Response<string>> DeleteCompanyAsync(int id);
}
