﻿namespace Kader_System.Services.Services.HR;

public class CompanyService(IUnitOfWork unitOfWork, IStringLocalizer<SharedResource> shareLocalizer, IMapper mapper) : ICompanyService
{
    private HrCompany _instance;



    #region Retrieve

    public async Task<Response<IEnumerable<HrListOfCompaniesResponse>>> ListOfCompaniesAsync(string lang)
    {
        var result =
                await unitOfWork.Companies.GetSpecificSelectAsync(null!,
                select: x => new HrListOfCompaniesResponse
                {
                    Id = x.Id,
                    Name = lang == Localization.Arabic ? x.NameAr : x.NameEn,
                    Company_licenses = x.Company_licenses,
                }, orderBy: x =>
                  x.OrderByDescending(x => x.Id));

        if (!result.Any())
        {
            string resultMsg = shareLocalizer[Localization.NotFoundData];

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
            TotalRecords = await unitOfWork.Companies.CountAsync(filter: filter),

            Items = (await unitOfWork.Companies.GetSpecificSelectAsync(filter: filter,
                 take: model.PageSize,
                 skip: (model.PageNumber - 1) * model.PageSize,
                 select: x => new CompanyData
                 {
                     Id = x.Id,
                     Added_by = x.Added_by,
                     Add_date = x.Add_date.ToGetFullyDate(),
                     Company_owner = x.CompanyOwner,
                     Company_type_name = lang == Localization.Arabic ? x.CompanyType.Name : x.CompanyType.NameInEnglish,
                     Name = lang == Localization.Arabic ? x.NameAr : x.NameEn
                 }, orderBy: x =>
                   x.OrderByDescending(x => x.Id))).ToList()
        };

        if (result.TotalRecords is 0)
        {
            string resultMsg = shareLocalizer[Localization.NotFoundData];

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


    public async Task<Response<HrGetCompanyByIdResponse>> GetCompanyByIdAsync(int id)
    {
        var obj = await unitOfWork.Companies.GetByIdAsync(id);

        if (obj is null)
        {
            string resultMsg = shareLocalizer[Localization.NotFoundData];

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
                Name_en = obj.NameEn,
                Company_owner = obj.CompanyOwner,
                Company_type = obj.CompanyTypeId,
            },
            Check = true
        };
    }

    #endregion


    #region Insert

    public async Task<Response<HrCreateCompanyRequest>> CreateCompanyAsync(HrCreateCompanyRequest model)
    {
        var exists = await unitOfWork.Companies.ExistAsync(x => x.NameAr.Trim() == model.Name_ar
                                                                && x.NameEn.Trim() == model.Name_en.Trim());

        if (exists)
        {
            string resultMsg = string.Format(shareLocalizer[Localization.IsExist],
                shareLocalizer[Localization.Company]);

            return new()
            {
                Error = resultMsg,
                Msg = resultMsg
            };
        }




        List<GetFileNameAndExtension> getFileNameAnds = [];
        if (model.Company_contracts is not null && model.Company_contracts.Any())
        {
            getFileNameAnds = ManageFilesHelper.UploadFiles(model.Company_contracts, GoRootPath.HRFilesPath);
        }
        List<GetFileNameAndExtension> getLicenseFileNameAnds = [];
        if (model.Company_licenses is not null && model.Company_licenses.Any())
        {
            getLicenseFileNameAnds = ManageFilesHelper.UploadFiles(model.Company_licenses, GoRootPath.HRFilesPath);
        }

        await unitOfWork.Companies.AddAsync(new()
        {
            NameEn = model.Name_en,
            NameAr = model.Name_ar,
            CompanyOwner = model.Company_owner,
            CompanyTypeId = model.Company_type,
            Licenses = getLicenseFileNameAnds?.Select(l => new CompanyLicense()
            {
                LicenseExtension = l.FileExtension,
                LicenseName = l.FileName,
            }).ToList(),
            ListOfsContract = getFileNameAnds?.Select(y => new HrCompanyContract
            {
                CompanyContracts = y.FileName,
                CompanyContractsExtension = y.FileExtension
            }).ToList()
        });
        await unitOfWork.CompleteAsync();

        return new()
        {
            Msg = shareLocalizer[Localization.Done],
            Check = true,
            Data = model
        };
    }
    #endregion

    #region Update


    public async Task<Response<HrUpdateCompanyRequest>> UpdateCompanyAsync(int id, HrUpdateCompanyRequest model)
    {
        Expression<Func<HrCompany, bool>> filter = x => x.IsDeleted == false && x.Id == id;
        var obj = await unitOfWork.Companies.GetFirstOrDefaultAsync(filter, includeProperties: $"{nameof(_instance.Licenses) + "," + nameof(_instance.ListOfsContract)}");

        if (obj is null)
        {

            string resultMsg = string.Format(shareLocalizer[Localization.CannotBeFound],
                shareLocalizer[Localization.Company]);

            return new()
            {
                Data = model,
                Error = resultMsg,
                Msg = resultMsg
            };
        }

        using var transaction = unitOfWork.BeginTransaction();
        try
        {


            if (obj.ListOfsContract.Any())
            {
                ManageFilesHelper.RemoveFiles(obj.ListOfsContract.Select(l => GoRootPath.HRFilesPath + l.CompanyContracts).ToList());
                unitOfWork.CompanyContracts.RemoveRange(obj.ListOfsContract);
            }
            if (obj.Licenses.Any())
            {
                ManageFilesHelper.RemoveFiles(obj.Licenses.Select(l => GoRootPath.HRFilesPath + l.LicenseName).ToList());
                unitOfWork.CompanyLicenses.RemoveRange(obj.Licenses);
            }
            List<GetFileNameAndExtension> getFileNameAnds = [];
            if (model.Company_contracts is not null && model.Company_contracts.Any())
            {
                getFileNameAnds = ManageFilesHelper.UploadFiles(model.Company_contracts, GoRootPath.HRFilesPath);
            }
            List<GetFileNameAndExtension> getLicenseFileNameAnds = [];
            if (model.Company_licenses is not null && model.Company_licenses.Any())
            {
                getLicenseFileNameAnds = ManageFilesHelper.UploadFiles(model.Company_licenses, GoRootPath.HRFilesPath);
            }

            obj.NameEn = model.Name_en;
            obj.NameAr = model.Name_ar;
            obj.CompanyOwner = model.Company_owner;
            obj.CompanyTypeId = model.Company_type;
            obj.Licenses = getLicenseFileNameAnds?.Select(l => new CompanyLicense()
            {
                LicenseExtension = l.FileExtension,
                LicenseName = l.FileName,
            }).ToList();
            obj.ListOfsContract = getFileNameAnds?.Select(y => new HrCompanyContract
            {
                CompanyContracts = y.FileName,
                CompanyContractsExtension = y.FileExtension
            }).ToList();

            await unitOfWork.CompleteAsync();
            transaction.Commit();

            return new()
            {
                Msg = shareLocalizer[Localization.Done],
                Check = true,
                Data = model
            };


        }
        catch (Exception ex)
        {
            transaction.Rollback();
            return new()
            {
                Data = model,
                Error = ex.Message,
                Msg = ex.Message
            };
        }



    }

    public Task<Response<string>> UpdateActiveOrNotCompanyAsync(int id)
    {
        throw new NotImplementedException();
    }
    #endregion


    #region Delete

    public async Task<Response<string>> DeleteCompanyAsync(int id)
    {
        {
            Expression<Func<HrCompany, bool>> filter = x => x.Id == id;
            var obj = await unitOfWork.Companies.GetFirstOrDefaultAsync(filter, includeProperties: $"{nameof(_instance.Licenses)},{nameof(_instance.ListOfsContract)}");

            if (obj == null)
            {
                string resultMsg = string.Format(shareLocalizer[Localization.CannotBeFound],
                    shareLocalizer[Localization.Company]);

                return new()
                {
                    Data = string.Empty,
                    Error = resultMsg,
                    Msg = resultMsg
                };
            }

            if (obj.Licenses.Any())
            {
                unitOfWork.CompanyLicenses.RemoveRange(obj.Licenses);
            }

            if (obj.ListOfsContract.Any())
            {
                unitOfWork.CompanyContracts.RemoveRange(obj.ListOfsContract);
            }

            unitOfWork.Companies.Remove(obj);
            await unitOfWork.CompleteAsync();

            return new()
            {
                Check = true,
                Data = string.Empty,
                Msg = shareLocalizer[Localization.Deleted]
            };
        }
    }

    #endregion
}


