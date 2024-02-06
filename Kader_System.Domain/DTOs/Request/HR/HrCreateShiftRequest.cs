namespace Kader_System.Domain.Dtos.Request.HR;

public class HrCreateShiftRequest
{
    
    [Display(Name = Annotations.NameInEnglish), Required(ErrorMessage = Annotations.FieldIsRequired)]
    public required string Name_en { get; set; }

    [Display(Name = Annotations.NameInArabic), Required(ErrorMessage = Annotations.FieldIsRequired)]
    public required string Name_ar { get; set; }

    public TimeOnly Start_shift { get; set; }
    public TimeOnly End_shift { get; set; }
}
