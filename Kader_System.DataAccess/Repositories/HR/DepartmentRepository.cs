namespace Kader_System.DataAccess.Repositories.HR;

public class DepartmentRepository(KaderDbContext context) : BaseRepository<HrDepartment>(context), IDepartmentRepository
{
}
