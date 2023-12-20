namespace Kader_System.Domain.Models.Trans;

[Table("Trans_Deductions")]
public class TransDeduction : BaseEntity
{
    [Key]
    public int Id { get; set; }
    public DateOnly Action_month { get; set; }
    public string? Notes { get; set; }
    public string? Attachment { get; set; }
    public string? AttachmentExtension { get; set; }

    public int Value_type { get; set; }
    [ForeignKey(nameof(Value_type))]
    public TransAmountType AmountType { get; set; } = default!;

    public int Salary_effect_id { get; set; }
    [ForeignKey(nameof(Salary_effect_id))]
    public TransSalaryEffect SalaryEffect { get; set; } = default!;

    public int Employee_id { get; set; }
    [ForeignKey(nameof(Employee_id))]
    public HrEmployee Employee { get; set; } = default!;

    public int Deduction_id { get; set; }
    [ForeignKey(nameof(Deduction_id))]
    public HrDeduction Deduction { get; set; } = default!;
}
