namespace Kader_System.DataAccess.Repositories.HR;

public class AccountingWayRepository(KaderDbContext context) : BaseRepository<HrAccountingWay>(context), IAccountingWayRepository
{
}
