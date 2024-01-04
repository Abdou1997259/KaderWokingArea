
namespace Kader_System.Services.Services.HR
{
    public class ContractService(
        IUnitOfWork unitOfWork,
        IStringLocalizer<SharedResource> shareLocalizer,
        IMapper mapper) : IContractService
    {
        private HrContract _instanceContract;

        public async Task<Response<IEnumerable<ListOfContractsResponse>>> ListOfContractsAsync(string lang)
        {
            var result =
                await unitOfWork.Contracts.GetSpecificSelectAsync(null!
                    , includeProperties: $"{nameof(_instanceContract.Employee)}",
                    select: x => new ListOfContractsResponse
                    {
                        Id = x.Id,
                        TotalSalary = x.TotalSalary,
                        FixedSalary = x.FixedSalary,
                        EmployeeId = x.EmployeeId,
                        EmployeeName = lang == Localization.Arabic ? x.Employee!.FullNameAr : x.Employee!.FullNameEn,
                        StartDate = x.StartDate,
                        EndDate = x.EndDate,
                        HousingAllowance = x.HousingAllowance

                    }, orderBy: x => x.OrderByDescending(x => x.Id));

            var listOfEmployeesResponses = result.ToList();
            if (!listOfEmployeesResponses.Any())
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
                Data = listOfEmployeesResponses,
                Check = true
            };
        }

        public async Task<Response<GetAllContractsResponse>> GetAllContractAsync(string lang,
            GetAlFilterationForContractRequest model)
        {

            Expression<Func<HrContract, bool>> filter = x => x.IsDeleted == model.IsDeleted;

            var result = new GetAllContractsResponse
            {
                TotalRecords = await unitOfWork.Contracts.CountAsync(filter: filter),

                Items = (await unitOfWork.Contracts.GetAllContractsAsync
                (contractFilter: filter,
                    lang: lang,
                    take: model.PageSize,
                    skip: (model.PageNumber - 1) * model.PageSize))
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

        public async Task<Response<GetContractByIdResponse>> GetContractByIdAsync(int id)
        {
            Expression<Func<HrContract, bool>> filter = x => x.Id == id;
            var obj = await unitOfWork.Contracts.GetFirstOrDefaultAsync(filter,
                includeProperties:
                $"{nameof(_instanceContract.Employee)},{nameof(_instanceContract.ListOfAllowancesDetails)}");

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
                    Id = obj.Id,
                    EmployeeId = obj.EmployeeId,
                    EmployeeName = obj.Employee!.FullNameAr,
                    StartDate = obj.StartDate,
                    EndDate = obj.EndDate,
                    TotalSalary = obj.TotalSalary,
                    FixedSalary = obj.FixedSalary,
                    HousingAllowance = obj.HousingAllowance,
                    ContractFile =
                        ManageFilesHelper.ConvertFileToBase64(Path.Combine(GoRootPath.HRFilesPath, obj.FileName)),
                    Details = obj.ListOfAllowancesDetails.Select(detail => new GetAllContractDetailsResponse()
                    {
                        AllowanceId = detail.AllowanceId,
                        Value = detail.Value,
                        IsPercent = detail.IsPercent
                    }).ToList()

                },
                Check = true
            };
        }

        public async Task<Response<CreateContractRequest>> CreateContractAsync(CreateContractRequest model)
        {
            var newContract = mapper.Map<HrContract>(model);
            GetFileNameAndExtension contractFile = new();
            if (!string.IsNullOrEmpty(model.ContractFile))
            {

                contractFile = ManageFilesHelper.UploadFile(model.ContractFile, GoRootPath.HRFilesPath);
            }

            newContract.FileName = contractFile?.FileName;
            newContract.FileExtension = contractFile?.FileExtension;
            await unitOfWork.Contracts.AddAsync(newContract);
            await unitOfWork.CompleteAsync();

            return new()
            {
                Msg = string.Format(shareLocalizer[Localization.Done],
                    shareLocalizer[Localization.Contract]),
                Check = true,
                Data = model
            };

        }


