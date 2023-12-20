namespace Kader_System.Services.Services.HR;

public class CompanyService(IUnitOfWork unitOfWork, IStringLocalizer<SharedResource> sharLocalizer, IMapper mapper) : ICompanyService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IStringLocalizer<SharedResource> _sharLocalizer = sharLocalizer;
    private readonly IMapper _mapper = mapper;

    #region Company

    public async Task<Response<IEnumerable<HrListOfCompaniesResponse>>> ListOfCompaniesAsync(string lang)
    {
        var result =
                await _unitOfWork.Companies.GetSpecificSelectAsync(null!,
                select: x => new HrListOfCompaniesResponse
                {
                    Id = x.Id,
                    Name = lang == Localization.Arabic ? x.Name_ar : x.Name_en,
                    Company_licenses = x.Company_licenses,
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

    public async Task<Response<HrGetAllCompaniesResponse>> GetAllCompaniesAsync(string lang, HrGetAllFiltrationsForCompaniesRequest model)
    {
        Expression<Func<HrCompany, bool>> filter = x => x.IsDeleted == model.IsDeleted;

        var result = new HrGetAllCompaniesResponse
        {
            TotalRecords = await _unitOfWork.Companies.CountAsync(filter: filter),

            Items = (await _unitOfWork.Companies.GetSpecificSelectAsync(filter: filter,
                 take: model.PageSize,
                 skip: (model.PageNumber - 1) * model.PageSize,
                 select: x => new CompanyData
                 {
                     Id = x.Id,
                     Added_by = x.Added_by,
                     Add_date = x.Add_date.ToGetFullyDate(),
                     Company_owner = x.Company_owner,
                     Company_type_name = lang == Localization.Arabic ? x.CompanyType.Name : x.CompanyType.NameInEnglish,
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

    public async Task<Response<HrCreateCompanyRequest>> CreateCompanyAsync(HrCreateCompanyRequest model)
    {
        bool exists = false;
        exists = await _unitOfWork.Companies.ExistAsync(x => x.Name_ar.Trim() == model.Name_ar
        && x.Name_en.Trim() == model.Name_en.Trim());

        if (exists)
        {
            string resultMsg = string.Format(_sharLocalizer[Localization.IsExist],
                _sharLocalizer[Localization.Company]);

            return new()
            {
                Error = resultMsg,
                Msg = resultMsg
            };
        }

        string imageName = null!, imageExtension = null!;
        if (model.Company_licenses is not null)
        {
            var fileObj = ManageFilesHelper.UploadFile(model.Company_licenses, GoRootPath.HRImagesPath);
            imageName = fileObj.FileName;
            imageExtension = fileObj.FileExtension;
        }

        List<GetFileNameAndExtension> getFileNameAnds = [];
        if (model.Company_contracts.Any())
        {
            getFileNameAnds = ManageFilesHelper.UploadFiles(model.Company_contracts, GoRootPath.HRFilesPath);
        }

        await _unitOfWork.Companies.AddAsync(new()
        {
            Name_en = model.Name_en,
            Name_ar = model.Name_ar,
            Company_owner = model.Company_owner,
            Company_licenses = imageName,
            Company_licenses_extension = imageExtension,
            Company_type = model.Company_type,
            ListOfsContract = getFileNameAnds.Select(y => new HrCompanyContract
            {
                Company_contracts = y.FileName,
                Company_contracts_extension = y.FileExtension
            }).ToList()
        });
        await _unitOfWork.CompleteAsync();

        return new()
        {
            Msg = _sharLocalizer[Localization.Done],
            Check = true,
            Data = model
        };
    }

    public async Task<Response<HrGetCompanyByIdResponse>> GetCompanyByIdAsync(int id)
    {
        var obj = await _unitOfWork.Companies.GetByIdAsync(id);

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
                Name_en = obj.Name_en,
                Company_owner = obj.Company_owner,
                Company_type = obj.Company_type,
                Company_licenses = obj.Company_licenses is not null ? string.Concat(ReadRootPath.HRImagesPath, obj.Company_licenses) : null,
                Company_licenses_extension = obj.Company_licenses_extension            },
            Check = true
        };
    }

    public Task<Response<HrUpdateCompanyRequest>> UpdateCompanyAsync(int id, HrUpdateCompanyRequest model)
    {
        throw new NotImplementedException();
    }

    public Task<Response<string>> UpdateActiveOrNotCompanyAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<Response<string>> DeleteCompanyAsync(int id)
    {
        {
            var obj = await _unitOfWork.Companies.GetByIdAsync(id);

            if (obj == null)
            {
                string resultMsg = string.Format(_sharLocalizer[Localization.CannotBeFound],
                    _sharLocalizer[Localization.Company]);

                return new()
                {
                    Data = string.Empty,
                    Error = resultMsg,
                    Msg = resultMsg
                };
            }

            _unitOfWork.Companies.Remove(obj);
            await _unitOfWork.CompleteAsync();

            return new()
            {
                Check = true,
                Data = string.Empty,
                Msg = _sharLocalizer[Localization.Deleted]
            };
        }
    }

    #endregion
}


