using Kader_System.Domain.DTOs;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Kader_System.Services.Services.HR
{
    public class HrJobService(IUnitOfWork unitOfWork, IStringLocalizer<SharedResource> sharLocalizer, IMapper mapper) : IHrJobService
    {
        private readonly IMapper _mapper = mapper;
        public async Task<Response<IEnumerable<SelectListResponse>>> ListOfJobsAsync(string lang)
        {
            var result =
                await unitOfWork.Jobs.GetSpecificSelectAsync(null!,
                    select: x => new SelectListResponse
                    {
                        Id = x.Id,
                        Name = lang == Localization.Arabic ? x.NameAr : x.NameEn
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

        public async Task<Response<HrGetAllJobsResponse>> GetAllJobsAsync(string lang, HrGetAllFilterationForJobRequest model,string host)
        {
            Expression<Func<HrJob, bool>> filter = x => x.IsDeleted == model.IsDeleted;
           

            var totalRecords = await unitOfWork.Jobs.CountAsync(filter: filter);
            int page = 1;
            int totalPages = (int)Math.Ceiling((double)totalRecords / (model.PageSize==0?10:model.PageSize));
            if (model.PageNumber < 1)
                page = 1;

            var pageLinks = Enumerable.Range(1, totalPages)
                .Select(p => new Link() { label = p.ToString(), url =host+ $"?PageSize={model.PageSize}&PageNumber={p}&IsDeleted={model.IsDeleted}", active = p==model.PageNumber})
                .ToList();
            var result = new HrGetAllJobsResponse
            {
                TotalRecords =totalRecords ,
               
            Items = (await unitOfWork.Jobs.GetSpecificSelectAsync(filter: filter,
                    take: model.PageSize,
                    skip: (model.PageNumber - 1) * model.PageSize,
                    select: x => new JobData()
                    {
                        Id = x.Id,
                        Name = lang == Localization.Arabic ? x.NameAr : x.NameEn
                    }, orderBy: x =>
                        x.OrderByDescending(x => x.Id))).ToList(),
                CurrentPage = model.PageNumber,
                FirstPageUrl =host+ $"?PageSize={model.PageSize}&PageNumber=1&IsDeleted={model.IsDeleted}",
                From = (page - 1) * model.PageSize + 1,
                To = Math.Min(page * model.PageSize, totalRecords),
                LastPage = totalPages,
                LastPageUrl = host + $"?PageSize={model.PageSize}&PageNumber={totalPages}&IsDeleted={model.IsDeleted}",
                PreviousPage = page > 1? host + $"?PageSize={model.PageSize}&PageNumber={page-1}&IsDeleted={model.IsDeleted}":null,
                NextPageUrl = page < totalPages ? host + $"?PageSize={model.PageSize}&PageNumber={page + 1}&IsDeleted={model.IsDeleted}":null,
                Path = host,
                PerPage = model.PageSize,
                Links = pageLinks,
                
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
                Check = true,
                
            };
        }

        public async Task<Response<HrCreateJobRequest>> CreateJobAsync(HrCreateJobRequest model)
        {
            bool exists = false;
            exists = await unitOfWork.Jobs.ExistAsync(x => x.NameAr.Trim() == model.NameAr
                                                              && x.NameEn.Trim() == model.NameEn.Trim());

            if (exists)
            {
                string resultMsg = string.Format(sharLocalizer[Localization.IsExist],
                    sharLocalizer[Localization.Qualification]);

                return new()
                {
                    Error = resultMsg,
                    Msg = resultMsg
                };
            }

            await unitOfWork.Jobs.AddAsync(new()
            {
                NameEn = model.NameEn,
                NameAr = model.NameAr
            });
            await unitOfWork.CompleteAsync();

            return new()
            {
                Msg = sharLocalizer[Localization.Done],
                Check = true,
                Data = model
            };
        }

        public async Task<Response<HrGetJobByIdResponse>> GetJobByIdAsync(int id)
        {
            var obj = await unitOfWork.Jobs.GetByIdAsync(id);

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
                Data = new()
                {
                    Id = id,
                    NameAr = obj.NameAr,
                    NameEn = obj.NameEn
                },
                Check = true
            };
        }

        public async Task<Response<HrUpdateJobRequest>> UpdateJobAsync(int id, HrUpdateJobRequest model)
        {
            var obj = await unitOfWork.Jobs.GetByIdAsync(id);

            if (obj == null)
            {
                string resultMsg = string.Format(sharLocalizer[Localization.CannotBeFound],
                    sharLocalizer[Localization.Qualification]);

                return new()
                {
                    Data = model,
                    Error = resultMsg,
                    Msg = resultMsg
                };
            }

            obj.NameAr = model.NameAr;
            obj.NameEn = model.NameEn;

            unitOfWork.Jobs.Update(obj);
            await unitOfWork.CompleteAsync();

            return new()
            {
                Check = true,
                Data = model,
                Msg = sharLocalizer[Localization.Updated]
            };
        }

        public Task<Response<string>> UpdateActiveOrNotBenefitAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<string>> DeleteJobAsync(int id)
        {
            var obj=await unitOfWork.Jobs.GetByIdAsync(id);
            if (obj == null)
            {
                string resultMsg = string.Format(sharLocalizer[Localization.CannotBeFound],
                    sharLocalizer[Localization.Job]);

                return new()
                {
                    Data = string.Empty,
                    Error = resultMsg,
                    Msg = resultMsg
                };
            }
            Expression<Func<HrEmployee, bool>> filter = x => x.JobId==id;
            var isUsed = await unitOfWork.Employees.GetFirstOrDefaultAsync(filter);
            if (isUsed == null)
            {
                unitOfWork.Jobs.Remove(obj);
                await unitOfWork.CompleteAsync();

                return new()
                {
                    Check = true,
                    Data = string.Empty,
                    Msg = sharLocalizer[Localization.Deleted]
                };
            }
            else
            {
                string resultMsg = string.Format(sharLocalizer[Localization.CannotDeleteItemHasRelativeData],
                    sharLocalizer[Localization.Job]);
                return new()
                {
                    Data = string.Empty,
                    Error = resultMsg,
                    Msg = resultMsg
                };
            }
        }
    }
}
