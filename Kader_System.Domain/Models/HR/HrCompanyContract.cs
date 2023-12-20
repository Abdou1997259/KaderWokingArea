namespace Kader_System.Domain.Models.HR;

[Table("Hr_CompanyContracts")]
public class HrCompanyContract : BaseEntity
{
    [Key]
    public int Id { get; set; }
    public string? Company_contracts { get; set; }
    public string? Company_contracts_extension { get; set; }

    public int Company_id { get; set; }
    [ForeignKey(nameof(Company_id))]
    public HrCompany CompanyType { get; set; } = default!;
}
