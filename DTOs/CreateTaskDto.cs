using System.ComponentModel.DataAnnotations;
using TaskFlowAPI.Attributes;

namespace TaskFlowAPI.DTOs
{
    public class CreateTaskDto
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        [StartsWithLetter("A", ErrorMessage = "The title is required to begin with the letter 'A' or 'a'.")]
        public string Title { get; set; } = string.Empty;

        [Required]
        [Range(1, int.MaxValue)]
        [ProjectExists()]
        public int ProjectId { get; set; }

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        public bool IsCompleted { get; set; }
    }
}
