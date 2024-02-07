namespace Kader_System.Services.IServices.HR;

public interface IEmployeeService
{
    Task<Response<IEnumerable<ListOfEmployeesResponse>>> ListOfEmployeesAsync(string lang);
    Task<Response<GetAllEmployeesResponse>> GetAllEmployeesAsync(string lang, GetAllEmployeesFilterRequest model, string host);
    Task<Response<CreateEmployeeRequest>> CreateEmployeeAsync(CreateEmployeeRequest model);
    Task<Response<GetEmployeeByIdResponse>> GetEmployeeByIdAsync(int id);
    Task<Response<CreateEmployeeRequest>> UpdateEmployeeAsync(int id, CreateEmployeeRequest model);
    Task<Response<string>> UpdateActiveOrNotEmployeeAsync(int id);
    Task<Response<string>> DeleteEmployeeAsync(int id);
}
