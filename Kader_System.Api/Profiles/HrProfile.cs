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
    }
}
