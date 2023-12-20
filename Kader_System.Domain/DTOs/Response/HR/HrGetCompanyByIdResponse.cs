namespace Kader_System.Domain.DTOs.Response.HR;

public class HrGetCompanyByIdResponse
{
    public int Id { get; set; }
    public string Name_en { get; set; } = string.Empty;
    public string Name_ar { get; set; } = string.Empty;
    public string Company_owner { get; set; } = string.Empty;
    public string? Company_licenses { get; set; }
    public string? Company_licenses_extension { get; set; }
    public int Company_type { get; set; }
}

