namespace Kader_System.Services.Services.HR;

public class QualificationService(IUnitOfWork unitOfWork, IStringLocalizer<SharedResource> sharLocalizer, IMapper mapper) : IQualificationService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IStringLocalizer<SharedResource> _sharLocalizer = sharLocalizer;
    private readonly IMapper _mapper = mapper;

    #region Qualification

    public async Task<Response<IEnumerable<SelectListResponse>>> ListOfQualificationsAsync(string lang)
    {
        var result =
                await _unitOfWork.Qualifications.GetSpecificSelectAsync(null!,
                select: x => new SelectListResponse
                {
                    Id = x.Id,
                    Name = lang == Localization.Arabic ? x.NameAr : x.NameEn
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

    public async Task<Response<HrGetAllQualificationsResponse>> GetAllQualificationsAsync(string lang, HrGetAllFiltrationsForQualificationsRequest model)
    {
        Expression<Func<HrQualification, bool>> filter = x => x.IsDeleted == model.IsDeleted;

        var result = new HrGetAllQualificationsResponse
        {
            TotalRecords = await _unitOfWork.Qualifications.CountAsync(filter: filter),

            Items = (await _unitOfWork.Qualifications.GetSpecificSelectAsync(filter: filter,
                 take: model.PageSize,
                 skip: (model.PageNumber - 1) * model.PageSize,
                 select: x => new QualificationData
                 {
                     Id = x.Id,
                     Name = lang == Localization.Arabic ? x.NameAr : x.NameEn
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

    public async Task<Response<HrCreateQualificationRequest>> CreateQualificationAsync(HrCreateQualificationRequest model)
    {
        bool exists = false;
        exists = await _unitOfWork.Qualifications.ExistAsync(x => x.NameAr.Trim() == model.Name_ar
        && x.NameEn.Trim() == model.Name_en.Trim());

        if (exists)
        {
            string resultMsg = string.Format(_sharLocalizer[Localization.IsExist],
                _sharLocalizer[Localization.Qualification]);

            return new()
            {
                Error = resultMsg,
                Msg = resultMsg
            };
        }

        await _unitOfWork.Qualifications.AddAsync(new()
        {
            NameEn = model.Name_en,
            NameAr = model.Name_ar
        });
        await _unitOfWork.CompleteAsync();

        return new()
        {
            Msg = _sharLocalizer[Localization.Done],
            Check = true,
            Data = model
        };
    }

    public async Task<Response<HrGetQualificationByIdResponse>> GetQualificationByIdAsync(int id)
    {
        var obj = await _unitOfWork.Qualifications.GetByIdAsync(id);

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
                Name_ar = obj.NameAr,
                Name_en = obj.NameEn
            },
            Check = true
        };
    }

    public async Task<Response<HrUpdateQualificationRequest>> UpdateQualificationAsync(int id, HrUpdateQualificationRequest model)
    {
        var obj = await _unitOfWork.Qualifications.GetByIdAsync(id);

        if (obj == null)
        {
            string resultMsg = string.Format(_sharLocalizer[Localization.CannotBeFound],
                _sharLocalizer[Localization.Qualification]);

            return new()
            {
                Data = model,
                Error = resultMsg,
                Msg = resultMsg
            };
        }

        obj.NameAr = model.Name_ar;
        obj.NameEn = model.Name_en;

        _unitOfWork.Qualifications.Update(obj);
        await _unitOfWork.CompleteAsync();

        return new()
        {
            Check = true,
            Data = model,
            Msg = _sharLocalizer[Localization.Updated]
        };
    }

    public Task<Response<string>> UpdateActiveOrNotBenefitAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<Response<string>> DeleteQualificationAsync(int id)
    {
        var obj = await _unitOfWork.Qualifications.GetByIdAsync(id);

        if (obj == null)
        {
            string resultMsg = string.Format(_sharLocalizer[Localization.CannotBeFound],
                _sharLocalizer[Localization.Qualification]);

            return new()
            {
                Data = string.Empty,
                Error = resultMsg,
                Msg = resultMsg
            };
        }

        _unitOfWork.Qualifications.Remove(obj);
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


