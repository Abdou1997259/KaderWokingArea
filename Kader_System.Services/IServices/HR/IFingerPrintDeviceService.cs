namespace Kader_System.Services.IServices.HR
{
    public interface IFingerPrintDeviceService
    {
        Task<Response<GetAllFingerPrintDevicesResponse>> GetAllFingerPrintDevicesAsync(string lang,GetAllFingerPrintDevicesFilterrationRequest request);
        Task<Response<GetFingerPrintDeviceByIdResponse>> GetFingerPrintDeviceByIdAsync(int id);
        Task<Response<IEnumerable<ListOfFingerPrintDevicesResponse>>> GetFingerPrintDevicesAsync(string lang);
        Task<Response<CreateFingerPrintDeviceRequest>> CreateFingerPrintDevicesAsync(CreateFingerPrintDeviceRequest request);
        Task<Response<CreateFingerPrintDeviceRequest>> UpdateFingerPrintDevicesAsync(int id,CreateFingerPrintDeviceRequest request);
        Task<Response<string>> UpdateActiveOrNotEmployeeAsync(int id);
        Task<Response<string>> DeleteFingerPrintAsync(int id);
    }
}
