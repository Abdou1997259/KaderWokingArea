using Kader_System.Domain.DTOs;
using Kader_System.Domain.DTOs.Request.HR.Loan;
using Kader_System.Domain.DTOs.Response.HR.Loan;

namespace Kader_System.Services.Services.HR
{
    public class LoanService(IUnitOfWork unitOfWork, IStringLocalizer<SharedResource> sharLocalizer, IMapper mapper) : ILoanService
    {
        private readonly IMapper _mapper = mapper;
        public async Task<Response<CreateLoanRequest>> CreateLoanAsync(CreateLoanRequest loan)
        {

            await unitOfWork.LoanRepository.AddAsync(_mapper.Map<HrLoan>(loan));
            await unitOfWork.CompleteAsync();
            return new()
            {
                Check = true,
                Data = loan,
                Msg = sharLocalizer[Localization.Done]
            };

        }

        public Task<Response<string>> DeleteLoanAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<GetAllLoansReponse>> GetAllLoansAsync(string lang, GetAllFiterationForLoansRequest model, string host)
        {
            Expression<Func<HrLoan, bool>> filters = x => x.IsDeleted == model.IsDeleted && (string.IsNullOrEmpty(model.Word)
            || x.StartLoanDate.Equals(model.From) || x.EndDoDate.Equals(model.To));
            //pagination preprations

            var totalRecord = await unitOfWork.LoanRepository.CountAsync(filter: filters);
            var page = 1;
            int totalPages = (int)Math.Ceiling((decimal)totalRecord / (model.PageSize == 0 ? 10 : model.PageSize));
            if (model.PageSize < 1)
                page = 1;
            else
                page = model.PageSize;


            //end

            //prepare for Links in pagination
            var pageLinks = Enumerable.Range(1, totalPages)
                .Select(p => new Link
                {
                    label = p.ToString(),
                    url = $"{host}?PageSize={model.PageSize}={p}&IsDeleted={model.IsDeleted}&From={model.From}&To={model.To}",
                    active = p == model.PageNumber
                }).ToList();
            //end

            //Fiilterd and paginated items for returning
            var ItemsPaginated = await unitOfWork.LoanRepository.GetSpecificSelectAsync(filters, take: model.PageSize, skip: (model.PageNumber - 1) * model.PageSize,

                  select: x => new LoanData
                  {
                      StartLoanDate = x.StartLoanDate,
                      DocumentDate = x.DocumentDate,
                      EndDoDate = x.EndDoDate,
                      DocumentType = x.DocumentType,
                      InstallmentCount = x.InstallmentCount,
                      MonthlyDeducted = x.MonthlyDeducted,
                      Notes = x.Notes,
                      MakePaymentJournal = x.MakePaymentJournal,
                      IsDeductedFromSalary = x.IsDeductedFromSalary,
                      PrevDedcutedAmount = x.PrevDedcutedAmount,
                      EmpolyeeId = x.EmpolyeeId,


                  }
                );
            //end the segmentaions


            var returingResult = new GetAllLoansReponse
            {
                CurrentPage = model.PageNumber,
                Links = pageLinks,
                From = (page - 1) * model.PageSize + 1,
                To = Math.Min(page * model.PageSize, totalRecord),
                LastPage = totalPages,
                FirstPageUrl = host + $"?PageSize={model.PageSize}&PageNumber=1&IsDeleted={model.IsDeleted}",
                PreviousPage = page > 1 ? host + $"?PageSize={model.PageSize}&PageNumber={page - 1}&IsDeleted={model.IsDeleted}" : null,
                NextPageUrl = page < totalPages ? $"{host}?PageSize={model.PageSize}&PageNumber={totalPages}&IsDeleted={model.IsDeleted}" : null,
                LastPageUrl = host + $"?PageSize={model.PageSize}&PageNumber={totalPages}&IsDeleted={model.IsDeleted}",
                Path = host,
                PerPage = model.PageSize,

            };



            if (returingResult.TotalRecords is 0)
            {
                string resultMsg = sharLocalizer[Localization.NotFoundData];

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
                Check = true,
                Data = returingResult
            };





        }

