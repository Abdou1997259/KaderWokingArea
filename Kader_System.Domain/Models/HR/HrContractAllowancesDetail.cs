namespace Kader_System.Domain.Models.HR;

[Table("Hr_ContractAllowancesDetails")]
public class HrContractAllowancesDetail : BaseEntity
{
    [Key]
    public int Id { get; set; }
    public double Value { get; set; }

    public int Value_type { get; set; }
    [ForeignKey(nameof(Value_type))]
    public HrValueType ValueType { get; set; } = default!;

    public int Salary_effect_id { get; set; }
    [ForeignKey(nameof(Salary_effect_id))]
    public HrAllowance Allowance { get; set; } = default!;

    public int Contract_id { get; set; }
    [ForeignKey(nameof(Contract_id))]
    public HrContract Contract { get; set; } = default!;
}
