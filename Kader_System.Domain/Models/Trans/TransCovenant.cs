namespace Kader_System.Domain.Models.Trans;

[Table("Trans_Covenants")]
public class TransCovenant : BaseEntity
{
    [Key]
    public int Id { get; set; }
    public required string Name_en { get; set; }
    public required string Name_ar { get; set; }
    public DateOnly Date { get; set; }
    public string? Notes { get; set; }
    public double Covenant_amount { get; set; }

    public int Employee_id { get; set; }
    [ForeignKey(nameof(Employee_id))]
    public HrEmployee Employee { get; set; } = default!;
}
