namespace Kader_System.DataAccess.Repositories.HR;

public class ShiftRepository(KaderDbContext context) : BaseRepository<HrShift>(context), IShiftRepository
{
}
