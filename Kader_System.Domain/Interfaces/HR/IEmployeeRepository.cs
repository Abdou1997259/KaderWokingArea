using Kader_System.Domain.DTOs.Response.HR;

namespace Kader_System.Domain.Interfaces.HR;

public interface IEmployeeRepository : IBaseRepository<HrEmployee>
{
    Response<GetEmployeeByIdResponse> GetEmployeeByIdAsync(int id, string lang);
}
