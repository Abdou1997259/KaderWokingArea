using Kader_System.Domain.DTOs.Response.Trans;

namespace Kader_System.DataAccess.Repositories.Trans;

public class TransCovenantRepository(KaderDbContext context) : BaseRepository<TransCovenant>(context), ITransCovenantRepository
{
    public List<TransCovenantData> GetTransCovenantDataInfo(
       Expression<Func<TransCovenant, bool>> filter,
       Expression<Func<TransCovenantData, bool>> filterSearch,
       int? skip = null,
       int? take = null, string lang = "ar"
      )
    {

        var transBenefits = context.TransCovenants.Where(filter);


        var query = from trans in transBenefits
                    join employee in context.Employees on trans.EmployeeId equals employee.Id into empGroup
                    from employee in empGroup.DefaultIfEmpty()
                    join u in context.Users on trans.Added_by equals u.Id into userGroup
                    from u in userGroup.DefaultIfEmpty()
            

                    select new TransCovenantData()
                    {
                        
                        AddedBy = u.UserName,
                        AddedOn = trans.Add_date,
                        Amount = trans.Amount,
                        EmployeeId = trans.EmployeeId,
                        EmployeeName = lang == Localization.Arabic ? employee.FullNameAr : employee.FullNameEn,
                        Id = trans.Id,
                        Notes = trans.Notes,
                        NameAr = trans.NameAr,
                        NameEn = trans.NameEn,
                        Date = trans.Date
                    };

        if (filterSearch != null)
            query = query.Where(filterSearch);
        if (take.HasValue)
            query = query.Take(take.Value);
        if (skip.HasValue)
            query = query.Skip(skip.Value);

        return query.ToList();

    }
}
