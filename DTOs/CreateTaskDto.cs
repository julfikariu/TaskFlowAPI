using System.ComponentModel.DataAnnotations;

namespace TaskFlowAPI.DTOs
{
    public class CreateTaskDto
    {
        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        public int ProjectId { get; set; }

        public string Description { get; set; } = string.Empty;

        public bool IsCompleted { get; set; }
    }
}
