namespace Kader_System.Domain.DTOs.Response.HR
{
    public class GetEmployeeByIdResponse
    {
        public int Id { get; set; }
        public  string FirstNameAr { get; set; }
        public  string FirstNameEn { get; set; }
        public  string FatherNameAr { get; set; }
        public  string FatherNameEn { get; set; }
        public  string GrandFatherNameAr { get; set; }
        public  string GrandFatherNameEn { get; set; }
        public  string FamilyNameAr { get; set; }
        public  string FamilyNameEn { get; set; }
 
        public string FullNameAr => $"{FirstNameAr} {FatherNameAr} {GrandFatherNameAr} {FamilyNameAr}";
    
        public string FullNameEn => $"{FirstNameEn} {FatherNameEn} {GrandFatherNameEn} {FamilyNameEn}";

        public int MaritalStatusId { get; set; }
       

        public  string Address { get; set; }
        public  double FixedSalary { get; set; }
        public  DateOnly HiringDate { get; set; }
        public  DateOnly ImmediatelyDate { get; set; }
        public  bool IsActive { get; set; }

        public  double TotalSalary { get; set; }
        public int GenderId { get; set; }
      

        public  DateOnly BirthDate { get; set; }
        public int ReligionId { get; set; }
      
        public  string Phone { get; set; }
        public  string Email { get; set; }
        public string NationalId { get; set; }
        public string JobNumber { get; set; }

        public int SalaryPaymentWayId { get; set; }
    
        public  string Username { get; set; }
        public  string Password { get; set; }
      
        public  int ChildrenNumber { get; set; }



        public int ShiftId { get; set; }
      
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

        public int ManagementId { get; set; }
    

        public int DepartmentId { get; set; }
    

        public int NationalityId { get; set; }
       

        public int QualificationId { get; set; }
       

        public int VacationId { get; set; }
        public int JobId { get; set; }
  
        /// <summary>
        /// مقيم / مواطن
        /// </summary>
        public int EmployeeTypeId { get; set; }
        public long? AccountNo { get; set; }
     
    }
}
