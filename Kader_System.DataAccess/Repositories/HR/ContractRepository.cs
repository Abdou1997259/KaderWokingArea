using Kader_System.Domain.DTOs.Response.HR;

namespace Kader_System.DataAccess.Repositories.HR;

public class ContractRepository(KaderDbContext context) : BaseRepository<HrContract>(context), IContractRepository
{

    public async Task<List<ContractData>>  GetAllContractsAsync(
        Expression<Func<HrContract, bool>> contractFilter,
        string lang,
        int? skip = null,
        int? take = null)
    {
        var query = context.Set<HrContract>().AsNoTracking()
            .Include(v => v.Employee)
            .Include(v => v.ListOfAllowancesDetails)
            .ThenInclude(d => d.Allowance)
            .IgnoreQueryFilters()
            .Where(contractFilter);

        if (skip.HasValue)
            query = query.Skip(skip.Value);
        if (take.HasValue)
            query = query.Take(take.Value);

        return await query.Select(x => new ContractData()
        {
            Id = x.Id,
            TotalSalary = x.TotalSalary,
            FixedSalary = x.FixedSalary,
            EmployeeId = x.EmployeeId,
            EmployeeName = lang == Localization.Arabic ? x.Employee!.FullNameAr : x.Employee!.FullNameEn,
            StartDate = x.StartDate,
            EndDate = x.EndDate,
            HousingAllowance = x.HousingAllowance,
            ContractFile = ManageFilesHelper.ConvertFileToBase64(Path.Combine(GoRootPath.HRFilesPath, x.FileName)),
            Details = x.ListOfAllowancesDetails.Select(details => new GetAllContractDetailsResponse()
            {
                AllowanceId = details.AllowanceId,
                IsPercent = details.IsPercent,
                Value = details.Value,
                AllowanceName = lang == Localization.Arabic ? details.Allowance!.Name_ar : details.Allowance!.Name_en,
            }

            ).ToList()
        }).ToListAsync();




    }
}
