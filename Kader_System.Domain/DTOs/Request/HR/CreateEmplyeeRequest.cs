namespace Kader_System.Domain.DTOs.Request.HR
{
    public class CreateEmployeeRequest
    {
        [Display(Name = Annotations.NameInArabic), Required(ErrorMessage = Annotations.FieldIsRequired)]
        public  string FirstNameAr { get; set; }
        [Required(ErrorMessage = Annotations.FieldIsRequired)]
        public  string FirstNameEn { get; set; }
        [Required(ErrorMessage = Annotations.FieldIsRequired)]
        public  string FatherNameAr { get; set; }
        [Required(ErrorMessage = Annotations.FieldIsRequired)]
        public  string FatherNameEn { get; set; }
        [Required(ErrorMessage = Annotations.FieldIsRequired)]
        public  string GrandFatherNameAr { get; set; }
        [Required(ErrorMessage = Annotations.FieldIsRequired)]
        public  string GrandFatherNameEn { get; set; }
        [Required(ErrorMessage = Annotations.FieldIsRequired)]
        public  string FamilyNameAr { get; set; }
        [Required(ErrorMessage = Annotations.FieldIsRequired)]
        public  string FamilyNameEn { get; set; }

        [Required(ErrorMessage = Annotations.FieldIsRequired)]
        public int MaritalStatusId { get; set; }
        [Required(ErrorMessage = Annotations.FieldIsRequired)]
        public  string Address { get; set; }

        public string NationalId { get; set; }
        public string JobNumber { get; set; }

        [Required(ErrorMessage = Annotations.FieldIsRequired)]
        public  DateOnly HiringDate { get; set; }
        [Required(ErrorMessage = Annotations.FieldIsRequired)]
        public  DateOnly ImmediatelyDate { get; set; }
        [Required(ErrorMessage = Annotations.FieldIsRequired)]
        public  bool IsActive { get; set; }
        [Required(ErrorMessage = Annotations.FieldIsRequired)]
        public int GenderId { get; set; }

        [Required(ErrorMessage = Annotations.FieldIsRequired)]
        public  DateOnly BirthDate { get; set; }
        [Required(ErrorMessage = Annotations.FieldIsRequired)]
        public int ReligionId { get; set; }

        [Required(ErrorMessage = Annotations.FieldIsRequired)]
        public  string Phone { get; set; }
        [Required(ErrorMessage = Annotations.FieldIsRequired)]
        public  string Email { get; set; }

        [Required(ErrorMessage = Annotations.FieldIsRequired)]
        public int SalaryPaymentWayId { get; set; }

        [Required(ErrorMessage = Annotations.FieldIsRequired)]
        public  string Username { get; set; }
        [Required(ErrorMessage = Annotations.FieldIsRequired)]
        public  string Password { get; set; }
        [DefaultValue(0)]
        [Required(ErrorMessage = Annotations.FieldIsRequired)]
        public  int ChildrenNumber { get; set; }

        [Required(ErrorMessage = Annotations.FieldIsRequired)]
        public int ShiftId { get; set; }
        [Required(ErrorMessage = Annotations.FieldIsRequired)]
        public int CompanyId { get; set; }
    
        /// <summary>
        /// جهاز البصمه
        /// </summary>
        public int? FingerPrintId { get; set; }

        /// <summary>
        /// كود الموظف المسجل على جهاز البصمه
        /// </summary>
        public string? FingerPrintCode { get; set; }

        public string? EmployeeImage { get; set; }
        public string? EmployeeImageExtension { get; set; }
        [Required(ErrorMessage = Annotations.FieldIsRequired)]
        public int ManagementId { get; set; }

        [Required(ErrorMessage = Annotations.FieldIsRequired)]
        public int DepartmentId { get; set; }

        [Required(ErrorMessage = Annotations.FieldIsRequired)]
        public int NationalityId { get; set; }
        [Required(ErrorMessage = Annotations.FieldIsRequired)]
        public int QualificationId { get; set; }
        [Required(ErrorMessage = Annotations.FieldIsRequired)]
        public int VacationId { get; set; }

        [Required(ErrorMessage = Annotations.FieldIsRequired)]
        public int JobId { get; set; }

        /// <summary>
        /// مقيم / مواطن
        /// </summary>
        [Required(ErrorMessage = Annotations.FieldIsRequired)]
        public int EmployeeTypeId { get; set; }
     

        public long? AccountNo { get; set; }

        [AllowedLetters(FileSettings.SpecialChar), MaxFileLettersCount(FileSettings.Length), FileExtensionValidation(FileSettings.AllowedExtension)]
        public IFormFileCollection? EmployeeAttachments { get; set; } = default!;

    }
}
