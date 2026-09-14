using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskFlowAPI.Data;
using TaskFlowAPI.DTOs.Project;
using TaskFlowAPI.Model;

namespace TaskFlowAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProjectsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var projects = await _context.Projects
                .Select(project => new ProjectResponseDto
                {
                    Id = project.Id,
                    Name = project.Name,
                    Description = project.Description,
                    StartDate = project.Startdate,
                    EndDate = project.Enddate,
                    TaskCount = project.Tasks.Count
                })
                .ToListAsync();

            return Ok(projects);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id) 
        {
            var project = await _context.Projects.FindAsync(id);

            if (project == null)
            {
                return NotFound();
            }

            var response = new ProjectResponseDto
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                StartDate = project.Startdate,
                EndDate = project.Enddate,
                TaskCount = project.Tasks.Count
            };

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProjectDto dto)
        {
            var project = new Project
            {
                Name = dto.Name,
                Description = dto.Description,
                Startdate = dto.StartDate,
                Enddate = dto.EndDate,
            };

            _context.Projects.Add(project);

            await _context.SaveChangesAsync();

            var response = new ProjectResponseDto
            {
                Id = project.Id,
                Name = dto.Name,
                Description= dto.Description,
                StartDate= dto.StartDate,
                EndDate = dto.EndDate,
            };

            return CreatedAtAction(
                       nameof(GetById),
                       new { id = project.Id },
                       response
                   );

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateProjectDto dto)
        {
            var project = await _context.Projects.FindAsync(id);

            if (project == null)
                return NotFound();

            project.Name = dto.Name;
            project.Description = dto.Description;
            project.Startdate = dto.StartDate;
            project.Enddate = dto.EndDate;

            await _context.SaveChangesAsync();

            var response = new ProjectResponseDto
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                StartDate = project.Startdate,
                EndDate = project.Enddate,
                //TaskCount = await _context.Tasks.CountAsync(task => task.ProjectId == project.Id)
                TaskCount = project.Tasks?.Count ?? 0,
            };

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            _context.Projects.Remove(project);

            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
