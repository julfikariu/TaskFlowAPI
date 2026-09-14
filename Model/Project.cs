namespace TaskFlowAPI.Model
{
    public class Project
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime Startdate { get; set; }
        public DateTime? Enddate { get; set; }

        public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();

    }
}
