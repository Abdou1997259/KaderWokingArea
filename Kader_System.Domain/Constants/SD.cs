﻿namespace Kader_System.Domain.Constants;

public static class SD
{
    public static class GoRootPath
    {
        public const string SettingImagesPath = "/wwwroot/Images/Setting/";
        public const string SettingFilesPath = "/wwwroot/Files/Setting/";
        public const string SettingAudiosPath = "/wwwroot/Audios/Setting/";
        public const string SettingVideosPath = "/wwwroot/Videos/Setting/";

        public const string HRImagesPath = "/wwwroot/Images/HR/";
        public const string HRFilesPath = "/wwwroot/Files/HR/";
        public const string HRAudiosPath = "/wwwroot/Audios/HR/";
        public const string HRVideosPath = "/wwwroot/Videos/HR/";
    }
    public static class ReadRootPath
    {
        public const string SettingImagesPath = "Images/Setting/";
        public const string SettingFilesPath = "Files/Setting/";
        public const string SettingAudiosPath = "Audios/Setting/";
        public const string SettingVideosPath = "Videos/Setting/";

        public const string HRImagesPath = "Images/HR/";
        public const string HRFilesPath = "Files/HR/";
        public const string HRAudiosPath = "Audios/HR/";
        public const string HRVideosPath = "Videos/HR/";
    }

    public static class FileSettings
    {
        public const string SpecialChar = @"|!#$%&[]=?»«@£§€{};<>";
        public const int Length = 50;
        public const string AllowedExtension = ".jpg,.png,.jpeg";
    }
    public static class Modules
    {
        public const string Auth = "Auth";
        public const string Setting = "Setting";
        public const string HR = "HR";
        public const string V1 = "v1";
        public const string Bearer = "Bearer";

    }
    public static class SuperAdmin
    {
        public const string Id = "b74ddd14-6340-4840-95c2-db12554843e5basb1";
        public const string RoleId = "fab4fac1-c546-41de-aebc-a14da68957ab1";
        public static string Password = "Mohammed88";
        public static string RoleNameInAr = "سوبر أدمن";
    }
    public static class Roles
    {
        public const string Administrative = "Administrative";
        public const string User = "User";
        public const string SuperAdmin = "SuperAdmin";
    }
    public static class RequestClaims
    {
        public const string Permission = "Permission";
        public const string RolePermission = "RolePermission";
        public const string UserPermission = "UserPermission";
        public const string DomainRestricted = "DomainRestricted";
        public const string UserId = "uid";
    }
    public static class Shared
    {
        public const string KaderSystem = "KaderSystem";
        public const string KaderSystemConnection = "KaderSystemConnection";
        public const string JwtSettings = "JwtSettings";
        public const string AccessToken = "access_token";
        public const string CorsPolicy = "CorsPolicy";
        public const string Development = "Development";
        public const string Production = "Production";
        public const string Local = "Local";
        public const string Notify = "/notify";
        public static string[] Cultures = ["en-US", "ar-EG"];
        public const string Resources = "Resources";
        public const string Company = "Company";
        public const string Department = "Department";
        public const string Task = "Task";
    }
    public static class ApiRoutes
    {
        public class MainScreenCategory
        {
            public const string ListOfMainScreensCategories = "screen_main/getListOfMainScreens";
            public const string GetAllMainScreenCategories = "screen_main";
            public const string CreateMainScreenCategory = "screen_main";
            public const string UpdateMainScreenCategory = "screen_main/{id}";
            public const string GetMainScreenCategoryById = "screen_main/{id}";
            public const string DeleteMainScreenCategory = "screen_main/{id}";
        }

        public class MainScreen
        {
            public const string ListOfMainScreens = "screen_cat/getListOfMainScreens";
            public const string GetAllMainScreens = "screen_cat";
            public const string CreateMainScreen = "screen_cat";
            public const string UpdateMainScreen = "screen_cat/{id}";
            public const string GetMainScreenById = "screen_cat/{id}";
            public const string DeleteMainScreen = "screen_cat/{id}";
        }

