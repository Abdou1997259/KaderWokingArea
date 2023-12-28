

using Microsoft.AspNetCore.Mvc;
using System.Xml;

namespace Kader_System.Services.Services.HR
{
    public class VacationService(IUnitOfWork unitOfWork, IStringLocalizer<SharedResource> sharLocalizer, IMapper mapper) : IVacationService
    {
        HrVacation instanceVacation;
        public async Task<Response<IEnumerable<SelectListResponse>>> ListOfVacationsAsync(string lang)
        {
            var result =
                await unitOfWork.Vacations.GetSpecificSelectAsync(null!,
                    select: x => new SelectListResponse
                    {
                        Id = x.Id,
                        Name = lang == Localization.Arabic ? x.NameAr : x.NameEn,
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

 

   

        public async Task<Response<GetAllVacationResponse>> GetAllVacationsAsync(string lang, GetAllFilterationFoVacationRequest model)
        {
            Expression<Func<HrVacation, bool>> filter = x => x.IsDeleted == model.IsDeleted;

            
            var result = new GetAllVacationResponse()
            {
                TotalRecords = await unitOfWork.Vacations.CountAsync(filter: filter),

                Items = (await unitOfWork.Vacations.GetSpecificSelectAsync
                    (filter: filter,
                    includeProperties: nameof(instanceVacation.VacationType),
                    take: model.PageSize,
                    skip: (model.PageNumber - 1) * model.PageSize,
                    select: x => new VacationData()
                    {
                        Id = x.Id,
                        Name = lang == Localization.Arabic ? x.NameAr : x.NameEn,
                        VacationType = x.VacationType.Name,
                        ApplyAfterMonth = x.ApplyAfterMonth,
                        TotalBalance = x.TotalBalance,
                        CanTransfer = x.CanTransfer,
                        EmployeesCount = 0
                    }
                    , orderBy: x =>
                        x.OrderByDescending(x => x.Id)))
                    .ToList()
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


        public async Task<Response<GetAllVacationResponse>> GetAllVacationsWithJoinAsync(string lang, GetAllFilterationFoVacationRequest model)
        {
            Expression<Func<HrVacation, bool>> filter = x => x.IsDeleted == model.IsDeleted;

            var result = new GetAllVacationResponse()
            {
                TotalRecords = await unitOfWork.Vacations.CountAsync(filter: filter),

                Items = ( unitOfWork.Vacations.GetVacationInfo
                    (   filter,
                        take: model.PageSize,
                        skip: (model.PageNumber - 1) * model.PageSize))
                        
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

        public async Task<Response<GetVacationDetailsByIdResponse>> GetVacationByIdAsync(int id)
        {

            var obj = await unitOfWork.Vacations.GetFirstOrDefaultAsync(v => v.Id == id, nameof(instanceVacation.VacationType));

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
                    NameEn = obj.NameAr,
                    NameAr = obj.NameEn,
                    ApplyAfterMonth = obj.ApplyAfterMonth,
                    CanTransfer = obj.CanTransfer,
                    VacationType = obj.VacationType.Name,
                    Balance = obj.TotalBalance
                },
                Check = true
            };
        }
        public async Task<Response<CreateVacationRequest>> CreateVacationAsync(CreateVacationRequest model)
        {
            bool exists = false;
            exists = await unitOfWork.Vacations.ExistAsync(x => x.NameAr.Trim() == model.NameAr
                                                                      && x.NameEn.Trim() == model.NameEn.Trim());

            if (exists)
            {
                string resultMsg = string.Format(sharLocalizer[Localization.IsExist],
                    sharLocalizer[Localization.Vacation]);

                return new()
                {
                    Error = resultMsg,
                    Msg = resultMsg
                };
            }

            var newVacation = new HrVacation()
            {

                NameEn = model.NameEn,
                NameAr = model.NameAr,
                CanTransfer = model.CanTransfer,
                TotalBalance = model.TotalBalance,
                ApplyAfterMonth = model.ApplyAfterMonth,
                VacationTypeId = model.VacationTypeId,
            };
            foreach (var vacationDistribution in model.VacationDistributions)
            {
                newVacation.VacationDistributions.Add(new HrVacationDistribution()
                {
                    NameAr = vacationDistribution.NameAr,
                    NameEn = vacationDistribution.NameEn,
                    DaysCount = vacationDistribution.DaysCount,
                    SalaryCalculatorId = vacationDistribution.SalaryCalculatorId,

                });
            }



            await unitOfWork.Vacations.AddAsync(newVacation);

            await unitOfWork.CompleteAsync();

            return new()
            {
                Msg = sharLocalizer[Localization.Done],
                Check = true,
                Data = model
            };
        }

        public async Task<Response<UpdateVacationRequest>> UpdateVacationAsync([FromRoute] int id, UpdateVacationRequest model)
        {


            HrVacation instanceVacation;
            var obj = await unitOfWork.Vacations.GetFirstOrDefaultAsync(v => v.Id == id, nameof(instanceVacation.VacationDistributions));

            if (obj == null)
            {
                string resultMsg = string.Format(sharLocalizer[Localization.CannotBeFound],
                    sharLocalizer[Localization.Vacation]);

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

                obj.NameAr = model.NameAr;
                obj.NameEn = model.NameEn;
                obj.ApplyAfterMonth = model.ApplyAfterMonth;
                obj.CanTransfer = model.CanTransfer;
                obj.TotalBalance = model.TotalBalance;
                obj.VacationTypeId = model.VacationTypeId;
                unitOfWork.Vacations.Update(obj);

                var oldDistributions = obj.VacationDistributions;
                var newDistributions = model.VacationDistributions;

                int[] oldIds = oldDistributions.Select(c => c.Id).ToArray();
                int[] newIds = newDistributions.Select(c => c.Id).ToArray();

                //New Details Inserted
                if (newDistributions.Any(d => d.Id == 0))
                {
                    foreach (var newItem in newDistributions.Where(d => d.Id == 0))
                    {
                        await unitOfWork.VacationDistributions.AddAsync(new()
                        {
                            NameAr = newItem.NameAr,
                            NameEn = newItem.NameEn,
                            VacationId = id,
                            SalaryCalculatorId = newItem.SalaryCalculatorId,
                            DaysCount = newItem.DaysCount,

                        });
                    }

                }
                //New Details Inserted
                if (newDistributions.Any(d => d.Id > 0))
                {
                    foreach (var updatedItem in newDistributions.Where(d => d.Id > 0))
                    {

                        var oldObj = await unitOfWork.VacationDistributions.GetByIdAsync(updatedItem.Id);
                        if (oldObj != null)
                        {
                            oldObj.NameEn = updatedItem.NameEn;
                            oldObj.NameAr = updatedItem.NameAr;
                            oldObj.DaysCount = updatedItem.DaysCount;
                            oldObj.SalaryCalculatorId = updatedItem.SalaryCalculatorId;
                            oldObj.VacationId = updatedItem.VacationId;
                            unitOfWork.VacationDistributions.Update(oldObj);
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
                            = await unitOfWork.VacationDistributions.GetByIdAsync(deletedItem);
                        if (existItem != null)
                        {
                            unitOfWork.VacationDistributions.Remove(existItem);
                        }
                    }
                }

                await unitOfWork.CompleteAsync();
                transaction.Commit();
                return new()
                {
                    Check = true,
                    Data = model,
                    Msg = sharLocalizer[Localization.Updated]
                };
            }
            catch (Exception e)
            {
                transaction.Rollback();
                return new()
                {
                    Data = model,
                    Error = e.Message,
                    Msg = e.Message
                };
            }
        }


        public async Task<Response<string>> DeleteVacationAsync(int id)
        {

            var obj = await unitOfWork.Vacations.GetFirstOrDefaultAsync(v => v.Id == id);

            if (obj == null)
            {
                string resultMsg = string.Format(sharLocalizer[Localization.CannotBeFound],
                    sharLocalizer[Localization.Vacation]);

                return new()
                {
                    Data = string.Empty,
                    Error = resultMsg,
                    Msg = resultMsg
                };
            }

            var vacationIsUsed = await unitOfWork.Employees.AnyAsync(e => e.Vacation_id == id);
            if (vacationIsUsed)
            {
                string resultMsg = string.Format(sharLocalizer[Localization.Used],
                    sharLocalizer[Localization.Vacation]);

                return new()
                {
                    Data = string.Empty,
                    Error = resultMsg,
                    Msg = resultMsg
                };
            }


            using var transaction = unitOfWork.BeginTransaction();
            try
            {


                unitOfWork.Vacations.Remove(obj);

                var vacationDistribution = await unitOfWork.VacationDistributions.GetAllAsync(v => v.VacationId == id);
                if (vacationDistribution.Any())
                {
                    unitOfWork.VacationDistributions.RemoveRange(vacationDistribution);

                }

                await unitOfWork.CompleteAsync();
                transaction.Commit();
                return new()
                {
                    Check = true,
                    Data = string.Empty,
                    Msg = sharLocalizer[Localization.Deleted]
                };
            }
            catch (Exception e)
            {
                transaction.Rollback();
                return new()
                {
                    Data = string.Empty,
                    Error = e.Message,
                    Msg = e.Message
                };
            }
        }


        public Task<Response<string>> UpdateActiveOrNotVacationAsync(int id)
        {
            throw new NotImplementedException();
        }


    }
}
