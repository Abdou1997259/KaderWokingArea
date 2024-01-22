namespace Kader_System.Services.Services.Trans
{
    public class TransDeductionService(IUnitOfWork unitOfWork, IStringLocalizer<SharedResource> sharLocalizer, IMapper mapper) : ITransDeductionService
    {
        private TransDeduction _insatance;
        public async Task<Response<IEnumerable<SelectListOfTransDeductionResponse>>> ListOfTransDeductionsAsync(string lang)
        {
            var result =
                await unitOfWork.TransDeductions.GetSpecificSelectAsync(null!,
                    includeProperties: $"{nameof(_insatance.Deduction)},{nameof(_insatance.Employee)},{nameof(_insatance.SalaryEffect)}" +
                                       $",{nameof(_insatance.AmountType)}",
                    select: x => new SelectListOfTransDeductionResponse
                    {
                        Id = x.Id,
                        ActionMonth = x.ActionMonth,
                        SalaryEffect = lang == Localization.Arabic ? x.SalaryEffect!.Name : x.SalaryEffect!.NameInEnglish,
                        AddedOn = x.Add_date,
                        DeductionId = x.DeductionId,
                        DeductionName = lang == Localization.Arabic ? x.Deduction!.Name_ar : x.Deduction!.Name_en,
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

        public async Task<Response<GetAllTransDeductionResponse>> GetAllTransDeductionsAsync(string lang, GetAllFilterationForTransDeductionRequest model)
        {
            Expression<Func<TransDeduction, bool>> filter = x => x.IsDeleted == model.IsDeleted;

            var result = new GetAllTransDeductionResponse
            {
                TotalRecords = await unitOfWork.TransDeductions.CountAsync(filter: filter),

                Items = (await unitOfWork.TransDeductions.GetSpecificSelectAsync(filter: filter,
                    includeProperties: $"{nameof(_insatance.Deduction)},{nameof(_insatance.Employee)},{nameof(_insatance.SalaryEffect)}" +
                                       $",{nameof(_insatance.AmountType)}",
                    take: model.PageSize,
                    skip: (model.PageNumber - 1) * model.PageSize,
                    select: x => new TransDeductionData()
                    {
                        Id = x.Id,
                        ActionMonth = x.ActionMonth,
                        SalaryEffect = lang == Localization.Arabic ? x.SalaryEffect!.Name : x.SalaryEffect!.NameInEnglish,
                        AddedOn = x.Add_date,
                        DeductionId = x.DeductionId,
                        DeductionName = lang == Localization.Arabic ? x.Deduction!.Name_ar : x.Deduction!.Name_en,
                        Amount = x.Amount,
                        EmployeeId = x.EmployeeId,
                        EmployeeName = lang == Localization.Arabic ? x.Employee!.FullNameAr : x.Employee!.FullNameEn,
                        Notes = x.Notes,
                        SalaryEffectId = x.SalaryEffectId,
                        AmountTypeId = x.AmountTypeId,
                        ValueTypeName = lang == Localization.Arabic ? x.AmountType!.Name : x.AmountType!.NameInEnglish,
                        AttachmentFile = ManageFilesHelper.ConvertFileToBase64(GoRootPath.TransFilesPath+ x.Attachment)

                    }, orderBy: x =>
                        x.OrderByDescending(x => x.Id))).ToList()
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

        public async Task<Response<CreateTransDeductionRequest>> CreateTransDeductionAsync(CreateTransDeductionRequest model)
        {
            var newTrans = mapper.Map<TransDeduction>(model);

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

            await unitOfWork.TransDeductions.AddAsync(newTrans);
            await unitOfWork.CompleteAsync();
            return new()
            {
                Msg = sharLocalizer[Localization.Done],
                Check = true,
                Data = model
            };
        }

        public async Task<Response<GetTransDeductionById>> GetTransDeductionByIdAsync(int id)
        {
            var obj = await unitOfWork.TransDeductions.GetByIdAsync(id);

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
                Data = mapper.Map<GetTransDeductionById>(obj),
                Check = true
            };
        }

        public async Task<Response<GetTransDeductionById>> UpdateTransDeductionAsync(int id, CreateTransDeductionRequest model)
        {
            var obj = await unitOfWork.TransDeductions.GetByIdAsync(id);
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
            obj.DeductionId=model.DeductionId;
            obj.ActionMonth=model.ActionMonth;
            obj.Notes=model.Notes;
            obj.EmployeeId=model.EmployeeId;
            unitOfWork.TransDeductions.Update(obj);
            await unitOfWork.CompleteAsync();
            return new()
            {
                Msg = sharLocalizer[Localization.Done],
                Check = true,
                Data = mapper.Map<GetTransDeductionById>(obj)
            };
        }

        public Task<Response<string>> UpdateActiveOrNotTransDeductionAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<string>> DeleteTransDeductionAsync(int id)
        {
            var obj = await unitOfWork.TransDeductions.GetByIdAsync(id);
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

            unitOfWork.TransDeductions.Remove(obj);
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
