namespace Kader_System.Domain.Models.Setting;

[Table("St_Actions")]
public class StAction : SelectList
{
    public ICollection<StSubMainScreenAction> ListOfSubMainScreen { get; set; } = [];
}
