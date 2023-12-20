namespace Kader_System.Api.Profiles;

public class HrProfile : Profile
{
    public HrProfile()
    {
        #region Company

        CreateMap<StSubMainScreen, StUpdateSubMainScreenRequest>()
                .ReverseMap();

        #endregion
    }
}
