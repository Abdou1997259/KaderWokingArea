namespace Kader_System.DataAccess.Repositories.Trans;

public class TransDeductionRepository(KaderDbContext context) : BaseRepository<TransDeduction>(context), ITransDeductionRepository
{
}
