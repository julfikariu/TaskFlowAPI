using System.ComponentModel.DataAnnotations;

namespace TaskFlowAPI.DTOs
{
    public class UpdateTaskDto
    {
        [Required]
        [StringLength(150)]
        public string Title { get; set; } = string.Empty;

        public bool IsCompleted { get; set; }
    }
}
