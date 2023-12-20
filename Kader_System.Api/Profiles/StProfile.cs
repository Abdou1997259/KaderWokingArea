namespace Kader_System.Api.Profiles;

public class StProfile : Profile
{
    public StProfile()
    {
        #region Setting

        CreateMap<StSubMainScreen, StUpdateSubMainScreenRequest>()
                .ReverseMap();

        #endregion
    }
}
