﻿
using Kader_System.Domain.DTOs.Response.HR;

namespace Kader_System.Services.Services.HR
{
    public class FingerPrintDeviceService(IUnitOfWork unitOfWork, IStringLocalizer<SharedResource> shareLocalizer, IMapper mapper) : IFingerPrintDeviceService
    {
        private HrFingerPrint _fingerPrintInstance;
      
        #region MyRegion

        public async Task<Response<GetAllFingerPrintDevicesResponse>> GetAllFingerPrintDevicesAsync(string lang,GetAllFingerPrintDevicesFilterrationRequest request)
        {
            Expression<Func<HrFingerPrint, bool>> filter = x => x.IsDeleted == request.IsDeleted;
            var result = new GetAllFingerPrintDevicesResponse
            {
                TotalRecords = await unitOfWork.FingerPrints.CountAsync(filter),
                Items = (await unitOfWork.FingerPrints.GetSpecificSelectAsync(filter: filter,
                    includeProperties: $"{nameof(_fingerPrintInstance.Company)}",
                    take: request.PageSize,
                    skip: (request.PageNumber - 1) * request.PageSize,
                    select:
                    d => new FingerPrintDeviceData()
                    {
                        Id = d.Id,
                        NameAr = d.NameAr,
                        NameEn = d.NameEn,
                        IPAddress = d.IPAddress,
                        Port = d.Port,
                        CompanyId = d.CompanyId,
                        CompanyName = lang == Localization.Arabic ? d.Company!.NameAr : d.Company!.NameEn,
                        Username = d.Username,
                        Password = d.Password

                    })).ToList(),

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

        public async Task<Response<GetFingerPrintDeviceByIdResponse>> GetFingerPrintDeviceByIdAsync(int id)
        {
            Expression<Func<HrFingerPrint, bool>> filter = x => x.Id == id;
            var obj = await unitOfWork.FingerPrints.GetFirstOrDefaultAsync(filter, includeProperties: $"{nameof(_fingerPrintInstance.Company)}");

            if (obj is null)
            {
                string resultMsg = string.Format(shareLocalizer[Localization.NotFoundData], shareLocalizer[Localization.FingerPrintDevice]);

                return new()
                {
                    Data = new(),
                    Error = resultMsg,
                    Msg = resultMsg
                };

            }

            return new()
            {
                Check = true,
                Error = string.Empty,
                Msg = string.Empty,
                IsActive = obj.IsActive,
                Data = mapper.Map<GetFingerPrintDeviceByIdResponse>(obj)
            };
        }

        public async Task<Response<IEnumerable<ListOfFingerPrintDevicesResponse>>> GetFingerPrintDevicesAsync(string lang)
        {
            Expression<Func<HrFingerPrint, bool>> filter = x => x.IsDeleted == false;
            var result=  unitOfWork.FingerPrints.GetSpecificSelectAsync(filter,
                select: (d => new ListOfFingerPrintDevicesResponse()
                {
                    Id = d.CompanyId,
                    Name = lang==Localization.Arabic ? d.NameAr:d.NameEn,
                    IPAddress = d.IPAddress,
                    Port = d.Port,
                    CompanyId = d.CompanyId
                })).Result.ToList();
            if (!result.Any())
            {
                string resultMsg = shareLocalizer[Localization.NotFoundData];

                return new()
                {
                    Data = [],
                    Error = resultMsg,
                    Msg = resultMsg,
                    Check = false
                };
            }

            return new()
            {
                Data = result,
                Check = true
            };
        }

        #endregion


        #region Create
        public async Task<Response<CreateFingerPrintDeviceRequest>> CreateFingerPrintDevicesAsync(CreateFingerPrintDeviceRequest request)
        {
            var exists = await unitOfWork.FingerPrints.ExistAsync(x => x.NameAr.Trim() == request.NameAr.Trim()
                                                                    && x.NameEn.Trim() == request.NameEn.Trim());

            if (exists)
            {
                string resultMsg = string.Format(shareLocalizer[Localization.IsExist],
                    shareLocalizer[Localization.FingerPrintDevice]);
                return new()
                {
                    Check = false,
                    Data = request,
                    Error = resultMsg,
                    Msg = resultMsg
                };
            }

            var newDevice = mapper.Map<HrFingerPrint>(request);
            await unitOfWork.FingerPrints.AddAsync(newDevice);
            await unitOfWork.CompleteAsync();
            return new()
            {
                Msg = string.Format(shareLocalizer[Localization.Done],
                    shareLocalizer[Localization.FingerPrintDevice]),
                Check = true,
                Data = request
            };
        }


        #endregion

        #region Update
        public async Task<Response<CreateFingerPrintDeviceRequest>> UpdateFingerPrintDevicesAsync(int id, CreateFingerPrintDeviceRequest request)
        {
            var obj = await unitOfWork.FingerPrints.GetByIdAsync(id);
            if (obj == null)
            {
                string resultMsg = string.Format(shareLocalizer[Localization.NotFoundData],
                    shareLocalizer[Localization.FingerPrintDevice]);
                return new()
                {
                    Check = false,
                    Data = request,
                    Error = resultMsg,
                    Msg = resultMsg
                };
            }

            obj.Username = request.Username;
            obj.Password = request.Password;
            obj.CompanyId=request.CompanyId;
            obj.IPAddress=request.IPAddress;
            obj.Port=request.Port;
            obj.NameAr=request.NameAr;
            obj.NameEn=request.NameEn;
            unitOfWork.FingerPrints.Update(obj);
            await unitOfWork.CompleteAsync();

            return new()
            {
                Check = true,
                Data = request,
                Error = string.Empty,
                Msg = shareLocalizer[Localization.Done],
                IsActive = obj.IsActive
            };
        }
        public Task<Response<string>> UpdateActiveOrNotEmployeeAsync(int id)
        {
            throw new NotImplementedException();
        }


        #endregion


        #region Delete

        public async Task<Response<string>> DeleteFingerPrintAsync(int id)
        {
            var obj = await unitOfWork.FingerPrints.GetByIdAsync(id);
            if (obj == null)
            {
                string resultMsg = string.Format(shareLocalizer[Localization.NotFoundData],
                    shareLocalizer[Localization.FingerPrintDevice]);
                return new()
                {
                    Check = false,
                    Data = string.Empty,
                    Error = resultMsg,
                    Msg = resultMsg
                };
            }

            try
            {
                unitOfWork.FingerPrints.Remove(obj);
                await unitOfWork.CompleteAsync();
                return new()
                {
                    Check = true,
                    Data = string.Empty,
                    Error = string.Empty,
                    Msg = shareLocalizer[Localization.Deleted]
                };
            }
            catch (Exception e)
            {
                return new()
                {
                    Check = false,
                    Data = string.Empty,
                    Error = e.Message,
                    Msg = e.Message
                };
            }
        }

        #endregion

    }
}
