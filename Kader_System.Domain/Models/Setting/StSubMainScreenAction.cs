namespace Kader_System.Domain.Models.Setting;

[Table("St_SubMainScreenActions")]
public class StSubMainScreenAction : BaseEntity
{
    [Key]
    public int Id { get; set; }

    public int SubMainScreenId { get; set; }
    [ForeignKey(nameof(SubMainScreenId))]
    public StSubMainScreen SubMainScreen { get; set; } = default!;

    public int ActionId { get; set; }
    [ForeignKey(nameof(ActionId))]
    public StAction Action { get; set; } = default!;
}
