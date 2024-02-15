﻿
using Kader_System.Domain.DTOs.Response.HR;

namespace Kader_System.DataAccess.Repositories.HR;

public class VacationRepository(KaderDbContext context) : BaseRepository<HrVacation>(context), IVacationRepository

{


    public List<VacationData> GetVacationInfo(
        Expression<Func<HrVacation, bool>> vacationFilter,
    int? skip = null,
    int? take = null)
    {
        var query = context.Set<HrVacation>()
            .Include(v => v.VacationType)
            .Where(vacationFilter)

            .GroupJoin(
                context.Set<HrEmployee>(),
                v => v.Id,
                e => e.VacationId,
                (v, employees) => new { Vacation = v, Employees = employees })
            .SelectMany(
                x => x.Employees.DefaultIfEmpty(),
                (vacation, employee) => new { vacation.Vacation, Employee = employee });


        var groupedQuery= query
                 .GroupBy(x => new { x.Vacation.Id, x.Vacation.NameAr, x.Vacation.NameEn,x.Vacation.ApplyAfterMonth,x.Vacation.TotalBalance,x.Vacation.CanTransfer ,x.Vacation.VacationType.Name })
                 .Select(group => new VacationData()
                 {
                     Id = group.Key.Id,
                     Name = group.Key.NameAr,
                     EmployeesCount = group.Count(x => x.Employee != null),
                     VacationType = group.Key.Name,
                     ApplyAfterMonth = group.Key.ApplyAfterMonth,
                     CanTransfer = group.Key.CanTransfer,
                     TotalBalance = group.Key.TotalBalance,
                     
                 });
        if (take.HasValue)
            groupedQuery = groupedQuery.Take(take.Value);
        if (skip.HasValue)
            groupedQuery = groupedQuery.Skip(skip.Value);
        return groupedQuery.ToList();

    }




}




