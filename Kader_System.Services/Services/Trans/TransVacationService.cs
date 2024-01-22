namespace Kader_System.Services.Services.Trans
{
    public class TransVacationService(IUnitOfWork unitOfWork, IStringLocalizer<SharedResource> sharLocalizer, IMapper mapper) : ITransVacationService
    {
        private TransVacation _insatance;
        public async Task<Response<IEnumerable<SelectListOfTransVacationResponse>>> ListOfTransVacationsAsync(string lang)
        {
            var result =
                await unitOfWork.TransVacations.GetSpecificSelectAsync(null!,
                    includeProperties: $"{nameof(_insatance.Vacation)},{nameof(_insatance.Employee)}" 
                                       ,
                    select: x => new SelectListOfTransVacationResponse
                    {
                        Id = x.Id,
                        StartDate = x.StartDate,
                        DaysCount = x.DaysCount,
                        VacationId = x.VacationId,
                        VacationName = lang == Localization.Arabic ? x.Vacation!.NameAr : x.Vacation!.NameEn,
                        EmployeeId = x.EmployeeId,
                        EmployeeName = lang == Localization.Arabic ? x.Employee!.FullNameAr : x.Employee!.FullNameEn,
                        Notes = x.Notes
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

        public async Task<Response<GetAllTransVacationResponse>> GetAllTransVacationsAsync(string lang, GetAllFilterationForTransVacationRequest model)
        {
            Expression<Func<TransVacation, bool>> filter = x => x.IsDeleted == model.IsDeleted;

            var result = new GetAllTransVacationResponse
            {
                TotalRecords = await unitOfWork.TransVacations.CountAsync(filter: filter),

                Items = (await unitOfWork.TransVacations.GetSpecificSelectAsync(filter: filter,
                    includeProperties: $"{nameof(_insatance.Vacation)},{nameof(_insatance.Employee)}",
                    take: model.PageSize,
                    skip: (model.PageNumber - 1) * model.PageSize,
                    select: x => new TransVacationData()
                    {
                        Id = x.Id,
                        StartDate = x.StartDate,
                        DaysCount = x.DaysCount,
                        VacationId = x.VacationId,
                        VacationName = lang == Localization.Arabic ? x.Vacation!.NameAr : x.Vacation!.NameEn,
                        EmployeeId = x.EmployeeId,
                        EmployeeName = lang == Localization.Arabic ? x.Employee!.FullNameAr : x.Employee!.FullNameEn,
                        Notes = x.Notes ,
                        AttachmentFile = ManageFilesHelper.ConvertFileToBase64(GoRootPath.TransFilesPath + x.Attachment)
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

        public async Task<Response<CreateTransVacationRequest>> CreateTransVacationAsync(CreateTransVacationRequest model)
        {
            var newTrans = mapper.Map<TransVacation>(model);

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

            await unitOfWork.TransVacations.AddAsync(newTrans);
            await unitOfWork.CompleteAsync();
            return new()
            {
                Msg = sharLocalizer[Localization.Done],
                Check = true,
                Data = model
            };
        }

        public async Task<Response<GetTransVacationById>> GetTransVacationByIdAsync(int id)
        {
            var obj = await unitOfWork.TransVacations.GetFirstOrDefaultAsync(v=>v.Id==id
                ,includeProperties: $"{nameof(_insatance.Vacation)},{nameof(_insatance.Employee)}");

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
                Data = mapper.Map<GetTransVacationById>(obj),
                Check = true
            };
        }

        public async Task<Response<GetTransVacationById>> UpdateTransVacationAsync(int id, CreateTransVacationRequest model)
        {
            var obj = await unitOfWork.TransVacations.GetByIdAsync(id);
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

            obj.DaysCount = model.DaysCount;
            obj.StartDate = model.StartDate;
            obj.VacationId = model.VacationId;
            obj.Notes = model.Notes;
            obj.EmployeeId = model.EmployeeId;
            unitOfWork.TransVacations.Update(obj);
            await unitOfWork.CompleteAsync();
            return new()
            {
                Msg = sharLocalizer[Localization.Done],
                Check = true,
                Data = mapper.Map<GetTransVacationById>(obj)
            };
        }

        public Task<Response<string>> UpdateActiveOrNotTransVacationAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<string>> DeleteTransVacationAsync(int id)
        {
            var obj = await unitOfWork.TransVacations.GetByIdAsync(id);
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
                ManageFilesHelper.RemoveFile(GoRootPath.TransFilesPath + obj.Attachment);
            }

            unitOfWork.TransVacations.Remove(obj);
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
