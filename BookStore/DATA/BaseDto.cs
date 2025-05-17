using System.ComponentModel.DataAnnotations;

namespace BookStore.DATA;

public class BaseDto<TId>
{
    [Key] public TId Id { get; set; }

    public DateTime? CreationDate { get; set; } = DateTime.UtcNow;
}