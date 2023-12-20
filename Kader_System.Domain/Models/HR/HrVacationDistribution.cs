namespace Kader_System.Domain.Models.HR;

[Table("Hr_VacationDistributions")]
public class HrVacationDistribution : BaseEntity
{
    [Key]
    public int Id { get; set; }
    public required string Name_en { get; set; }
    public required string Name_ar { get; set; }
    public int Days_count { get; set; }

    public int AccountingWay_id { get; set; }
    [ForeignKey(nameof(AccountingWay_id))]
    public HrAccountingWay AccountingWay { get; set; } = default!;

    public int Vacation_id { get; set; }
    [ForeignKey(nameof(Vacation_id))]
    public HrVacation Vacation { get; set; } = default!;
}
