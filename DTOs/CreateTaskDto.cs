using System.ComponentModel.DataAnnotations;

namespace TaskFlowAPI.DTOs
{
    public class CreateTaskDto
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [Range(1, int.MaxValue)]
        public int ProjectId { get; set; }

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        public bool IsCompleted { get; set; }
    }
}
