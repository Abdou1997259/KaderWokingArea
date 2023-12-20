namespace Kader_System.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IDatabaseTransaction BeginTransaction();

    IUserRepository Users { get; }
    IRoleClaimRepository RoleClaims { get; }
    IUserDeviceRepository UserDevices { get; }
    IRoleRepository Roles { get; }
    IUserRoleRepository UserRoles { get; }
    ISubMainScreenRepository SubMainScreens { get; }
    ISubMainScreenActionRepository SubMainScreenActions { get; }
    IMainScreenRepository MainScreens { get; }
    IMainScreenCategoryRepository MainScreenCategories { get; }


    IAccountingWayRepository AccountingWays { get; }
    IAllowanceRepository Allowances { get; }
    IBenefitRepository Benefits { get; }
    ICompanyRepository Companies { get; }
    ICompanyTypeRepository CompanyTypes { get; }
    IContractAllowancesDetailRepository ContractAllowancesDetails { get; }
    IContractRepository Contracts { get; }
    IDeductionRepository Deductions { get; }
    IDepartmentRepository Departments { get; }
    IEmployeeAttachmentRepository EmployeeAttachments { get; }
    IEmployeeRepository Employees { get; }
    IEmployeeTypeRepository EmployeeTypes { get; }
    IFingerPrintRepository FingerPrints { get; }
    IJobRepository Jobs { get; }
    IQualificationRepository Qualifications { get; }
    ISalaryPaymentWayRepository SalaryPaymentWays { get; }
    ISectionDepartmentRepository SectionDepartments { get; }
    ISectionRepository Sections { get; }
    IShiftRepository Shifts { get; }
    IVacationDistributionRepository VacationDistributions { get; }
    IVacationRepository Vacations { get; }
    IVacationTypeRepository VacationTypes { get; }


    ITransAllowanceRepository TransAllowances { get; }
    ITransSalaryEffectRepository TransSalaryEffects { get; }
    ITransAmountTypeRepository TransAmountTypes { get; }
    ITransBenefitRepository TransBenefits { get; }
    ITransCovenantRepository TransCovenants { get; }
    ITransDeductionRepository TransDeductions { get; }


    Task<int> CompleteAsync();
}
