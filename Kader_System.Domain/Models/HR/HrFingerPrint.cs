namespace Kader_System.Domain.Models.HR;

[Table("Hr_FingerPrints")]
public class HrFingerPrint : BaseEntity
{
    [Key]
    public int Id { get; set; }
    public int Ip { get; set; }
    public required string Name_ar { get; set; }
    public required string Name_en { get; set; }
    public required string Password { get; set; }
    public required string Port { get; set; }
    public required string Username { get; set; }

    public int Company_id { get; set; }
    [ForeignKey(nameof(Company_id))]
    public HrCompany Company { get; set; } = default!;
}
