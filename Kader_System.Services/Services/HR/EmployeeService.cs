using Kader_System.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace Kader_System.Services.Services.HR
{
    public class EmployeeService(IUnitOfWork unitOfWork, IStringLocalizer<SharedResource> shareLocalizer, IMapper mapper
    , UserManager<ApplicationUser> userManager) : IEmployeeService
    {
        private HrEmployee _instanceEmployee;

        #region Retreive

        public async Task<Response<IEnumerable<ListOfEmployeesResponse>>> ListOfEmployeesAsync(string lang)
        {
            var result =
                await unitOfWork.Employees.GetSpecificSelectAsync(null!
                    , includeProperties: $"{nameof(_instanceEmployee.Company)},{nameof(_instanceEmployee.Management)}," +
                                         $"{nameof(_instanceEmployee.Department)},{nameof(_instanceEmployee.MaritalStatus)}," +
                                         $"{nameof(_instanceEmployee.Nationality)}",
                    select: x => new ListOfEmployeesResponse
                    {
                        Id = x.Id,
                        FullName = lang == Localization.Arabic ? x.FullNameAr : x.FullNameEn,
                        Company = lang == Localization.Arabic ? x.Company!.NameAr : x.Company!.NameEn,
                        Management = lang == Localization.Arabic ? x.Management!.NameAr : x.Management!.NameEn,
                        HiringDate = x.HiringDate,
                        IsActive = x.IsActive,
                        Address = x.Address,
                        ImmediatelyDate = x.ImmediatelyDate,
                        MaritalStatus = lang == Localization.Arabic ? x.MaritalStatus!.Name : x.MaritalStatus!.NameInEnglish,
                        Nationality = lang == Localization.Arabic ? x.Nationality!.Name : x.Nationality!.NameInEnglish,

                    }, orderBy: x =>
                        x.OrderByDescending(x => x.Id));

            var listOfEmployeesResponses = result.ToList();
            if (!listOfEmployeesResponses.Any())
            {
                string resultMsg = shareLocalizer[Localization.NotFoundData];

                return new()
                {
                    Data = [],
                    Error = resultMsg,
                    Msg = resultMsg
                };
            }

            return new()
            {
                Data = listOfEmployeesResponses,
                Check = true
            };
        }


        public async Task<Response<GetEmployeeByIdResponse>> GetEmployeeByIdAsync(int id)
        {
            Expression<Func<HrEmployee, bool>> filter = x => x.Id == id;
            var obj = await unitOfWork.Employees.GetFirstOrDefaultAsync(filter, 
                includeProperties: $"{nameof(_instanceEmployee.Management)},{nameof(_instanceEmployee.Job)}," +
                                   $"{nameof(_instanceEmployee.User)}");

            if (obj is null)
            {
                string resultMsg = shareLocalizer[Localization.NotFoundData];

                return new()
                {
                    Data = new(),
                    Error = resultMsg,
                    Msg = resultMsg
                };
            }

            return new()
            {
                Data = new()
                {
                    ManagementId = obj.ManagementId,
                    CompanyId = obj.CompanyId,
                    FirstNameEn = obj.FirstNameEn,
                    FatherNameAr = obj.FatherNameAr,
                    FatherNameEn = obj.FatherNameEn,
                    GrandFatherNameAr = obj.GrandFatherNameAr,
                    GrandFatherNameEn = obj.GrandFatherNameEn,
                    FamilyNameAr = obj.FamilyNameAr,
                    FamilyNameEn = obj.FamilyNameEn,
                    FirstNameAr = obj.FirstNameAr,
                    IsActive = obj.IsActive,
                    VacationId = obj.VacationId,
                    AccountNo = obj.AccountNo,
                    Address = obj.Address,
                    BirthDate = obj.BirthDate,
                    ChildrenNumber = obj.ChildrenNumber,
                    DepartmentId = obj.DepartmentId,
                    Email = obj.Email,
                    EmployeeImageExtension = obj.EmployeeImageExtension,
                    EmployeeTypeId = obj.EmployeeTypeId,
                    FingerPrintCode = obj.FingerPrintCode,
                    FingerPrintId = obj.FingerPrintId,
                    FixedSalary = obj.FixedSalary,
                    GenderId = obj.GenderId,
                    HiringDate = obj.HiringDate,
                    ImmediatelyDate = obj.ImmediatelyDate,
                    Id = obj.Id,
                    JobId = obj.JobId,
                    JobNumber = obj.JobNumber,
                    MaritalStatusId = obj.MaritalStatusId,
                    NationalId = obj.NationalId,
                    NationalityId = obj.NationalityId,
                    Password = obj.User!.VisiblePassword,
                    Phone = obj.Phone,
                    QualificationId = obj.QualificationId,
                    ReligionId = obj.ReligionId,
                    SalaryPaymentWayId = obj.SalaryPaymentWayId,
                    ShiftId = obj.ShiftId,
                    TotalSalary = obj.TotalSalary,
                    Username = obj.User!.UserName,
                    EmployeeImage = $"{GoRootPath.EmployeeImagesPath}{obj.EmployeeImage}"
                },
                Check = true
            };
        }
        public async Task<Response<GetAllEmployeesResponse>> GetAllEmployeesAsync(string lang,
            GetAllEmployeesFilterRequest model,string host)
        {
            Expression<Func<HrEmployee, bool>> filter = x => x.IsDeleted == model.IsDeleted;

            var totalRecords = await unitOfWork.Employees.CountAsync(filter: filter);
            int page = 1;
            int totalPages = (int)Math.Ceiling((double)totalRecords / (model.PageSize == 0 ? 10 : model.PageSize));
            if (model.PageNumber < 1)
                page = 1;
            var pageLinks = Enumerable.Range(1, totalPages)
                .Select(p => new Link() { label = p.ToString(), url = host + $"?PageSize={model.PageSize}&PageNumber={p}&IsDeleted={model.IsDeleted}", active = p == model.PageNumber })
                .ToList();
            var result = new GetAllEmployeesResponse
            {
                TotalRecords = totalRecords,

                Items = (await unitOfWork.Employees.GetSpecificSelectAsync(filter: filter,
                    includeProperties: $"{nameof(_instanceEmployee.Company)},{nameof(_instanceEmployee.Management)}," +
                    $"{nameof(_instanceEmployee.Department)},{nameof(_instanceEmployee.MaritalStatus)}," +
                    $"{nameof(_instanceEmployee.Nationality)},{nameof(_instanceEmployee.FingerPrint)}," +
                    $"{nameof(_instanceEmployee.EmployeeType)},{nameof(_instanceEmployee.Gender)}," +
                    $"{nameof(_instanceEmployee.Job)},{nameof(_instanceEmployee.Qualification)},{nameof(_instanceEmployee.Vacation)}," +
                    $"{nameof(_instanceEmployee.Shift)},{nameof(_instanceEmployee.Religion)}," +
                    $"{nameof(_instanceEmployee.SalaryPaymentWay)},{nameof(_instanceEmployee.User)}",
                    take: model.PageSize,
                    skip: (model.PageNumber - 1) * model.PageSize,
                    select: x => new EmployeesData
                    {
                        Id = x.Id,
                        FullName = lang == Localization.Arabic ? x.FullNameAr : x.FullNameEn,
                        Company = lang == Localization.Arabic ? x.Company!.NameAr : x.Company!.NameEn,
                        Management = lang == Localization.Arabic ? x.Management!.NameAr : x.Management!.NameEn,
                        HiringDate = x.HiringDate,
                        IsActive = x.IsActive,
                        Address = x.Address,
                        ImmediatelyDate = x.ImmediatelyDate,
                        MaritalStatus = lang == Localization.Arabic ? x.MaritalStatus!.Name : x.MaritalStatus!.NameInEnglish,
                        Nationality = lang == Localization.Arabic ? x.Nationality!.Name : x.Nationality!.NameInEnglish,
                        Department = lang == Localization.Arabic ? x.Department!.NameAr : x.Department!.NameEn,
                        FingerPrint = lang == Localization.Arabic ? x.FingerPrint!.NameAr : x.FingerPrint!.NameEn,
                        BirthDate = x.BirthDate,
                        ChildrenNumber = x.ChildrenNumber,
                        Email = x.Email,
                        EmployeeType = lang == Localization.Arabic ? x.EmployeeType!.Name : x.EmployeeType!.NameInEnglish,
                        FingerPrintCode = x.FingerPrintCode,
                        Gender = lang == Localization.Arabic ? x.Gender!.Name : x.Gender!.NameInEnglish,
                        Job = lang == Localization.Arabic ? x.Job!.NameAr : x.Job!.NameEn,
                        JobNumber = x.JobNumber,
                        NationalId = x.NationalId,
                        Password = x.User!.VisiblePassword,
                        Phone = x.Phone,
                        Qualification = lang == Localization.Arabic ? x.Qualification!.NameAr : x.Qualification!.NameEn,
                        Username = x.User!.UserName,
                        Shift = lang == Localization.Arabic ? x.Shift!.Name_ar : x.Shift!.Name_en,
                        Vacation = lang == Localization.Arabic ? x.Vacation!.NameAr : x.Vacation!.NameEn,
                        Religion = lang == Localization.Arabic ? x.Religion!.Name : x.Religion!.NameInEnglish,
                        SalaryPaymentWay = lang == Localization.Arabic ? x.SalaryPaymentWay!.Name : x.SalaryPaymentWay!.NameInEnglish,

                    }, orderBy: x =>
                        x.OrderByDescending(x => x.Id))).ToList(),
                CurrentPage = model.PageNumber,
                FirstPageUrl = host + $"?PageSize={model.PageSize}&PageNumber=1&IsDeleted={model.IsDeleted}",
                From = (page - 1) * model.PageSize + 1,
                To = Math.Min(page * model.PageSize, totalRecords),
                LastPage = totalPages,
                LastPageUrl = host + $"?PageSize={model.PageSize}&PageNumber={totalPages}&IsDeleted={model.IsDeleted}",
                PreviousPage = page > 1 ? host + $"?PageSize={model.PageSize}&PageNumber={page - 1}&IsDeleted={model.IsDeleted}" : null,
                NextPageUrl = page < totalPages ? host + $"?PageSize={model.PageSize}&PageNumber={page + 1}&IsDeleted={model.IsDeleted}" : null,
                Path = host,
                PerPage = model.PageSize,
                Links = pageLinks
            };

            if (result.TotalRecords is 0)
            {
                string resultMsg = shareLocalizer[Localization.NotFoundData];

                return new()
                {
                    Data = new()
                    {
                        Items = []
                    },
                    Error = resultMsg,
                    Msg = resultMsg
                };
            }

            return new()
            {
                Data = result,
                Check = true
            };
        }
        #endregion



        #region Insert

        public async Task<Response<CreateEmployeeRequest>> CreateEmployeeAsync(CreateEmployeeRequest model)
        {
            var fullNameAr = $"{model.FirstNameAr} {model.FatherNameAr} {model.GrandFatherNameAr} {model.FamilyNameAr}";
            var fullNameEn = $"{model.FirstNameEn} {model.FatherNameEn} {model.GrandFatherNameEn} {model.FamilyNameEn}";

            var exists = await unitOfWork.Employees.ExistAsync(x => x.FullNameEn.Trim() == fullNameEn.Trim()
                                                                    && x.FullNameAr.Trim() == fullNameAr.Trim());
            if (exists)
            {
                string resultMsg = string.Format(shareLocalizer[Localization.IsExist],
                    shareLocalizer[Localization.Employee]);

                return new()
                {
                    Error = resultMsg,
                    Msg = resultMsg,
                    Check = false
                };
            }

            using var transaction = unitOfWork.BeginTransaction();
            try
            {
                var newEmployee = mapper.Map<HrEmployee>(model);

                GetFileNameAndExtension imageFile = new();
                if (model.EmployeeImageFile != null)
                {
                    imageFile = ManageFilesHelper.UploadFile(model.EmployeeImageFile, GoRootPath.EmployeeImagesPath);
                }

                List<GetFileNameAndExtension> employeeAttachments = [];
                if (model.EmployeeAttachments is not null && model.EmployeeAttachments.Any())
                {
                    employeeAttachments =
                        ManageFilesHelper.UploadFiles(model.EmployeeAttachments, GoRootPath.HRFilesPath);
                }

                newEmployee.EmployeeImage = imageFile?.FileName;
                newEmployee.EmployeeImageExtension = imageFile?.FileExtension;
                newEmployee.ListOfAttachments = employeeAttachments?.Select(f => new HrEmployeeAttachment
                {
                    FileExtension = f.FileExtension,
                    FileName = f.FileName,
                    IsActive = true
                }).ToList();
                await unitOfWork.Employees.AddAsync(newEmployee);

                var newUser = await unitOfWork.Users.AddAsync(new ApplicationUser()
                {
                    UserName = model.Username,
                    Id = Guid.NewGuid().ToString(),
                    NormalizedUserName = model.Username.ToUpper(),
                    Email = newEmployee.Email,
                    NormalizedEmail = newEmployee.Email.ToUpper(),
                    EmailConfirmed = true,
                    IsActive = true,
                    PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(null!, model.Password),
                    VisiblePassword = model.Password

                });
                if (newUser != null)
                {
                    newEmployee.UserId = newUser.Id;
                    await unitOfWork.UserRoles.AddAsync(new ApplicationUserRole()
                    {
                        RoleId = UserRole.Id,
                        UserId = newUser.Id
                    });
                }

               
                await unitOfWork.CompleteAsync();
                transaction.Commit();
                return new()
                {
                    Msg = string.Format(shareLocalizer[Localization.Done],
                        shareLocalizer[Localization.Employee]),
                    Check = true,
                    Data = model
                };
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return new()
                {
                    Msg = string.Format(shareLocalizer[Localization.Error],
                        shareLocalizer[Localization.Employee]),
                    Check = false,
                    Data = model,
                    Error = ex.Message
                };
            }


        }


        #endregion

        #region Update

        public async Task<Response<CreateEmployeeRequest>> UpdateEmployeeAsync(int id, CreateEmployeeRequest model)
        {


            Expression<Func<HrEmployee, bool>> filter = x => x.IsDeleted == false && x.Id == id;
            var obj = await unitOfWork.Employees.GetFirstOrDefaultAsync(filter, includeProperties: $"{nameof(_instanceEmployee.ListOfAttachments)}");

            if (obj is null)
            {

                string resultMsg = string.Format(shareLocalizer[Localization.CannotBeFound],
                    shareLocalizer[Localization.Employee]);

                return new()
                {
                    Data = model,
                    Error = resultMsg,
                    Msg = resultMsg
                };
            }


            var fullNameAr = $"{model.FirstNameAr} {model.FatherNameAr} {model.GrandFatherNameAr} {model.FamilyNameAr}";
            var fullNameEn = $"{model.FirstNameEn} {model.FatherNameEn} {model.GrandFatherNameEn} {model.FamilyNameEn}";

            var exists = await unitOfWork.Employees.ExistAsync(x => x.FullNameEn.Trim() == fullNameEn.Trim()
                                                                    && x.FullNameAr.Trim() == fullNameAr.Trim() && x.Id != id);
            if (exists)
            {
                string resultMsg = string.Format(shareLocalizer[Localization.IsExist],
                    shareLocalizer[Localization.Employee]);

                return new()
                {
                    Error = resultMsg,
                    Msg = resultMsg,
                    Check = false
                };
            }

            var transaction = unitOfWork.BeginTransaction();
            try
            {
                if (!string.IsNullOrEmpty(obj.EmployeeImage))
                {
                    string file = Path.Combine(GoRootPath.EmployeeImagesPath, obj.EmployeeImage);
                    ManageFilesHelper.RemoveFile(file);
                }

                if (obj.ListOfAttachments.Any())
                {
                    ManageFilesHelper.RemoveFiles(obj.ListOfAttachments.Select(f => Path.Combine(GoRootPath.HRFilesPath, f.FileName)).ToList());
                    unitOfWork.EmployeeAttachments.RemoveRange(obj.ListOfAttachments);
                }



                GetFileNameAndExtension imageFile = new();
                if (model.EmployeeImageFile != null)
                {
                    imageFile = ManageFilesHelper.UploadFile(model.EmployeeImageFile, GoRootPath.EmployeeImagesPath);
                }

                List<GetFileNameAndExtension> employeeAttachments = [];
                if (model.EmployeeAttachments is not null && model.EmployeeAttachments.Any())
                {
                    employeeAttachments = ManageFilesHelper.UploadFiles(model.EmployeeAttachments, GoRootPath.HRFilesPath);
                }
                obj.Address = model.Address;
                obj.AccountNo = model.AccountNo;
                obj.Email = model.Email;
                obj.BirthDate = model.BirthDate;
                obj.ChildrenNumber = model.ChildrenNumber;
                obj.CompanyId = model.CompanyId;
                obj.DepartmentId = model.DepartmentId;
                obj.EmployeeTypeId = model.EmployeeTypeId;
                obj.VacationId = model.VacationId;
                obj.FamilyNameAr = model.FamilyNameAr;
                obj.FamilyNameEn = model.FamilyNameEn;
                obj.FirstNameEn = model.FirstNameEn;
                obj.FirstNameAr = model.FirstNameAr;
                obj.FatherNameAr = model.FatherNameAr;
                obj.FatherNameEn = model.FatherNameEn;
                obj.GrandFatherNameAr = model.GrandFatherNameAr;
                obj.GrandFatherNameEn = model.GrandFatherNameEn;
                obj.FingerPrintCode = model.FingerPrintCode;
                obj.FingerPrintId = model.FingerPrintId;
              
                obj.Phone = model.Phone;
                obj.GenderId = model.GenderId;
                obj.QualificationId = model.QualificationId;
                obj.ShiftId = model.ShiftId;
                obj.NationalId = model.NationalId;
                obj.NationalityId = model.NationalityId;
                obj.JobId = model.JobId;
                obj.JobNumber = model.JobNumber;
                obj.HiringDate = model.HiringDate;
                obj.ImmediatelyDate = model.ImmediatelyDate;
                obj.IsActive = model.IsActive;
                obj.EmployeeImage = imageFile?.FileName;
                obj.EmployeeImageExtension = imageFile?.FileExtension;
                obj.ListOfAttachments = employeeAttachments?.Select(f => new HrEmployeeAttachment
                {
                    FileExtension = f.FileExtension,
                    FileName = f.FileName,
                    IsActive = true
                }).ToList();
                unitOfWork.Employees.Update(obj);


                var userExist = await userManager.FindByNameAsync(model.Username);
                if (userExist != null)
                {
                    userExist.VisiblePassword = model.Password;
                    userExist.Email = obj.Email;
                    userExist.NormalizedEmail = obj.Email.ToUpper();
                    userExist.PasswordHash =
                        new PasswordHasher<ApplicationUser>().HashPassword(userExist, model.Password);
                    obj.UserId = userExist.Id;
                    await userManager.UpdateAsync(userExist);
                }
                else
                {
                   var newUser=await unitOfWork.Users.AddAsync(new ApplicationUser()
                    {
                        VisiblePassword = model.Password,
                        Email = obj.Email,
                        ConcurrencyStamp = "1",
                        NormalizedEmail = obj.Email.ToUpper(),
                        PhoneNumber = obj.Phone,
                        IsActive = true,
                        PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(null!,model.Password),
                        Id = Guid.NewGuid().ToString(),
                        UserName = model.Username,
                        NormalizedUserName = model.Username.ToUpper(),
                        
                    });
                    if (newUser != null)
                    {
                        obj.UserId= newUser.Id;
                       await unitOfWork.UserRoles.AddAsync(new ApplicationUserRole()
                        {
                            RoleId = UserRole.Id,
                            UserId = newUser.Id,

                        });
                    }
                }

                await unitOfWork.CompleteAsync();
                transaction.Commit();
                return new()
                {
                    Msg = string.Format(shareLocalizer[Localization.Done],
                        shareLocalizer[Localization.Employee]),
                    Check = true,
                    Data = model
                };

            }
            catch (Exception e)
            {
                transaction.Rollback();
                return new()
                {
                    Msg = shareLocalizer[Localization.Error],
                    Check = false,
                    Data = model,
                    Error = e.Message
                };
            }
        }

        #endregion

        public Task<Response<string>> UpdateActiveOrNotEmployeeAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<string>> DeleteEmployeeAsync(int id)
        {
            var transaction = unitOfWork.BeginTransaction();
            try

            {
                Expression<Func<HrEmployee, bool>> filter = x => x.Id == id;
                var obj = await unitOfWork.Employees.GetFirstOrDefaultAsync(filter, includeProperties: $"{nameof(_instanceEmployee.ListOfAttachments)}");

                if (obj == null)
                {
                    string resultMsg = string.Format(shareLocalizer[Localization.CannotBeFound],
                        shareLocalizer[Localization.Employee]);

                    return new()
                    {
                        Data = string.Empty,
                        Error = resultMsg,
                        Msg = resultMsg
                    };
                }

                if (!string.IsNullOrEmpty(obj.EmployeeImage))
                {
                    string file = Path.Combine(GoRootPath.EmployeeImagesPath, obj.EmployeeImage);
                    ManageFilesHelper.RemoveFile(file);
                }

                if (obj.ListOfAttachments.Any())
                {
                    ManageFilesHelper.RemoveFiles(obj.ListOfAttachments.Select(f => Path.Combine(GoRootPath.HRFilesPath, f.FileName)).ToList());
                    unitOfWork.EmployeeAttachments.RemoveRange(obj.ListOfAttachments);
                }

                unitOfWork.Employees.Remove(obj);
                if (!string.IsNullOrEmpty(obj.UserId))
                {
                    var userData = await unitOfWork.Users.GetFirstOrDefaultAsync(c => c.Id == obj.UserId);
                    if (userData != null)
                    {
                        var userRoles = await unitOfWork.UserRoles.GetAllAsync(c => c.UserId == userData.Id);
                        if (userRoles.Any())
                        {
                            unitOfWork.UserRoles.RemoveRange(userRoles);
                        }
                        unitOfWork.Users.Remove(userData);
                    }
                }
              
                await unitOfWork.CompleteAsync();


                transaction.Commit();
                return new()
                {
                    Check = true,
                    Data = string.Empty,
                    Msg = shareLocalizer[Localization.Deleted]
                };
            }
            catch (Exception e)
            {
                transaction.Rollback();
                return new()
                {
                    Check = false,
                    Error = e.Message,
                    Msg = shareLocalizer[Localization.Error],

                };
            }
        }
    }
}
