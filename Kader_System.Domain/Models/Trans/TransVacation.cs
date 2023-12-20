namespace Kader_System.Domain.Models.Trans;

[Table("Trans_Vacations")]
public class TransVacation : BaseEntity
{
    [Key]
    public int Id { get; set; }
    public DateOnly Start_date { get; set; }
    public double Days_count { get; set; }
    public string? Notes { get; set; }

    public int Employee_id { get; set; }
    [ForeignKey(nameof(Employee_id))]
    public HrEmployee Employee { get; set; } = default!;

    public int Vacation_system_d_id { get; set; }
    [ForeignKey(nameof(Vacation_system_d_id))]
    public HrVacation Vacation { get; set; } = default!;
}
