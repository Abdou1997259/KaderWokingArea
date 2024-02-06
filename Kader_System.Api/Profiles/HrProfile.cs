using Kader_System.Domain.Models.HR;

namespace Kader_System.Api.Profiles;

public class HrProfile : Profile
{
    public HrProfile()
    {
        #region Company

        CreateMap<StScreenSub, StUpdateSubMainScreenRequest>()
                .ReverseMap();

        #endregion


        #region Department

        CreateMap<HrDepartment,CreateDepartmentRequest>()
            .ReverseMap();

        #endregion

        #region Employee
        CreateMap<HrEmployee, CreateEmployeeRequest>()
            .IgnoreAllPropertiesWithAnInaccessibleSetter()
            .ReverseMap();
        CreateMap<HrEmployee,EmployeesData>()
            .ForMember(dest => dest.Company, opt => opt.MapFrom(src => src.Company!.NameAr))
            .ForMember(dest => dest.Management, opt => opt.MapFrom(src => src.Management!.NameAr));


        #endregion


        #region Contract
        CreateMap<HrContract,CreateContractRequest>()
            .ForMember(dest => dest.Details, opt => opt.MapFrom(src => src.ListOfAllowancesDetails.Select(d => new CreateContractDetailsRequest
            {
                AllowanceId = d.AllowanceId,
                Value = d.Value,
                IsPercent = d.IsPercent,
                Id = d.Id
            }).ToList()))
            .ReverseMap();


        #endregion


        #region FingerPrintDevice

        CreateMap<HrFingerPrint, CreateFingerPrintDeviceRequest>()
            .IgnoreAllPropertiesWithAnInaccessibleSetter()
            .ReverseMap();
  
        CreateMap<HrFingerPrint, GetFingerPrintDeviceByIdResponse>()
            .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Company!.NameAr))
            .ForMember(dest => dest.CompanyId, opt => opt.MapFrom(src => src.Company!.Id))
            .IgnoreAllPropertiesWithAnInaccessibleSetter()
            .ReverseMap();
        #endregion
    }
}
