using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDo.Entities;
using ToDo.Models;

namespace ToDo.Services
{
    public class TaskService
    {
        private readonly ToDoDbContext _context;
        private readonly UserService _userService;
        public TaskService(ToDoDbContext context, UserService userService)
        {
            this._context = context;
            this._userService = userService;
        }
        public IEnumerable<TaskDto> GetAllTasks(int projectId)
        {
            var user = _userService.loggedUser();
            if (user == null)
            {
                return null;
            }
            var project = _context.Projects.SingleOrDefault(x => x.Id == projectId && x.UserId == user.Id);
            if(project == null)
            {
                return null;
            }
            var tasks = _context.Tasks.Include(x => x.SubTasks).Where(x => x.ProjectId == project.Id).Select(x => new TaskDto()
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                Category = x.Category,
                PriorityLevel = x.PriorityLevel,
                DeadLine = x.DeadLine,
                IsDone = x.IsDone,
                DoneRatio = x.SubTasks.Where(y => y.isDone == true).Count()/x.SubTasks.Count(),
            });
            return tasks;
        }
        public TaskDto? GetTask(int id)
        {
            var user = _userService.loggedUser();
            if (user == null)
            {
                return null;
            }
            var project = _context.Projects.SingleOrDefault(x => x.Id == id && x.UserId == user.Id);
            if(project == null)
            {
                return null;
            }
            var task = _context.Tasks.Include(x => x.SubTasks).Where(x => x.ProjectId == project.Id).Select(x => new TaskDto()
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                Category = x.Category,
                PriorityLevel = x.PriorityLevel,
                DeadLine = x.DeadLine,
                IsDone = x.IsDone,
                SubTasks = x.SubTasks.Select(y => new SubTaskDto()
                {
                    Id = y.Id,
                    Description = y.Description,
                    isDone = y.isDone,
                }).ToList(),
            }).SingleOrDefault();
            return task;
        }
        public void DeleteTask(int taskId)
        {
            var user = _userService.loggedUser();
            if (user == null)
            {
                return;
            }
            var task = _context.Tasks.SingleOrDefault(x => x.Id == taskId);
            var project = _context.Projects.SingleOrDefault(x => x.Id == id && x.UserId == user.Id);
            if (project == null)
            {
                return;
            }
            _context.Projects.Remove(project);
            _context.SaveChanges();
        }
        public void AddProject(CreateProjectDto dto)
        {
            var user = _userService.loggedUser();
            if (user == null)
            {
                return;
            }
            var project = new Project()
            {
                Name = dto.Name,
                Description = dto.Description,
                Color = dto.Color,
                DeadLine = dto.DeadLine,
                User = user,
                UserId = user.Id,
            };
            _context.Projects.Add(project);
            _context.SaveChanges();
        }
        public void EditProject(int projectId, CreateProjectDto dto)
        {
            var user = _userService.loggedUser();
            if (user == null)
            {
                return;
            }
            var project = _context.Projects.SingleOrDefault(x => x.Id == projectId && x.UserId == user.Id);
            if (project == null)
            {
                return;
            }
            project.Name = dto.Name;
            project.Description = dto.Description;
            project.Color = dto.Color;
            project.DeadLine = dto.DeadLine;
            _context.Projects.Update(project);
            _context.SaveChanges();
        }
        public void ToggleDone(int projectId)
        {
            var user = _userService.loggedUser();
            if (user == null)
            {
                return;
            }
            var project = _context.Projects.SingleOrDefault(x => x.Id == projectId && x.UserId == user.Id);
            if (project == null)
            {
                return;
            }
            project.IsDone = !project.IsDone;
            _context.Projects.Update(project);
            _context.SaveChanges();
        }
    }
}
