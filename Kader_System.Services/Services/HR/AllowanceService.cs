namespace Kader_System.Services.Services.HR;

public class AllowanceService(IUnitOfWork unitOfWork, IStringLocalizer<SharedResource> sharLocalizer, IMapper mapper) : IAllowanceService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IStringLocalizer<SharedResource> _sharLocalizer = sharLocalizer;
    private readonly IMapper _mapper = mapper;

    #region Allowance

    public async Task<Response<IEnumerable<SelectListResponse>>> ListOfAllowancesAsync(string lang)
    {
        var result =
                await _unitOfWork.Allowances.GetSpecificSelectAsync(null!,
                select: x => new SelectListResponse
                {
                    Id = x.Id,
                    Name = lang == Localization.Arabic ? x.Name_ar : x.Name_en
                }, orderBy: x =>
                  x.OrderByDescending(x => x.Id));

        if (!result.Any())
        {
            string resultMsg = _sharLocalizer[Localization.NotFoundData];

            return new()
            {
                Data = [],
                Error = resultMsg,
                Msg = resultMsg
            };
        }

        return new()
        {
            Data = result,
            Check = true
        };
    }

    public async Task<Response<HrGetAllAllowancesResponse>> GetAllAllowancesAsync(string lang, HrGetAllFiltrationsForAllowancesRequest model)
    {
        Expression<Func<HrAllowance, bool>> filter = x => x.IsDeleted == model.IsDeleted;

        var result = new HrGetAllAllowancesResponse
        {
            TotalRecords = await _unitOfWork.Allowances.CountAsync(filter: filter),

            Items = (await _unitOfWork.Allowances.GetSpecificSelectAsync(filter: filter,
                 take: model.PageSize,
                 skip: (model.PageNumber - 1) * model.PageSize,
                 select: x => new AllowanceData
                 {
                     Id = x.Id,
                     Name = lang == Localization.Arabic ? x.Name_ar : x.Name_en
                 }, orderBy: x =>
                   x.OrderByDescending(x => x.Id))).ToList()
        };

        if (result.TotalRecords is 0)
        {
            string resultMsg = _sharLocalizer[Localization.NotFoundData];

            return new()
            {
                Data = new()
                {
                    Items = []
                },
                Error = resultMsg,
                Msg = resultMsg
            };
        }

        return new()
        {
            Data = result,
            Check = true
        };
    }

    public async Task<Response<HrCreateAllowanceRequest>> CreateAllowanceAsync(HrCreateAllowanceRequest model)
    {
        bool exists = false;
        exists = await _unitOfWork.Companies.ExistAsync(x => x.Name_ar.Trim() == model.Name_ar
        && x.Name_en.Trim() == model.Name_en.Trim());

        if (exists)
        {
            string resultMsg = string.Format(_sharLocalizer[Localization.IsExist],
                _sharLocalizer[Localization.Allowance]);

            return new()
            {
                Error = resultMsg,
                Msg = resultMsg
            };
        }

        await _unitOfWork.Allowances.AddAsync(new()
        {
            Name_en = model.Name_en,
            Name_ar = model.Name_ar
        });
        await _unitOfWork.CompleteAsync();

        return new()
        {
            Msg = _sharLocalizer[Localization.Done],
            Check = true,
            Data = model
        };
    }

    public async Task<Response<HrGetAllowanceByIdResponse>> GetAllowanceByIdAsync(int id)
    {
        var obj = await _unitOfWork.Allowances.GetByIdAsync(id);

        if (obj is null)
        {
            string resultMsg = _sharLocalizer[Localization.NotFoundData];

            return new()
            {
                Data = new(),
                Error = resultMsg,
                Msg = resultMsg
            };
        }

        return new()
        {
            Data = new()
            {
                Id = id,
                Name_ar = obj.Name_ar,
                Name_en = obj.Name_en
            },
            Check = true
        };
    }

    public async Task<Response<HrUpdateAllowanceRequest>> UpdateAllowanceAsync(int id, HrUpdateAllowanceRequest model)
    {
        var obj = await _unitOfWork.Allowances.GetByIdAsync(id);

        if (obj == null)
        {
            string resultMsg = string.Format(_sharLocalizer[Localization.CannotBeFound],
                _sharLocalizer[Localization.Allowance]);

            return new()
            {
                Data = model,
                Error = resultMsg,
                Msg = resultMsg
            };
        }

        obj.Name_ar = model.Name_ar;
        obj.Name_en = model.Name_en;

        _unitOfWork.Allowances.Update(obj);
        await _unitOfWork.CompleteAsync();

        return new()
        {
            Check = true,
            Data = model,
            Msg = _sharLocalizer[Localization.Updated]
        };
    }

    public Task<Response<string>> UpdateActiveOrNotAllowanceAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<Response<string>> DeleteAllowanceAsync(int id)
    {
        var obj = await _unitOfWork.Allowances.GetByIdAsync(id);

        if (obj == null)
        {
            string resultMsg = string.Format(_sharLocalizer[Localization.CannotBeFound],
                _sharLocalizer[Localization.Allowance]);

            return new()
            {
                Data = string.Empty,
                Error = resultMsg,
                Msg = resultMsg
            };
        }

        _unitOfWork.Allowances.Remove(obj);
        await _unitOfWork.CompleteAsync();

        return new()
        {
            Check = true,
            Data = string.Empty,
            Msg = _sharLocalizer[Localization.Deleted]
        };
    }

    #endregion
}


