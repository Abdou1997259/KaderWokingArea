namespace Kader_System.DataAccess.Repositories.Setting;

public class SubMainScreenActionRepository(KaderDbContext context) : BaseRepository<StSubMainScreenAction>(context), ISubMainScreenActionRepository
{
    private new readonly KaderDbContext _context = context;
    public async Task<IEnumerable<GetEachSubMainWithActions>> GetEachSubMainWithActionsAsync(string lang)
    {
        var ee = await _context.SubMainScreenActions.GroupBy(gro => gro.SubMainScreenId)
            .Select(x => new GetEachSubMainWithActions
            {
                SubMainId = x.Key,
                Actions = x.Select(y => new ActionsWithClaimValue
                {
                    ActionId = y.ActionId,
                    ActionName = lang == Localization.Arabic ? y.Action.Name : y.Action.NameInEnglish,
                    ClaimValue = $"Permissions.{y.SubMainScreen.Name}.{y.Action.NameInEnglish}"
                }).ToList()
            }).ToListAsync();

        return ee;
    }
}
