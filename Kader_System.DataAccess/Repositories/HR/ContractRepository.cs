using Kader_System.Domain.DTOs.Response.HR;

namespace Kader_System.DataAccess.Repositories.HR;

public class ContractRepository(KaderDbContext context) : BaseRepository<HrContract>(context), IContractRepository
{
    public  List<ContractData> GetAllContractsAsync(
        Expression<Func<HrContract, bool>> contractFilter,
        string lang,
        int? skip = null,
        int? take = null)
    {
        var query = (from con in context.Contracts
                     join u in context.Users on con.Added_by equals u.Id into userJoin
                     from u in userJoin.DefaultIfEmpty()
                     join emp in context.Employees on con.EmployeeId equals emp.Id
                     join d in context.ContractAllowancesDetails on con.Id equals d.ContractId into detailsJoin
                     from d in detailsJoin.DefaultIfEmpty()
                     join allow in context.Allowances on d.AllowanceId equals allow.Id into allowanceJoin
                     from allow in allowanceJoin.DefaultIfEmpty()
                     where con.IsDeleted == false
                     select new
                     {
                         con.Id,
                         con.TotalSalary,
                         con.FixedSalary,
                         con.EmployeeId,
                         emp!.FullNameAr,  // Use null-conditional operator
                         emp!.FullNameEn,  // Use null-conditional operator
                         con.StartDate,
                         con.EndDate,
                         con.HousingAllowance,
                         UserName = u!.UserName,  // Use null-conditional operator
                         AllowanceId = allow!.Id,  // Use null-conditional operator
                         AllowanceNameAr = allow!.Name_ar,  // Use null-conditional operator
                         IsPercent = d!.IsPercent,  // Use null-conditional operator
                         AllowanceValue = d!.Value,  // Use null-conditional operator
                         con.FileName
                     }
             into contractWithAllowances
                     group contractWithAllowances by new
                     {
                         contractWithAllowances.Id,
                         contractWithAllowances.TotalSalary,
                         contractWithAllowances.FixedSalary,
                         contractWithAllowances.EmployeeId,
                         contractWithAllowances.FullNameAr,
                         contractWithAllowances.FullNameEn,
                         contractWithAllowances.StartDate,
                         contractWithAllowances.EndDate,
                         contractWithAllowances.HousingAllowance,
                         contractWithAllowances.UserName,
                         contractWithAllowances.FileName
                     }
             into groupedContract
                     select new ContractData()
                     {
                         Id = groupedContract.Key.Id,
                         TotalSalary = groupedContract.Key.TotalSalary,
                         FixedSalary = groupedContract.Key.FixedSalary,
                         EmployeeId = groupedContract.Key.EmployeeId,
                         EmployeeName = lang == Localization.Arabic ? groupedContract.Key!.FullNameAr ?? "N/A" : groupedContract.Key!.FullNameEn ?? "N/A",
                         StartDate = groupedContract.Key.StartDate,
                         EndDate = groupedContract.Key.EndDate,
                         HousingAllowance = groupedContract.Key.HousingAllowance,
                         ContractFile = ManageFilesHelper.ConvertFileToBase64(GoRootPath.HRFilesPath + groupedContract.Key!.FileName ?? ""),
                         AddedByUser = groupedContract.Key!.UserName ?? "N/A",
                         Details = groupedContract.Select(a => new GetAllContractDetailsResponse()
                         {
                             AllowanceId = a.AllowanceId ,  // Provide a default value if AllowanceId is nullable
                             Value = a.AllowanceValue ,  // Provide a default value if AllowanceValue is nullable
                             AllowanceName = a.AllowanceNameAr ?? "N/A",  // Provide a default value if AllowanceNameAr is nullable
                             IsPercent = a.IsPercent   // Provide a default value if IsPercent is nullable
                         }).ToList()
                     });

        if (skip.HasValue)
            query = query.Skip(skip.Value);

        if (take.HasValue)
            query = query.Take(take.Value);

        return query.ToList();
    }


}

