using System.ComponentModel.DataAnnotations;

namespace TaskFlowAPI.DTOs.Project
{
    public class CreateProjectDto
    {
        [Required]
        [StringLength(150)]
        public string Name { get; set; } = string.Empty;

        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}
