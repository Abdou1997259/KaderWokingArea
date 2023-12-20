namespace Kader_System.DataAccess.Repositories.HR;

public class VacationRepository(KaderDbContext context) : BaseRepository<HrVacation>(context), IVacationRepository
{
}
