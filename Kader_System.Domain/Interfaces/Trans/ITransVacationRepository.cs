namespace Kader_System.Domain.Interfaces.Trans
{
    public interface ITransVacationRepository : IBaseRepository<TransVacation>
    {
        List<TransVacationData> GetTransVacationInfo(
            Expression<Func<TransVacation, bool>> filter,
            Expression<Func<TransVacationData, bool>> filterSearch,
            int? skip = null,
            int? take = null, string lang = "ar"
        );
    }
}
