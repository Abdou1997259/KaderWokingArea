namespace Kader_System.Domain.Models.HR;

[Table("Hr_Companies")]
public class HrCompany : BaseEntity
{
    [Key]
    public int Id { get; set; }
    public required string Name_en { get; set; }
    public required string Name_ar { get; set; }
    public required string Company_owner { get; set; }
    public string? Company_licenses { get; set; }
    public string? Company_licenses_extension { get; set; }
    public int Company_type { get; set; }
    [ForeignKey(nameof(Company_type))]
    public HrCompanyType CompanyType { get; set; } = default!;

    public ICollection<HrCompanyContract> ListOfsContract { get; set; } = [];
}
