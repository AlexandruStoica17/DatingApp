using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;

[Table("Photos")]
public class Photo
{
    public int Id { get; set; }
    public required string Url { get; set; }
    public bool IsMain { get; set; }
    public string? PublicId { get; set; }

    //Navigation properties
    //required one to many navigation more on link https://learn.microsoft.com/en-us/ef/core/modeling/relationships/one-to-many

    public int AppUserId { get; set; }
    public AppUser AppUser { get; set; } = null!;
}