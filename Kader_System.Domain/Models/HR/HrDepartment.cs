namespace Kader_System.Domain.Models.HR;

[Table("Hr_Departments")]
public class HrDepartment : SelectList
{
    public ICollection<HrSectionDepartment> ListOfDepartments { get; set; } = [];
}