        public async Task<Response<GetLoanByIdResponse>> GetLoanByIdAsync(int id)
        {


            var obj = await unitOfWork.LoanRepository.GetByIdAsync(id);
            if (obj is null)
            {
                string resultMsg = sharLocalizer[Localization.NotFoundData];
                return new()
                {
                    Error = resultMsg,
                    Data = new(),
                    Msg = resultMsg,

                };
            };

            return new()
            {
                Data = _mapper.Map<GetLoanByIdResponse>(obj),
                Check = true,
            };


        }

        public async Task<Response<IEnumerable<ListOfLoansReponse>>> ListOfLoansAsync(string lang)
        {
            var result = await unitOfWork.LoanRepository.GetSpecificSelectAsync(null!,

                select: x => new ListOfLoansReponse
                {
                    StartLoanDate = x.StartLoanDate,
                    DocumentDate = x.DocumentDate,
                    EndDoDate = x.EndDoDate,
                    DocumentType = x.DocumentType,
                    InstallmentCount = x.InstallmentCount,
                    MonthlyDeducted = x.MonthlyDeducted,
                    Notes = x.Notes,
                    MakePaymentJournal = x.MakePaymentJournal,
                    IsDeductedFromSalary = x.IsDeductedFromSalary,
                    PrevDedcutedAmount = x.PrevDedcutedAmount,
                    EmpolyeeId = x.EmpolyeeId,


                }, orderBy: x => x.OrderByDescending(o => o.Id));


            if (!result.Any())
            {
                string resultMessage = sharLocalizer[Localization.NotFoundData];
                return new()
                {
                    Data = [],
                    Error = resultMessage,
                    Msg = resultMessage
                };

            }
            return new()
            {
                Data = result,
                Check = true
            };

        }

        public Task<Response<GetLoanByIdResponse>> RestoreLoanAsync(int id)
        {
            throw new NotImplementedException();
            //    var obj = await unitOfWork.Jobs.GetByIdAsync(id);

            //    if (obj == null)
            //    {
            //        string resultMsg = string.Format(sharLocalizer[Localization.CannotBeFound],
            //            sharLocalizer[Localization.Qualification]);

            //        return new()
            //        {
            //            Data = null,
            //            Error = resultMsg,
            //            Msg = resultMsg
            //        };
            //    }

            //    obj.IsDeleted = false;

            //    unitOfWork.Jobs.Update(obj);
            //    await unitOfWork.CompleteAsync();

            //    return new()
            //    {
            //        Check = true,
            //        Data = new()
            //        {
            //            HasAdditionalTime = obj.HasAdditionalTime,
            //            HasNeedLicense = obj.HasNeedLicense,
            //            Id = obj.Id,
            //            NameAr = obj.NameAr,
            //            NameEn = obj.NameEn
            //        },
            //        Msg = sharLocalizer[Localization.Updated]
            //    };
            //

        }

        public async Task<Response<string>> UpdateActiveOrNotLoanAsync(int id)
        {


            throw new NotImplementedException();
        }

        public async Task<Response<UpdateLoanRequest>> UpdateLoanAsync(int id, UpdateLoanRequest model)
        {

            var obj = await unitOfWork.LoanRepository.GetByIdAsync(id);
            if (obj == null)
            {
                string resultMsg = string.Format(sharLocalizer[Localization.CannotBeFound], sharLocalizer[Localization.Loan]);
                return new()
                {
                    Msg = resultMsg,
                    Error = resultMsg,
                    Data = model
                };

            }
            var mapResult = _mapper.Map<HrLoan>(obj);
            unitOfWork.LoanRepository.Update(mapResult);
            await unitOfWork.CompleteAsync();
            return new()
            {
                Check = true,
                Data = model,
                Msg = sharLocalizer[Localization.Updated]
            };

        }
    }
}
