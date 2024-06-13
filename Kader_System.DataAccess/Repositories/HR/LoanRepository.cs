namespace Kader_System.DataAccess.Repositories.HR
{
    public class LoanRepository(KaderDbContext context) : BaseRepository<HrLoan>(context), ILoanRepository
    {
    }
}
