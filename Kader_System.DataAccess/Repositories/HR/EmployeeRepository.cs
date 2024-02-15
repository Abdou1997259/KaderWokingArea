using Kader_System.Domain.DTOs.Response.HR;
using Kader_System.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace Kader_System.DataAccess.Repositories.HR;

public class EmployeeRepository(KaderDbContext context) : BaseRepository<HrEmployee>(context), IEmployeeRepository
{

    public Response<GetEmployeeByIdResponse> GetEmployeeByIdAsync(int id, string lang)
    {
        try
        {
            var employeeAllowances = context.TransAllowances.Where(e => e.EmployeeId == id).ToList();
            var employeeVacations = context.TransVacations.Where(e => e.EmployeeId == id).ToList();


            var result = from employee in context.Employees
                join department in context.Departments on employee.DepartmentId equals department.Id into deptGroup
                from dept in deptGroup.DefaultIfEmpty()
                join management in context.Managements on employee.ManagementId equals management.Id into manGroup
                from man in manGroup.DefaultIfEmpty()
                join user in context.Users on employee.UserId equals user.Id into userGroup
                from usr in userGroup.DefaultIfEmpty()
                join qualification in context.HrQualifications on employee.QualificationId equals qualification.Id into
                    qualGroup
                from qual in qualGroup.DefaultIfEmpty()
                join job in context.HrJobs on employee.JobId equals job.Id into jobGroup
                from j in jobGroup.DefaultIfEmpty()
                join relegion in context.Relegions on employee.ReligionId equals relegion.Id into relegioGroup
                from r in relegioGroup.DefaultIfEmpty()

                join company in context.Companys on employee.CompanyId equals company.Id into companyGroup
                from com in companyGroup.DefaultIfEmpty()

                join nationality in context.Nationalities on employee.NationalityId equals nationality.Id into
                    nationalityGroup
                from nat in nationalityGroup.DefaultIfEmpty()

                join marital in context.MaritalStatus on employee.MaritalStatusId equals marital.Id into maritalGroup
                from ms in maritalGroup.DefaultIfEmpty()

                join shift in context.Shifts on employee.MaritalStatusId equals shift.Id into shiftGroup
                from sh in shiftGroup.DefaultIfEmpty()
                         where employee.Id == id
                select new GetEmployeeByIdResponse()
                {
                    FullNameAr = employee.FullNameAr,
                    FullNameEn = employee.FullNameEn,
                    ManagementId = employee.ManagementId,
                    CompanyId = employee.CompanyId,
                    FirstNameEn = employee.FirstNameEn,
                    FatherNameAr = employee.FatherNameAr,
                    FatherNameEn = employee.FatherNameEn,
                    GrandFatherNameAr = employee.GrandFatherNameAr,
                    GrandFatherNameEn = employee.GrandFatherNameEn,
                    FamilyNameAr = employee.FamilyNameAr,
                    FamilyNameEn = employee.FamilyNameEn,
                    FirstNameAr = employee.FirstNameAr,
                    IsActive = employee.IsActive,
                    VacationId = employee.VacationId,
                    AccountNo = employee.AccountNo,
                    Address = employee.Address,
                    BirthDate = employee.BirthDate,
                    ChildrenNumber = employee.ChildrenNumber,
                    DepartmentId = employee.DepartmentId,
                    Email = employee.Email,
                    EmployeeImageExtension = employee.EmployeeImageExtension,
                    EmployeeTypeId = employee.EmployeeTypeId,
                    FingerPrintCode = employee.FingerPrintCode,
                    FingerPrintId = employee.FingerPrintId,
                    FixedSalary = employee.FixedSalary,
                    GenderId = employee.GenderId,
                    HiringDate = employee.HiringDate,
                    ImmediatelyDate = employee.ImmediatelyDate,
                    Id = employee.Id,
                    JobId = employee.JobId,
                    JobNumber = employee.JobNumber,
                    MaritalStatusId = employee.MaritalStatusId,
                    NationalId = employee.NationalId,
                    NationalityId = employee.NationalityId,
                    Phone = employee.Phone,
                    QualificationId = employee.QualificationId,
                    ReligionId = employee.ReligionId,
                    SalaryPaymentWayId = employee.SalaryPaymentWayId,
                    ShiftId = employee.ShiftId,
                    TotalSalary = employee.TotalSalary,
                    Username = usr.UserName,
                    EmployeeImage = $"{GoRootPath.EmployeeImagesPath}{employee.EmployeeImage}",
                    qualification_name = lang == Localization.Arabic ? qual.NameAr : qual.NameEn,
                    company_name = lang == Localization.Arabic ? com.NameAr : com.NameEn,
                    management_name = lang == Localization.Arabic ? man.NameAr : man.NameEn,
                    employee_loans_count = 0,
                    vacation_days_count = employeeVacations.Sum(v=>v.DaysCount),
                    job_name = lang == Localization.Arabic ? j.NameAr : j.NameEn,
                    department_name = lang == Localization.Arabic ? dept.NameAr : dept.NameEn,
                    marital_status_name = lang == Localization.Arabic ? ms.Name : ms.NameInEnglish,
                    nationality_name = lang == Localization.Arabic ? nat.Name : nat.NameInEnglish,
                    religion_name = lang == Localization.Arabic ? r.Name : r.NameInEnglish,
                    note = employee.Note,
                    shift_name = lang == Localization.Arabic ?sh.Name_ar :sh.Name_en,
                    allowances_sum = employeeAllowances.Sum(a=>a.Amount),
                    employee_loans_sum = 0
                };
            if (result?.FirstOrDefault()?.Id == null || result?.FirstOrDefault()?.Id == 0)
            {
                return new()
                {
                    Msg = $"Employee with id {id} can not be found",
                    Check = false,
                    Data = null,
                    Error = string.Empty
                };
            }

            return new Response<GetEmployeeByIdResponse>()
            {
                Check = true,
                Data = result?.FirstOrDefault(),
                Error = string.Empty,
                Msg = string.Empty
            };
        }
        catch (Exception exception)
        {
            return new()
            {
                Msg = $"ERROR",
                Check = false,
                Data = null,
                Error = exception.InnerException!=null ? exception.InnerException.ToString() :exception.Message
            };
        }
        


    }

