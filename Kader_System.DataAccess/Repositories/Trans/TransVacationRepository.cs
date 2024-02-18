using Kader_System.Domain.DTOs.Response.Trans;

namespace Kader_System.DataAccess.Repositories.Trans
{
    public class TransVacationRepository(KaderDbContext context) : BaseRepository<TransVacation>(context), ITransVacationRepository
    {
        public List<TransVacationData> GetTransVacationInfo(
     Expression<Func<TransVacation, bool>> filter,
     Expression<Func<TransVacationData, bool>> filterSearch,
     int? skip = null,
     int? take = null, string lang = "ar"
    )
        {

            var transVacations = context.TransVacations.Where(filter);


            var query = from trans in transVacations
                        join employee in context.Employees on trans.EmployeeId equals employee.Id into empGroup
                        from employee in empGroup.DefaultIfEmpty()
                        join u in context.Users on trans.Added_by equals u.Id into userGroup
                        from u in userGroup.DefaultIfEmpty()
                        join vacation in context.Vacations on trans.VacationId equals vacation.Id into vacationGroup
                        from vacation in vacationGroup.DefaultIfEmpty()
                     

                        select new TransVacationData()
                        {
                            StartDate = trans.StartDate,
                            AddedBy = u.UserName,
                            DaysCount = trans.DaysCount,
                            VacationId = trans.VacationId,
                            VacationName = lang == Localization.Arabic ? vacation.NameAr : vacation.NameEn,
                            EmployeeId = trans.EmployeeId,
                            EmployeeName = lang == Localization.Arabic ? employee.FullNameAr : employee.FullNameEn,
                            Id = trans.Id,
                            Notes = trans.Notes,
                            
                         
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
}
