using Kader_System.Domain.DTOs;
using Microsoft.Extensions.Hosting;

namespace Kader_System.Services.Services.Trans
{
    public class TransBenefitService(IUnitOfWork unitOfWork, IStringLocalizer<SharedResource> sharLocalizer, IMapper mapper) : ITransBenefitService
    {
        private TransBenefit _insatance;
        public async Task<Response<IEnumerable<SelectListOfTransBenefitResponse>>> ListOfTransBenefitsAsync(string lang)
        {
            var result =
                await unitOfWork.TransBenefits.GetSpecificSelectAsync(null!,
                    includeProperties: $"{nameof(_insatance.Benefit)},{nameof(_insatance.Employee)},{nameof(_insatance.SalaryEffect)}" +
                                       $",{nameof(_insatance.AmountType)}",
                    select: x => new SelectListOfTransBenefitResponse
                    {
                        Id = x.Id,
                        ActionMonth = x.ActionMonth,
                        SalaryEffect = lang == Localization.Arabic ? x.SalaryEffect!.Name : x.SalaryEffect!.NameInEnglish,
                        AddedOn = x.Add_date,
                        BenefitId = x.BenefitId,
                        BenefitName = lang == Localization.Arabic ? x.Benefit!.Name_ar : x.Benefit!.Name_en,
                        Amount = x.Amount,
                        EmployeeId = x.EmployeeId,
                        EmployeeName = lang == Localization.Arabic ? x.Employee!.FullNameAr : x.Employee!.FullNameEn,
                        Notes = x.Notes,
                        SalaryEffectId = x.SalaryEffectId,
                        AmountTypeId = x.AmountTypeId,
                        ValueTypeName = lang == Localization.Arabic ? x.AmountType!.Name : x.AmountType!.NameInEnglish
                    }, orderBy: x =>
                        x.OrderByDescending(x => x.Id));

            if (!result.Any())
            {
                string resultMsg = sharLocalizer[Localization.NotFoundData];

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

        public async Task<Response<GetAllTransBenefitResponse>> GetAllTransBenefitsAsync(string lang, 
            GetAllFilterationForTransBenefitRequest model,string host)
        {
            Expression<Func<TransBenefit, bool>> filter = x => x.IsDeleted == model.IsDeleted;
            var totalRecords = await unitOfWork.TransBenefits.CountAsync(filter: filter);
            int page = 1;
            int totalPages = (int)Math.Ceiling((double)totalRecords / (model.PageSize == 0 ? 10 : model.PageSize));
            if (model.PageNumber < 1)
                page = 1;
            else
                page = model.PageNumber;
            var pageLinks = Enumerable.Range(1, totalPages)
                .Select(p => new Link() { label = p.ToString(), url = host + $"?PageSize={model.PageSize}&PageNumber={p}&IsDeleted={model.IsDeleted}", active = p == model.PageNumber })
                .ToList();
            var result = new GetAllTransBenefitResponse
            {
                TotalRecords = totalRecords,

                Items = (await unitOfWork.TransBenefits.GetSpecificSelectAsync(filter: filter,
                    includeProperties: $"{nameof(_insatance.Benefit)},{nameof(_insatance.Employee)},{nameof(_insatance.SalaryEffect)}" +
                                       $",{nameof(_insatance.AmountType)}",
                    take: model.PageSize,
                    skip: (model.PageNumber - 1) * model.PageSize,
                    select: x => new TransBenefitData()
                    {
                        Id = x.Id,
                        ActionMonth = x.ActionMonth,
                        SalaryEffect = lang == Localization.Arabic ? x.SalaryEffect!.Name : x.SalaryEffect!.NameInEnglish,
                        AddedOn = x.Add_date,
                        BenefitId = x.BenefitId,
                        BenefitName = lang == Localization.Arabic ? x.Benefit!.Name_ar : x.Benefit!.Name_en,
                        Amount = x.Amount,
                        EmployeeId = x.EmployeeId,
                        EmployeeName = lang == Localization.Arabic ? x.Employee!.FullNameAr : x.Employee!.FullNameEn,
                        Notes = x.Notes,
                        SalaryEffectId = x.SalaryEffectId,
                        AmountTypeId = x.AmountTypeId,
                        ValueTypeName = lang == Localization.Arabic ? x.AmountType!.Name : x.AmountType!.NameInEnglish,
                        AttachmentFile = ManageFilesHelper.ConvertFileToBase64(GoRootPath.TransFilesPath+ x.Attachment)

                    }, orderBy: x =>
                        x.OrderByDescending(x => x.Id))).ToList(),
                CurrentPage = model.PageNumber,
                FirstPageUrl = host + $"?PageSize={model.PageSize}&PageNumber=1&IsDeleted={model.IsDeleted}",
                From = (page - 1) * model.PageSize + 1,
                To = Math.Min(page * model.PageSize, totalRecords),
                LastPage = totalPages,
                LastPageUrl = host + $"?PageSize={model.PageSize}&PageNumber={totalPages}&IsDeleted={model.IsDeleted}",
                PreviousPage = page > 1 ? host + $"?PageSize={model.PageSize}&PageNumber={page - 1}&IsDeleted={model.IsDeleted}" : null,
                NextPageUrl = page < totalPages ? host + $"?PageSize={model.PageSize}&PageNumber={page + 1}&IsDeleted={model.IsDeleted}" : null,
                Path = host,
                PerPage = model.PageSize,
                Links = pageLinks
            };

            if (result.TotalRecords is 0)
            {
                string resultMsg = sharLocalizer[Localization.NotFoundData];

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

        public async Task<Response<CreateTransBenefitRequest>> CreateTransBenefitAsync(CreateTransBenefitRequest model)
        {
            var newTrans = mapper.Map<TransBenefit>(model);

            if (!string.IsNullOrEmpty(model.Attachment))
            {
                var fileNameAndExt = ManageFilesHelper.SaveBase64StringToFile(model.Attachment!, GoRootPath.TransFilesPath, model.FileName!);
                if (fileNameAndExt != null)
                {
                    newTrans.Attachment = fileNameAndExt.FileName;
                    newTrans.AttachmentExtension = fileNameAndExt.FileExtension;
                }
                else
                {
                    newTrans.Attachment = null;
                    newTrans.AttachmentExtension = null;
                }
            }

            await unitOfWork.TransBenefits.AddAsync(newTrans);
            await unitOfWork.CompleteAsync();
            return new()
            {
                Msg = sharLocalizer[Localization.Done],
                Check = true,
                Data = model
            };
        }

        public async Task<Response<GetTransBenefitById>> GetTransBenefitByIdAsync(int id)
        {
            var obj = await unitOfWork.TransBenefits.GetByIdAsync(id);

            if (obj is null)
            {
                string resultMsg = sharLocalizer[Localization.NotFoundData];

                return new()
                {
                    Data = new(),
                    Error = resultMsg,
                    Msg = resultMsg
                };
            }

            return new()
            {
                Data = mapper.Map<GetTransBenefitById>(obj),
                Check = true
            };
        }

        public async Task<Response<GetTransBenefitById>> UpdateTransBenefitAsync(int id, CreateTransBenefitRequest model)
        {
            var obj = await unitOfWork.TransBenefits.GetByIdAsync(id);
            if (obj is null)
            {
                string resultMsg = sharLocalizer[Localization.NotFoundData];

                return new()
                {
                    Data = new(),
                    Error = resultMsg,
                    Msg = resultMsg
                };
            }

            if (!string.IsNullOrEmpty(obj.Attachment))
            {
                ManageFilesHelper.RemoveFile(Path.Combine(GoRootPath.TransFilesPath, obj.Attachment));
            }

            if (!string.IsNullOrEmpty(model.Attachment))
            {
                var fileNameAndExt = ManageFilesHelper.SaveBase64StringToFile(model.Attachment!, GoRootPath.TransFilesPath, model.FileName!);

                obj.Attachment = fileNameAndExt?.FileName;
                obj.AttachmentExtension = fileNameAndExt?.FileExtension;

            }
            else
            {
                obj.Attachment = null;
                obj.AttachmentExtension = null;
            }

            obj.Amount = model.Amount;
            obj.AmountTypeId=model.AmountTypeId;
            obj.SalaryEffectId=model.SalaryEffectId;
            obj.BenefitId=model.BenefitId;
            obj.ActionMonth=model.ActionMonth;
            obj.Notes=model.Notes;
            obj.EmployeeId=model.EmployeeId;
            unitOfWork.TransBenefits.Update(obj);
            await unitOfWork.CompleteAsync();
            return new()
            {
                Msg = sharLocalizer[Localization.Done],
                Check = true,
                Data = mapper.Map<GetTransBenefitById>(obj)
            };
        }

        public Task<Response<string>> UpdateActiveOrNotTransBenefitAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<string>> DeleteTransBenefitAsync(int id)
        {
            var obj = await unitOfWork.TransBenefits.GetByIdAsync(id);
            if (obj is null)
            {
                string resultMsg = sharLocalizer[Localization.NotFoundData];

                return new()
                {
                    Data = string.Empty,
                    Error = resultMsg,
                    Msg = resultMsg
                };
            }

            if (!string.IsNullOrEmpty(obj.Attachment))
            {
                ManageFilesHelper.RemoveFile(GoRootPath.TransFilesPath+obj.Attachment);
            }

            unitOfWork.TransBenefits.Remove(obj);
            await unitOfWork.CompleteAsync();
            return new()
            {
                Check = true,
                Data = string.Empty,
                Msg = sharLocalizer[Localization.Deleted]
            };
        }
    }
}