        public class SubMainScreen
        {
            public const string ListOfSubMainScreens = "screen_sub/getListOfSubMainScreens";
            public const string GetAllSubMainScreens = "screen_sub";
            public const string CreateSubMainScreen = "screen_sub";
            public const string UpdateSubMainScreen = "screen_sub/{id}";
            public const string GetSubMainScreenById = "screen_sub/{id}";
            public const string DeleteSubMainScreen = "screen_sub/{id}";
        }
        public class User
        {
            public const string GetAllUsers = "GetAllUsers";
            public const string AddUser = "AddUser";
            public const string LoginUser = "LoginUser";
            public const string LogOutUser = "LogOutUser";
            public const string LoginUser1 = "LoginUser1";
            public const string ChangeActiveOrNot = "ChangeActiveOrNotUser/{id}";
            public const string UpdateUser = "UpdateUser/{id}";
            public const string ShowPasswordToSpecificUser = "ShowPasswordToSpecificUser/{id}";
            public const string ChangePassword = "ChangePassword";
            public const string DeleteUser = "DeleteUser/{id}";
            public const string SetNewPasswordToSpecificUser = "SetNewPasswordToSpecificUser";
            public const string SetNewPasswordToSuperAdmin = "SetNewPasswordToSuperAdmin/{newPassword}";
        }

        public class Perm
        {
            public const string GetAllRoles = "GetAllRoles";
            public const string CreateRole = "CreateRole";
            public const string UpdateRole = "UpdateRole/{id}";
            public const string DeleteRoleById = "DeleteRoleById/{id}";

            public const string GetEachUserWithHisRoles = "GetEachUserWithHisRoles";
            public const string ManageUserRoles = "ManageUserRoles/{userId}";
            public const string UpdateUserRoles = "UpdateUserRoles";

            public const string GetAllPermissionsByCategoryName = "GetAllPermissionsByCategoryName";
            public const string ManageRolePermissions = "ManageRolePermissions/{roleId}";
            public const string UpdateRolePermissions = "UpdateRolePermissions";
            public const string ManageUserPermissions = "ManageUserPermissions/{userId}";
            public const string UpdateUserPermissions = "UpdateUserPermissions";
        }

        public class Company
        {
            public const string ListOfCompanies = "company/getListOfCompanies";
            public const string GetAllCompanies = "company";
            public const string CreateCompany = "company/create";
            public const string UpdateCompany = "company/update/{id}";
            public const string GetCompanyById = "company/getById/{id}";
            public const string DeleteCompany = "company/delete/{id}";
        }

        public class Allowance
        {
            public const string ListOfAllowances = "allowance/getListOfAllowances";
            public const string GetAllAllowances = "allowance/getAll";
            public const string CreateAllowance = "allowance/create";
            public const string UpdateAllowance = "allowance/update/{id}";
            public const string GetAllowanceById = "allowance/getById/{id}";
            public const string DeleteAllowance = "allowance/delete/{id}";
        }
        public class Benefit
        {
            public const string ListOfBenefits = "benefit/getListOfBenefits";
            public const string GetAllBenefits = "benefit/getAll";
            public const string CreateBenefit = "benefit/create";
            public const string UpdateBenefit = "benefit/update/{id}";
            public const string GetBenefitById = "benefit/getById/{id}";
            public const string DeleteBenefit = "benefit/delete/{id}";
        }

        public class Deduction
        {
            public const string ListOfDeductions = "deduction/getListOfDeductions";
            public const string GetAllDeductions = "deduction/getAll";
            public const string CreateDeduction = "deduction/create";
            public const string UpdateDeduction = "deduction/update/{id}";
            public const string GetDeductionById = "deduction/getById/{id}";
            public const string DeleteDeduction = "deduction/delete/{id}";
        }

        public class Qualification
        {
            public const string ListOfQualifications = "qualification/getListOfQualifications";
            public const string GetAllQualifications = "qualification/getAll";
            public const string CreateQualification = "qualification/create";
            public const string UpdateQualification = "qualification/update/{id}";
            public const string GetQualificationById = "qualification/getById/{id}";
            public const string DeleteQualification = "qualification/delete/{id}";
        }
        public class Job
        {
            public const string ListOfJobs = "Job/getListOfJobs";
            public const string GetAllJobs = "Job/getAll";
            public const string CreateJob = "Job/create";
            public const string UpdateJob = "Job/update/{id}";
            public const string GetJobById = "Job/getById/{id}";
            public const string DeleteJob = "Job/delete/{id}";
        }
        public class Vacation
        {
            public const string ListOfVacations = "Vacation/getListOfVacations";
            public const string GetAllVacations = "Vacation/getAll";
            public const string CreateVacation = "Vacation/create";
            public const string UpdateVacation = "Vacation/update/{id}";
            public const string GetVacationById = "Vacation/getById/{id}";
            public const string DeleteVacation = "Vacation/delete/{id}";
        }
        public class Shift
        {
            public const string ListOfShifts = "shift/getListOfShifts";
            public const string GetAllShifts = "shift/getAll";
            public const string CreateShift = "shift/create";
            public const string UpdateShift = "shift/update/{id}";
            public const string GetShiftById = "shift/getById/{id}";
            public const string DeleteShift = "shift/delete/{id}";
        }
    }

