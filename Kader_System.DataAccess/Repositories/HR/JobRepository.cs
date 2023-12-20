namespace Kader_System.DataAccess.Repositories.HR;

public class JobRepository(KaderDbContext context) : BaseRepository<HrJob>(context), IJobRepository
{
}
