
using Kader_System.Domain.DTOs;
using Microsoft.Extensions.Hosting;

namespace Kader_System.Services.Services.Trans;
public class TransAllowanceService(IUnitOfWork unitOfWork, IStringLocalizer<SharedResource> sharLocalizer, IMapper mapper) :ITransAllowanceService
{
    private TransAllowance _insatance;

    #region Retreive

    public async Task<Response<IEnumerable<SelectListForTransAllowancesResponse>>> ListOfTransAllowancesAsync(string lang)
    {
        var result =
            await unitOfWork.TransAllowances.GetSpecificSelectAsync(null!,
                includeProperties: $"{nameof(_insatance.Allowance)},{nameof(_insatance.Employee)},{nameof(_insatance.SalaryEffect)}",
                select: x => new SelectListForTransAllowancesResponse
                {
                    Id = x.Id,
                    ActionMonth = x.ActionMonth,
                    SalaryEffect = lang == Localization.Arabic ? x.SalaryEffect!.Name : x.SalaryEffect!.NameInEnglish,
                    AddedOn = x.Add_date,
                    AllowanceId = x.AllowanceId,
                    AllowanceName = lang == Localization.Arabic ? x.Allowance!.Name_ar : x.Allowance!.Name_en,
                    Amount = x.Amount,
                    EmployeeId = x.EmployeeId,
                    EmployeeName = lang == Localization.Arabic ? x.Employee!.FullNameAr : x.Employee!.FullNameEn,
                    Notes = x.Notes,
                    SalaryEffectId = x.SalaryEffectId
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

    public async Task<Response<TransAllowanceGetAllResponse>> GetAllTransAllowancesAsync(string lang, 
        GetAllFilterationAllowanceRequest model,string host)
    {
        Expression<Func<TransAllowance, bool>> filter = x => x.IsDeleted == model.IsDeleted;
        var totalRecords = await unitOfWork.TransAllowances.CountAsync(filter: filter);
        int page = 1;
        int totalPages = (int)Math.Ceiling((double)totalRecords / (model.PageSize == 0 ? 10 : model.PageSize));
        if (model.PageNumber < 1)
            page = 1;
        var pageLinks = Enumerable.Range(1, totalPages)
            .Select(p => new Link() { label = p.ToString(), url = host + $"?PageSize={model.PageSize}&PageNumber={p}&IsDeleted={model.IsDeleted}", active = p == model.PageNumber })
            .ToList();
        var result = new TransAllowanceGetAllResponse
        {
            TotalRecords = totalRecords,

            Items = (await unitOfWork.TransAllowances.GetSpecificSelectAsync(filter: filter,
                includeProperties: $"{nameof(_insatance.Allowance)},{nameof(_insatance.Employee)},{nameof(_insatance.SalaryEffect)}",
                take: model.PageSize,
                skip: (model.PageNumber - 1) * model.PageSize,
                select: x => new TransAllowanceData()
                {
                    Id = x.Id,
                    ActionMonth = x.ActionMonth,
                    SalaryEffect = lang == Localization.Arabic ? x.SalaryEffect!.Name : x.SalaryEffect!.NameInEnglish,
                    AddedOn = x.Add_date,
                    AllowanceId = x.AllowanceId,
                    AllowanceName = lang == Localization.Arabic ? x.Allowance!.Name_ar : x.Allowance!.Name_en,
                    Amount = x.Amount,
                    EmployeeId = x.EmployeeId,
                    EmployeeName = lang == Localization.Arabic ? x.Employee!.FullNameAr : x.Employee!.FullNameEn,
                    Notes = x.Notes,
                    SalaryEffectId = x.SalaryEffectId

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

    public async Task<Response<TransactionAllowanceGetByIdResponse>> GetTransAllowanceByIdAsync(int id)
    {
        var obj = await unitOfWork.TransAllowances.GetByIdAsync(id);

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
            Data = mapper.Map<TransactionAllowanceGetByIdResponse>(obj),
            Check = true
        };
    }

    #endregion

    #region Create
    public async Task<Response<CreateTransAllowanceRequest>> CreateTransAllowanceAsync(CreateTransAllowanceRequest model)
    {

        var newTrans = mapper.Map<TransAllowance>(model);
        await unitOfWork.TransAllowances.AddAsync(newTrans);
        await unitOfWork.CompleteAsync();
        return new()
        {
            Msg = sharLocalizer[Localization.Done],
            Check = true,
            Data = model
        };
    }

    #endregion

    #region Update
    public async Task<Response<TransactionAllowanceGetByIdResponse>> UpdateTransAllowanceAsync(int id, CreateTransAllowanceRequest model)
    {
        var obj = await unitOfWork.TransAllowances.GetByIdAsync(id);
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

        obj.AllowanceId = model.AllowanceId;
        obj.Amount = model.Amount;
        obj.EmployeeId = model.EmployeeId;
        obj.Notes = model.Notes;
        obj.SalaryEffectId = model.SalaryEffectId;
        obj.ActionMonth = model.ActionMonth;
        unitOfWork.TransAllowances.Update(obj);
        await unitOfWork.CompleteAsync();
        return new()
        {
            Msg = sharLocalizer[Localization.Done],
            Check = true,
            Data = mapper.Map<TransactionAllowanceGetByIdResponse>(obj)
        };
    }

    public Task<Response<string>> UpdateActiveOrNotTransAllowanceAsync(int id)
    {
        throw new NotImplementedException();
    }


    #endregion

    #region Delete
    public async Task<Response<string>> DeleteTransAllowanceAsync(int id)
    {
        var obj = await unitOfWork.TransAllowances.GetByIdAsync(id);
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

        unitOfWork.TransAllowances.Remove(obj);
        await unitOfWork.CompleteAsync();
        return new()
        {
            Check = true,
            Data = string.Empty,
            Msg = sharLocalizer[Localization.Deleted]
        };
    }
    #endregion

}