    public static class Localization
    {
        public const string Arabic = "ar-EG";
        public const string English = "en-US";
        public const string IsExist = "IsExist";
        public const string Project = "Project";
        public const string Task = "Task";
        public const string Notification = "Notification";
        public const string CanNotAddingToNotThereIsProjectAndDepartment = "CanNotAddingToNotThereIsProjectAndDepartment";

        public const string CannotUpdateTaskStatus = "CannotUpdateTaskStatus";
        public const string TaskExist = "TaskExist";
        public const string Department = "Department";
        public const string TaskComment = "TaskComment";
        public const string EmployeeVacation = "EmployeeVacation";
        public const string DepartmentManager = "DepartmentManager";
        public const string Done = "Done";
        public const string Error = "Error";
        public const string ThisAmountCannotBePaidFromTheMainTreasuryDueToItsAvailability = "ThisAmountCannotBePaidFromTheMainTreasuryDueToItsAvailability";
        public const string ThisAmountCannotBePaidFromTheTreasuryBranchDueToItsAvailability = "ThisAmountCannotBePaidFromTheTreasuryBranchDueToItsAvailability";
        public const string ThisAmountCannotBeTransferedFromTheTreasuryDueToItsAvailability = "ThisAmountCannotBeTransferedFromTheTreasuryDueToItsAvailability";
        public const string ThisAmountCannotBeTransferedFromTheBranchTreasuryDueToItsAvailability = "ThisAmountCannotBeTransferedFromTheBranchTreasuryDueToItsAvailability";
        public const string ThisAmountCannotBeReceitedAsThisClientHasNotPrice = "ThisAmountCannotBeReceitedAsThisClientHasNotPrice";
        public const string ItIsNecessaryThatAmountMoreThanZero = "ItIsNecessaryThatAmountMoreThanZero";
        public const string Used = "Used";
        public const string CannotBeFound = "CannotBeFound";
        public const string Departments = "Departments";

        public const string MainScreenCategory = "MainScreenCategory";
        public const string MainScreen = "MainScreen";
        public const string SubMainScreen = "SubMainScreen";











        public const string DepartmentsExist = "DepartmentsExist";
        public const string Jobs = "Jobs";
        public const string JobExist = "JobExist";
        public const string Projects = "Projects";
        public const string ProjectsExisit = "ProjectsExisit";
        public const string Tasks = "Tasks";
        public const string CannotDeletedThisRole = "CannotDeletedThisRole";
        public const string Service = "Service";
        public const string ServicesCategory = "ServicesCategory";
        public const string Policy = "Policy";
        public const string News = "News";
        public const string Updated = "Updated";
        public const string Deleted = "Deleted";
        public const string CurrentAndNewPasswordIsTheSame = "CurrentAndNewPasswordIsTheSame";
        public const string CurrentPasswordIsIncorrect = "CurrentPasswordIsIncorrect";
        public const string UserName = "UserName";
        public const string UserNameOrEmail = "UserNameOrEmail";
        public const string User = "User";
        public const string ThisEmployeeWasDeleted = "ThisEmployeeWasDeleted";
        public const string NotFound = "NotFound";
        public const string Email = "Email";
        public const string PasswordNotmatch = "PasswordNotmatch";
        public const string Role = "Role";

        public const string Employee = "Employee";
        public const string EmployeeExist = "EmployeeExist";
        public const string ThereAreNotAttachments = "ThereAreNotAttachments";
        public const string CanNotAssignAnyEmpToFindingOther = "CanNotAssignAnyEmpToFindingOther";
        public const string CanNotAssignAnyEmpToFindingComManager = "CanNotAssignAnyEmpToFindingComManager";
        public const string CanNotRemoveThisEmployeeAsHasAmountInHisTreasury = "CanNotRemoveThisEmployeeAsHasAmountInHisTreasury";
        public const string Request = "Request";

        public const string Job = "Job";
        public const string Category = "Category";
        //public const string Item = "Item";

