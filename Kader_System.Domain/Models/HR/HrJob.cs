namespace Kader_System.Domain.Models.HR;

[Table("Hr_Jobs")]
public class HrJob : BaseEntity
{
    [Key]
    public int Id { get; set; }
    public required string Name_en { get; set; }
    public required string Name_ar { get; set; }
    public bool Has_need_license { get; set; }
    public bool Has_additional_time { get; set; }
}
