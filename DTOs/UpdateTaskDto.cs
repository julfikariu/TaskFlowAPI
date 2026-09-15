using System.ComponentModel.DataAnnotations;

namespace TaskFlowAPI.DTOs
{
    public class UpdateTaskDto
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Title { get; set; } = string.Empty;

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
    }
}
