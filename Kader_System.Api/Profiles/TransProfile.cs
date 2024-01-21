

using Kader_System.Domain.DTOs.Response.Trans;

namespace Kader_System.Api.Profiles
{
    public class TransProfile : Profile
    {
        public TransProfile()
        {
            #region Allowance

            CreateMap<TransAllowance, CreateTransAllowanceRequest>()
                .ReverseMap();
            CreateMap<TransactionAllowanceGetByIdResponse, TransAllowance>()
                .ForMember(dest => dest.Add_date, opt => opt.MapFrom(src => src.AddedOn)).ReverseMap();

            #endregion
        }
    }
}
