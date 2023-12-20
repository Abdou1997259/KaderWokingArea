namespace Kader_System.Domain.Models.HR;

[Table("Hr_Vacations")]
public class HrVacation : BaseEntity
{
    [Key]
    public int Id { get; set; }

    public int Apply_months { get; set; }
    public int Total_vacation { get; set; }
    public bool Transfer_vacation { get; set; }
    public required string Name_en { get; set; }
    public required string Name_ar { get; set; }

    public int Vacation_type { get; set; }
    [ForeignKey(nameof(Vacation_type))]
    public HrVacationType VacationType { get; set; } = default!;

    public ICollection<HrVacationDistribution> ListOfVacationDistributions { get; set; } = [];
}
