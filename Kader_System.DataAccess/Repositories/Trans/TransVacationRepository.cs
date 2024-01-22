namespace Kader_System.DataAccess.Repositories.Trans
{
    public class TransVacationRepository(KaderDbContext context) : BaseRepository<TransVacation>(context), ITransVacationRepository
    {
    }
}
