using Kader_System.Domain.Models.HR;

namespace Kader_System.Api.Profiles;

public class HrProfile : Profile
{
    public HrProfile()
    {
        #region Company

        CreateMap<StSubMainScreen, StUpdateSubMainScreenRequest>()
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
    }
}
