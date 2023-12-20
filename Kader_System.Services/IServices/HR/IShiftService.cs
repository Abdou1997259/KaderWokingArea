namespace Kader_System.Services.IServices.HR;

public interface IShiftService
{
    Task<Response<IEnumerable<SelectListResponse>>> ListOfShiftsAsync(string lang);
    Task<Response<HrGetAllShiftsResponse>> GetAllShiftsAsync(string lang, HrGetAllFiltrationsForShiftsRequest model);
    Task<Response<HrCreateShiftRequest>> CreateShiftAsync(HrCreateShiftRequest model);
    Task<Response<HrGetShiftByIdResponse>> GetShiftByIdAsync(int id);
    Task<Response<HrUpdateShiftRequest>> UpdateShiftAsync(int id, HrUpdateShiftRequest model);
    Task<Response<string>> UpdateActiveOrNotShiftAsync(int id);
    Task<Response<string>> DeleteShiftAsync(int id);
}
