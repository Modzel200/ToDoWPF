using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ToDo.Entities;
using ToDo.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ToDo.Services
{
    public class ProjectService
    {
        private readonly ToDoDbContext _context;
        private readonly UserService _userService;
        public ProjectService(ToDoDbContext context, UserService userService)
        {
            this._context = context;
            this._userService = userService;
        }
        public IEnumerable<ProjectDto> GetAllProjects(ProjectFilterSortDto searchQuery)
        {
            var user = _userService.loggedUser();
            if (user == null)
            {
                return null;
            }
            var baseQuery = _context.Projects.Include(x => x.Tasks).Where(x => (x.UserId == user.Id) && (searchQuery.Search == null ||
                (x.Name.ToLower().Contains(searchQuery.Search.ToLower()) || x.Description.ToLower().Contains(searchQuery.Search.ToLower()))) && (searchQuery.IsDone == null ||
                (x.IsDone == searchQuery.IsDone)) && (searchQuery.Colors.Any() ||
                (searchQuery.Colors.Contains(x.Color)))).Select(x => new ProjectDto()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Color = x.Color,
                    DeadLine = x.DeadLine,
                    IsDone = x.IsDone,
                    DoneRatio = x.Tasks.Where(y => y.IsDone == true).Count()+"/"+x.Tasks.Count(),
                }
                );
            var columnsSelectors = new Dictionary<string, Expression<Func<ProjectDto, object>>>
            {
                { nameof(ProjectDto.Id), x => x.Id},
                { nameof(ProjectDto.Name), x => x.Name},
                { nameof(ProjectDto.DeadLine), x => x.DeadLine},
            };
            var selectedColumn = columnsSelectors[searchQuery.SortBy];

            baseQuery = searchQuery.SortDirection == SortDirection.ASC ?
                    baseQuery.OrderBy(selectedColumn)
                    : baseQuery.OrderByDescending(selectedColumn);

            return baseQuery;
        }
        public ProjectDto? GetProject(int projectId)
        {
            var user = _userService.loggedUser();
            if (user == null)
            {
                return null;
            }
            var project = _context.Projects.Include(x => x.Tasks).ThenInclude(x => x.SubTasks).Where(x => x.Id == projectId && x.UserId == user.Id).Select(x => new ProjectDto()
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                Color = x.Color,
                DeadLine = x.DeadLine,
                IsDone = x.IsDone,
                Tasks = x.Tasks.Select(y => new TaskDto()
                {
                    Id = y.Id,
                    Name = y.Name,
                    Description = y.Description,
                    Category = y.Category,
                    PriorityLevel = y.PriorityLevel,
                    DeadLine = y.DeadLine,
                    IsDone = y.IsDone,
                    DoneRatio = y.SubTasks.Where(z => z.isDone == true).Count() / y.SubTasks.Count(),
                }).ToList(),
            }).SingleOrDefault();
            return project;
        }
        public void DeleteProject(int projectId)
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
            if(project == null)
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
        public void CheckAndMarkAsDone(int projectId)
        {
            var user = _userService.loggedUser();
            if (user == null)
            {
                return;
            }
            var project = _context.Projects.Include(x => x.Tasks).SingleOrDefault(x => x.Id == projectId && x.UserId == user.Id);
            if (project == null)
            {
                return;
            }
            if (project.Tasks.All(x => x.IsDone == true))
            {
                project.IsDone = true;
                _context.Projects.Update(project);
                _context.SaveChanges();
            } else
            {
                project.IsDone = false;
                _context.Projects.Update(project);
                _context.SaveChanges();
            }
        }
    }
}
