namespace Kader_System.DataAccess.Repositories.Trans;

public class TransCovenantRepository(KaderDbContext context) : BaseRepository<TransCovenant>(context), ITransCovenantRepository
{
}
