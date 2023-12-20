namespace Kader_System.DataAccess.Repositories.Trans;

public class TransBenefitRepository(KaderDbContext context) : BaseRepository<TransBenefit>(context), ITransBenefitRepository
{
}
