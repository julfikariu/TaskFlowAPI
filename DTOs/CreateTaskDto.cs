using System.ComponentModel.DataAnnotations;

namespace TaskFlowAPI.DTOs
{
    public class CreateTaskDto
    {
        [Required]
        public string Title { get; set; } = string.Empty;

        public bool IsCompleted { get; set; }
    }
}
