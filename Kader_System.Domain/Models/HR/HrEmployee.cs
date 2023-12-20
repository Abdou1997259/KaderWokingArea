namespace Kader_System.Domain.Models.HR;

[Table("Hr_Employees")]
public class HrEmployee : BaseEntity
{
    [Key]
    public int Id { get; set; }
    public required string Employee_name_ar { get; set; }
    public required string Employee_name_en { get; set; }
    public required string Father_name_ar { get; set; }
    public required string Father_name_en { get; set; }
    public required string Grand_father_name_ar { get; set; }
    public required string Grand_father_name_en { get; set; }
    public required string Family_name_ar { get; set; }
    public required string Family_name_en { get; set; }
    public required string Address { get; set; }
    public required string Fixed_salary { get; set; }
    public required DateOnly Hiring_date { get; set; }
    public required DateOnly Immediately_date { get; set; }
    public required double Total_salary { get; set; }
    public required string Phone { get; set; }
    public required string Email { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
    public required string Children_number { get; set; }
    public string? Employee_image { get; set; }
    public string? Employee_image_extension { get; set; }

    public int Gender_id { get; set; }
    [ForeignKey(nameof(Gender_id))]
    public HrGender Gender { get; set; } = default!;

    public int Relegion_id { get; set; }
    [ForeignKey(nameof(Relegion_id))]
    public HrRelegion Relegion { get; set; } = default!;

    public int SalaryPaymentWay_id { get; set; }
    [ForeignKey(nameof(SalaryPaymentWay_id))]
    public HrSalaryPaymentWay SalaryPaymentWay { get; set; } = default!;

    public int Shift_id { get; set; }
    [ForeignKey(nameof(Shift_id))]
    public HrShift Shift { get; set; } = default!;

    public int Department_id { get; set; }
    [ForeignKey(nameof(Department_id))]
    public HrDepartment Department { get; set; } = default!;

    public int Nationality_id { get; set; }
    [ForeignKey(nameof(Nationality_id))]
    public HrNationality Nationality { get; set; } = default!;

    public int Qualification_id { get; set; }
    [ForeignKey(nameof(Qualification_id))]
    public HrQualification Qualification { get; set; } = default!;

    public int AccountingWay_id { get; set; }
    [ForeignKey(nameof(AccountingWay_id))]
    public HrAccountingWay AccountingWay { get; set; } = default!;

    public int Vacation_id { get; set; }
    [ForeignKey(nameof(Vacation_id))]
    public HrVacation Vacation { get; set; } = default!;

    public int EmployeeType_id { get; set; }
    [ForeignKey(nameof(EmployeeType_id))]
    public HrEmployeeType EmployeeType { get; set; } = default!;

    public int Job_id { get; set; }
    [ForeignKey(nameof(Job_id))]
    public HrJob Job { get; set; } = default!;

    public ICollection<HrEmployeeAttachment> ListOfAttachments { get; set; } = [];
}
