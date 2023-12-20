namespace Kader_System.DataAccess.Repositories.Trans;

public class TransAllowanceRepository(KaderDbContext context) : BaseRepository<TransAllowance>(context), ITransAllowanceRepository
{
}