        public Task<Response<string>> UpdateActiveOrNotContractAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<CreateContractRequest>> UpdateContractAsync(int id, CreateContractRequest model)
        {
            Expression<Func<HrContract, bool>> filter = x => x.IsDeleted == false && x.Id == id;
            var obj = await unitOfWork.Contracts.GetFirstOrDefaultAsync(filter,
                includeProperties: $"{nameof(_instanceContract.ListOfAllowancesDetails)}");
            if (obj is null)
            {
                string resultMsg = string.Format(shareLocalizer[Localization.CannotBeFound],
                    shareLocalizer[Localization.Contract]);
                return new()
                {
                    Check = false,
                    Data = model,
                    Error = resultMsg,
                    Msg = resultMsg
                };
            }

            if (!string.IsNullOrEmpty(obj.FileName))
            {
                ManageFilesHelper.RemoveFile(Path.Combine(GoRootPath.HRFilesPath, obj.FileName));
            }

            obj.EmployeeId = model.EmployeeId;
            obj.EndDate = model.EndDate;
            obj.StartDate = model.StartDate;
            obj.TotalSalary = model.TotalSalary;
            obj.FixedSalary = model.FixedSalary;
            obj.HousingAllowance = model.HousingAllowance;


            var oldDetails = obj.ListOfAllowancesDetails;
            var newDetails = model.Details;

            int[] oldIds = oldDetails.Select(c => c.Id).ToArray();
            int[] newIds = newDetails.Select(c => c.Id).ToArray();

            //New Details Inserted
            if (newDetails.Any(d => d.Id == 0))
            {
                foreach (var newItem in newDetails.Where(d => d.Id == 0))
                {
                    await unitOfWork.ContractAllowancesDetails.AddAsync(new()
                    {
                        AllowanceId = newItem.AllowanceId,
                        Value = newItem.Value,
                        IsPercent = newItem.IsPercent,
                        ContractId = id
                    });
                }

            }

            //New Details Inserted
            if (newDetails.Any(d => d.Id > 0))
            {
                foreach (var updatedItem in newDetails.Where(d => d.Id > 0))
                {

                    var oldObj = await unitOfWork.ContractAllowancesDetails.GetByIdAsync(updatedItem.Id);
                    if (oldObj != null)
                    {
                        oldObj.AllowanceId = updatedItem.AllowanceId;
                        oldObj.Value = updatedItem.Value;
                        oldObj.IsPercent = updatedItem.IsPercent;
                        oldObj.ContractId = id;
                        unitOfWork.ContractAllowancesDetails.Update(oldObj);
                    }
                }
            }

            //Deleted Items
            var deletedIds = oldIds.Except(newIds);
            if (deletedIds.Any())
            {
                foreach (var deletedItem in deletedIds)
                {
                    var existItem
                        = await unitOfWork.ContractAllowancesDetails.GetByIdAsync(deletedItem);
                    if (existItem != null)
                    {
                        unitOfWork.ContractAllowancesDetails.Remove(existItem);
                    }
                }
            }

            GetFileNameAndExtension contractFile = new();
            if (model.ContractFile is not null)
            {

                if (model.ContractFile != null)
                {
                    contractFile = ManageFilesHelper.UploadFile(model.ContractFile, GoRootPath.HRFilesPath);
                }

            }

            obj.FileName = contractFile?.FileName;
            obj.FileExtension = contractFile?.FileExtension;
            unitOfWork.Contracts.Update(obj);
            await unitOfWork.CompleteAsync();

            return new()
            {
                Msg = string.Format(shareLocalizer[Localization.Done],
                    shareLocalizer[Localization.Contract]),
                Check = true,
                Data = model
            };


        }

        public async Task<Response<string>> DeleteContractAsync(int id)
        {
           using  var transaction = unitOfWork.BeginTransaction(); 
            try
            {
                var contractExist = await unitOfWork.Contracts.GetFirstOrDefaultAsync(c => c.Id == id,
                    $"{_instanceContract.ListOfAllowancesDetails}");
                if (contractExist is null)
                {
                    string resultMsg = string.Format(shareLocalizer[Localization.CannotBeFound],
                        shareLocalizer[Localization.Contract]);
                    return new()
                    {
                        Check = false,
                        Error = resultMsg,
                        Msg = resultMsg,
                        Data = string.Empty
                    };
                }

                if (!string.IsNullOrEmpty(contractExist.FileName))
                {
                    ManageFilesHelper.RemoveFile(Path.Combine(GoRootPath.HRFilesPath, contractExist.FileName));
                }

                var contractDetails = await unitOfWork.ContractAllowancesDetails.GetAllAsync(c => c.ContractId == id);
                if (contractDetails.Any())
                {
                    unitOfWork.ContractAllowancesDetails.RemoveRange(contractDetails);
                }

                unitOfWork.Contracts.Remove(contractExist);
                await unitOfWork.CompleteAsync();
                transaction.Commit();
                return new()
                {
                    Check = true,
                    Data = string.Empty,
                    Error = string.Empty,
                    Msg = string.Format(shareLocalizer[Localization.Deleted], shareLocalizer[Localization.Contract])
                };
            }
            catch (Exception e)
            {
                transaction.Rollback();
                return new()
                {
                    Check = true,
                    Data = string.Empty,
                    Error = string.Empty,

                };


            }








        }
    }
}
