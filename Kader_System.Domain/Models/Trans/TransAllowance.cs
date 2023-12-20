namespace Kader_System.Domain.Models.Trans;

[Table("Trans_Allowances")]
public class TransAllowance : BaseEntity
{
    [Key]
    public int Id { get; set; }
    public DateOnly Action_month { get; set; }
    public string? Notes { get; set; }
    public double Amount { get; set; }

    public int Salary_effect_id { get; set; }
    [ForeignKey(nameof(Salary_effect_id))]
    public TransSalaryEffect SalaryEffect { get; set; } = default!;

    public int Employee_id { get; set; }
    [ForeignKey(nameof(Employee_id))]
    public HrEmployee Employee { get; set; } = default!;

    public int Allowance_id { get; set; }
    [ForeignKey(nameof(Allowance_id))]
    public HrAllowance Allowance { get; set; } = default!;
}
