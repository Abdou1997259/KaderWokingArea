namespace Kader_System.Domain.Models.HR;

[Table("Hr_Contracts")]
public class HrContract : BaseEntity
{
    [Key]
    public int Id { get; set; }
    public double Salary_total { get; set; }
    public double Salary_fixed { get; set; }
    public DateOnly Start_date { get; set; }
    public DateOnly End_date { get; set; }
    public required string FileName { get; set; }
    public required string FileExtension { get; set; }

    public int Employee_id { get; set; }
    [ForeignKey(nameof(Employee_id))]
    public HrEmployee Employee { get; set; } = default!;

    public ICollection<HrContractAllowancesDetail> ListOfAllowancesDetails { get; set; } = [];
}
