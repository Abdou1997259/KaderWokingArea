namespace Kader_System.DataAccess.Repositories.HR;

public class QualificationRepository(KaderDbContext context) : BaseRepository<HrQualification>(context), IQualificationRepository
{
}