    public async Task<object> GetEmployeesDataAsLookUp(string lang)
    {
        return await context.Employees.
            Where(e => !e.IsDeleted && e.IsActive)
            .Include(j=>j.Job)
            .Include(j => j.Company)
            .Include(j => j.Nationality)
            .Include(j => j.Management)
            .Select(e => new
            {
                id=e.Id,
                employee_image=e.EmployeeImage,
                add_date=e.Add_date,
                employee_name=lang==Localization.Arabic? e.FullNameAr:e.FullNameEn,
                employee_job=lang==Localization.Arabic?e.Job!.NameAr:e.Job!.NameEn,
                gender=e.GenderId,
                hiring_date=e.HiringDate,
                salary_fixed=e.FixedSalary,
                salary_total=e.TotalSalary,
                immediately_date=e.ImmediatelyDate,
                is_active=e.IsActive,
                employee_serial=e.JobNumber,
                payment_method=e.SalaryPaymentWayId,
                note=e.Note,
                company_name=lang==Localization.Arabic? e.Company!.NameAr:e.Company!.NameEn,
                nationality_name=lang==Localization.Arabic?e.Nationality!.Name:e.Nationality!.NameInEnglish,
                management_id=e.ManagementId,
                management_name=lang==Localization.Arabic?e.Management!.NameAr:e.Management!.NameEn,
                department_id=e.DepartmentId,
                marital_status=e.MaritalStatusId
            }).ToListAsync();
    }
}