        public const string Section = "Section";
        public const string ClientCategory = "ClientCategory";
        public const string ThisEmployeeIsNotTechnician = "ThisEmployeeIsNotTechnician";
        public const string CompanyBranch = "CompanyBranch";
        public const string LockTechnicalsLogins = "LockTechnicalsLogins";
        public const string UnLockTechnicalsLogins = "UnLockTechnicalsLogins";
        public const string ThereIsActiveEmployeesRelatedToThisBranch = "ThereIsActiveEmployeesRelatedToThisBranch";
        public const string Activated = "Activated";
        public const string UserWasLoggedOutBefore = "UserWasLoggedOutBefore";
        public const string PleaseChangeEmployeeActivationState = "PleaseChangeEmployeeActivationState";
        public const string DeActivated = "DeActivated";
        public const string TheEmployeeNotActive = "TheEmployeeNotActive";
        public const string Region = "Region";
        public const string State = "State";
        public const string TaxOffice = "TaxOffice";
        public const string UserNotExist = "UserNotExist";
        public const string UserIsLoggedOut = "UserIsLoggedOut";
        public const string Location = "Location";
        public const string UserWithEmailExists = "UserWithEmailExists";
        public const string UserWithNameExists = "UserWithNameExists";
        public const string CompanyIsNotActivated = "CompanyIsNotActivated";
        public const string Email_Password_Incorrect = "Email_Password_Incorrect";
        public const string UserDataIsIncorrect = "UserDataIsIncorrect";
        public const string Company = "Company";
        public const string Allowance = "Allowance";
        public const string Benefit = "Benefit";
        public const string Qualification = "Qualification";
        public const string Deduction = "Deduction";
        public const string Vacation = "Vacation";
        public const string Image = "Image";
        public const string CompanyIsNotActive = "Company is not active";
        public const string NotActive = "NotActive";
        public const string NotActiveNotUpdate = "NotActiveNotUpdate";
        public const string NotFoundMainBranchToCompany = "NotFoundMainBranchToCompany";
        public const string NotFoundData = "NotFoundData";
        public const string CanNotAddCommentToSpecificComment = "CanNotAddCommentToSpecificComment";
        public const string Technician = "Technician";
        public const string UserIsAlreadyLoggedIn = "UserIsAlreadyLoggedIn";
        public const string HasAnyRelation = "HasAnyRelation";
        public const string Item = "Item";
        public const string Data = "Data";
        public const string Admin = "Admin";
        public const string Client = "Client";
        public const string ClientAppointmentMaking = "ClientAppointmentMaking";
        public const string CompletionStatus = "CompletionStatus";
        public const string ClientNotes = "ClientNotes";
        public const string ClientProcedure = "ClientProcedure";

        public const string ReceiptVoucher = "ReceiptVoucher";
        public const string PaymentVoucher = "PaymentVoucher";
        public const string Treasury = "Treasury";
        public const string BranchTreasury = "BranchTreasury";
        public const string PaymentGatway = "PaymentGatway ";
        public const string ClientBranch = "ClientBranch";
        public const string FinancialYear = "FinancialYear";
        public const string CanNotAddFinancialYear = "CanNotAddFinancialYear";
        public const string CanNotAddFinancialYearAsThereIsActiveOne = "CanNotAddFinancialYearAsThereIsActiveOne";
        public const string CanNotAddFinancialYearAsThereIsDateNotConventioned = "CanNotAddFinancialYearAsThereIsDateNotConventioned";
        public const string CanNotActivateFinancialYearAsThereIsActiveOne = "CanNotActivateFinancialYearAsThereIsActiveOne";
        public const string ThisEmployeeAlreadyIsAssignedBefore = "ThisEmployeeAlreadyIsAssignedBefore";
        public const string CanNotRemoveThisBranchTreasuryAsThereIsAmount = "CanNotRemoveThisBranchTreasuryAsThereIsAmount";
        public const string CompanyIsRequired = "CompanyIsRequired";
        public const string FinancialYearIsRequired = "FinancialYearIsRequired";
        public const string BothOfCompanyAndFinancialYearRequired = "BothOfCompanyAndFinancialYearRequired";
        public const string BillBook = "BillBook";
        public const string CanNotAddBillBook = "CanNotAddBillBook";
        public const string FinancialYearIsNotActive = "FinancialYearIsNotActive";
        public const string CannotDeleteItemHasRelativeData = "CannotDeleteItemHasRelativeData";
        public const string ThereIs = "ThereIs";
        public const string NumOfObjectsNotEqualNumOfUploadedImages = "NumOfObjectsNotEqualNumOfUploadedImages";
        public const string IsNotSuperAdmin = "IsNotSuperAdmin";
        public const string PaymentGateway = "PaymentGateway";
        public const string PathRoute = "PathRoute";
        public const string ThisEmployeeIsNotTech = "ThisEmployeeIsNotTech";
        public const string Unit = "Unit";
        public const string UnitOfConversion = "UnitOfConversion";
        public const string RefusedPermission = "RefusedPermission";
        public const string ThisStockHasAlreadyTechnique = "ThisStockHasAlreadyTechnique";
        public const string ThisStockWithOutTechnique = "ThisStockWithOutTechnique";
        public const string Stock = "Stock";
        public const string NotFoundPhotos = "NotFoundPhotos";
        public const string ThereIsOnlineTech = "ThereIsOnlineTech";
        public const string NotValidDate = "NotValidDate";
        public const string StockTrans = "StockTrans";
        public const string InvalidDocNum = "InvalidDocNum";
        public const string StockTransType = "StockTransType";
        public const string ItemSerial = "ItemSerial";
        public const string ThereIsNotEnoughQuantityInTheStock = "ThereIsNotEnoughQuantityInTheStock";
        public const string ThereIsSomeItemsNotEnoughQuantityInTheStock = "ThereIsSomeItemsNotEnoughQuantityInTheStock";
        public const string AvailableAmount = "AvailableAmount";
        public const string RequiredAmount = "RequiredAmount";
        public const string CannotSendDatatoNoOne = "CannotSendDatatoNoOne";

