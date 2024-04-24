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
            }).ToList();
            return tasks;
        }
        public TaskDto? GetTask(int taskId)
        {
            var user = _userService.loggedUser();
            if (user == null)
            {
                return null;
            }
            var project = _context.Projects.SingleOrDefault(x => x.Id == taskId && x.UserId == user.Id);
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
            var task = _context.Tasks.Include(x => x.Project).SingleOrDefault(x => x.Id == taskId && x.Project.UserId == user.Id);
            if(task == null)
            {
                return;
            }
            _context.Tasks.Remove(task);
            _context.SaveChanges();
        }
        public void AddTask(CreateTaskDto dto, int projectId)
        {
            var user = _userService.loggedUser();
            if (user == null)
            {
                return;
            }
            var project = _context.Projects.SingleOrDefault(x => x.Id == projectId && x.UserId == user.Id);
            if(project == null)
            {
                return;
            }
            var task = new Entities.Task()
            {
                Name = dto.Name,
                Description = dto.Description,
                Category = dto.Category,
                PriorityLevel = dto.PriorityLevel,
                DeadLine = dto.DeadLine,
                Project = project,
                ProjectId = project.Id,
            };
            _context.Tasks.Add(task);
            _context.SaveChanges();
        }
        public void EditTask(int taskId, CreateTaskDto dto)
        {
            var user = _userService.loggedUser();
            if (user == null)
            {
                return;
            }
            var task = _context.Tasks.Include(x => x.Project).SingleOrDefault(x => x.Id == taskId && x.Project.UserId == user.Id);
            if(task == null)
            {
                return;
            }
            task.Name = dto.Name;
            task.Description = dto.Description;
            task.Category = dto.Category;
            task.PriorityLevel = dto.PriorityLevel;
            task.DeadLine = dto.DeadLine;
            _context.Tasks.Update(task);
            _context.SaveChanges();
        }
        public void ToggleDone(int taskId)
        {
            var user = _userService.loggedUser();
            if (user == null)
            {
                return;
            }
            var task = _context.Tasks.Include(x => x.Project).SingleOrDefault(x => x.Id == taskId && x.Project.UserId == user.Id);
            if (task == null)
            {
                return;
            }
            task.IsDone = !task.IsDone;
            _context.Tasks.Update(task);
            _context.SaveChanges();
        }
    }
}
