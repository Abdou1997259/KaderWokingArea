namespace Kader_System.DataAccess.Repositories.HR;

public class ContractRepository(KaderDbContext context) : BaseRepository<HrContract>(context), IContractRepository
{
}
