namespace Kader_System.DataAccess.Repositories.HR;

public class EmployeeRepository(KaderDbContext context) : BaseRepository<HrEmployee>(context), IEmployeeRepository
{ 
}
