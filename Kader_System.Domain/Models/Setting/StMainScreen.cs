namespace Kader_System.Domain.Models.Setting;

[Table("St_MainScreens")]
public class StMainScreen : BaseEntity
{
    [Key]
    public int Id { get; set; }
    public required string Screen_cat_title_en { get; set; }
    public required string Screen_cat_title_ar { get; set; }

    public int Screen_cat_id { get; set; }
    [ForeignKey(nameof(Screen_cat_id))]
    public StMainScreenCategory MainScreenCategory { get; set; } = default!;
}
