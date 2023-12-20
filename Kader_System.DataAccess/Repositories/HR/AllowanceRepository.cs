namespace Kader_System.DataAccess.Repositories.HR;

public class AllowanceRepository(KaderDbContext context) : BaseRepository<HrAllowance>(context), IAllowanceRepository
{ 
}