﻿namespace Kader_System.Domain.DTOs.Request.HR
{
    public class HrUpdateJobRequest
    {
        
        [Display(Name = Annotations.NameInEnglish), Required(ErrorMessage = Annotations.FieldIsRequired)]
        public required string NameEn { get; set; }

        [Display(Name = Annotations.NameInArabic), Required(ErrorMessage = Annotations.FieldIsRequired)]
        public required string NameAr { get; set; }
    }
}