        public const string ReqEarlyExit = "ReqEarlyExit";
        public const string ReqLateAttendance = "ReqLateAttendance";
        public const string ReqPermitExit = "ReqPermitExit";
        public const string ReqPermitFromHome = "ReqPermitFromHome";
        public const string ReqPermitFingerprint = "ReqPermitFingerprint";
        public const string ReqResign = "ReqResign";
        public const string ReqSalaryInc = "ReqSalaryInc";
        public const string PermitTrust = "PermitTrust";
        public const string ReqVacation = "ReqVacation";
        public const string ReqTrustCash = "ReqTrustCash";
        public const string ReqTransfer = "ReqTransfer";
        public const string ReqReward = "ReqReward";
        public const string ReqWorkDayCalc = "ReqWorkDayCalc";
        public const string ReqExtraTimeCalc = "ReqExtraTimeCalc";
        public const string ReqExitPermission = "ReqExitPermission";
        public const string ReqAddvance = "ReqAddvance";
        public const string ThisCannotBeDoneDueToThePresenceOf = "ThisCannotBeDoneDueToThePresenceOf";
    }

    public class LoggingMessages
    {
        public const string ErrorWhileDeleting = "Error while deleting for";
        public const string Id = "id: ";
        public const string Obj = "and obj: ";
    }

    public class Annotations
    {
        public const string Name = "Name";
        public const string ConfirmationPassword = "Confirmation password";
        public const string DepartmentName = "Department";
        public const string ConfirmationPasswordNotMatch = "Password and confirmation password must match.";
        public const string AttachmentsNotes = "Attachments notes.";

        public const string AttachmentsType = "Attachments type.";
        public const string FieldIsRequired = "The {0} is required";
        public const string BirthDate = "Birth date.";

        public const string NationalID = "National ID";
        public const string FieldIsEqual = "The {0} field length must be equal 14.";
        public const string ProfilePhoto = "Profile photo.";
        public const string Files = "Personal files.";
        public const string CourseMatrial = "Course matrial.";
        public const string CourseMatrialType = "Course matrial type.";
        public const string Password = "Password";
        public const string Code = "Code";
        public const string Company = "Company";
        public const string Job = "Job";
        public const string Gender = "Gender";
        public const string Region = "Region";
        public const string MaritalStatus = "MaritalStatus";
        public const string MilitaryStatus = "MilitaryStatus";

        public const string NameInArabic = "Name in Arabic";
        public const string NameInEnglish = "Name in English";
        public const string CompanyOwner = "Company owner";
        public const string PersonalEmail = "Personal Email";
        public const string PhoneNumber = "Phone number";
        public const string HireDate = "Hire date";
        public const string Task = "Task";
        public const string UserName = "User name";
        public const string UserNameOrEmail = "User name or email";

        public const string CourseAsset = "Course asset";
        public const string CourseAssetDescription = "Course asset description";//
        public const string CourseAssetType = "Course asset type";
        public const string CourseAssetTypeDescription = "Course asset type description";

        public const string CourseDate = "Course date";
        public const string CourseType = "Course type";
        public const string CourseDescription = "Course description";

        public const string StartDate = "Start date";
        public const string EndDate = "End date";
        public const string RememberMe = "Remember me?";
    }
}
