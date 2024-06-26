﻿using Microsoft.EntityFrameworkCore;
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
        private readonly ProjectService _projectService;
        public TaskService(ToDoDbContext context, UserService userService, ProjectService projectService)
        {
            this._context = context;
            this._userService = userService;
            _projectService = projectService;
        }
        public IEnumerable<TaskDto> GetAllTasks(int projectId, TaskFilterSortDto queryDto)
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

            var query = _context.Tasks.Include(x => x.SubTasks).Where(x => x.ProjectId == project.Id);

            if (queryDto.CategoryFilter.HasValue)
            {
                query = query.Where(x => x.Category == queryDto.CategoryFilter.Value);
            }

            if (queryDto.Sort.HasValue)
            {
                switch (queryDto.Sort)
                {
                    case PriorityLevel.Very_Important:
                        query = query.OrderByDescending(x => x.PriorityLevel);
                        break;
                    default:
                        query = query.OrderBy(x => x.PriorityLevel);
                        break;
                }
            }

            var tasks = query.Select(x => new TaskDto()
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                Category = x.Category,
                PriorityLevel = x.PriorityLevel,
                DeadLine = x.DeadLine,
                IsDone = x.IsDone,
                DoneRatio = x.SubTasks.Where(y => y.isDone == true).Count() / (float)x.SubTasks.Count(), 
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
            var task = _context.Tasks.Include(x => x.SubTasks).Where(x => x.UserId == user.Id && x.Id == taskId).Select(x => new TaskDto()
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
            var task = _context.Tasks.SingleOrDefault(x => x.Id == taskId && x.UserId == user.Id);
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
                User = user,
                UserId = user.Id,
            };
            if (dto.DeadLine is not null)
            {
                var notification = new Notification()
                {
                    Message = "",
                    UserId = user.Id,
                };
                _context.Notifications.Add(notification);
                _context.SaveChanges();
                task.Notification = notification;
                task.NotificationId = notification.Id;
            }
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
            var task = _context.Tasks.SingleOrDefault(x => x.Id == taskId && x.UserId == user.Id);
            if(task == null)
            {
                return;
            }
            if (dto.DeadLine is not null)
            {
                var oldNotification = _context.Notifications.SingleOrDefault(x => x.Id == task.NotificationId);
                _context.Notifications.Remove(oldNotification);
                var notification = new Notification()
                {
                    Message = "",
                    UserId = user.Id,
                };
                _context.Notifications.Add(notification);
                _context.SaveChanges();
                task.Notification = notification;
                task.NotificationId = notification.Id;
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
            var task = _context.Tasks.SingleOrDefault(x => x.Id == taskId && x.UserId == user.Id);
            if (task == null)
            {
                return;
            }
            task.IsDone = !task.IsDone;
            _projectService.CheckAndMarkAsDone(task.ProjectId);
            _context.Tasks.Update(task);
            _context.SaveChanges();
        }
    }
}
