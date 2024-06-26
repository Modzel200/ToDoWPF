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
    public class SubtaskService
    {
        private readonly ToDoDbContext _context;
        private readonly UserService _userService;
        public SubtaskService(ToDoDbContext context, UserService userService)
        {
            this._context = context;
            this._userService = userService;
        }
        public IEnumerable<SubTaskDto> GetAllSubTasks(int taskId)
        {
            var user = _userService.loggedUser();
            if (user == null)
            {
                return null;
            }
            var task = _context.Tasks.SingleOrDefault(x => x.Id == taskId && x.UserId == user.Id);
            if (task == null)
            {
                return null;
            }
            var subtasks = _context.SubTasks.Where(x => x.TaskId == task.Id).Select(x => new SubTaskDto()
            {
                Id = x.Id,
                Description = x.Description,
                isDone = x.isDone,
            }).ToList();
            return subtasks;
        }
        public SubTaskDto? GetSubTask(int subtaskId)
        {
            var user = _userService.loggedUser();
            if (user == null)
            {
                return null;
            }
            var subtask = _context.SubTasks.SingleOrDefault(x => x.Id == subtaskId && x.UserId == user.Id);
            if (subtask == null)
            {
                return null;
            }
            var result = new SubTaskDto()
            {
                Id = subtaskId,
                Description = subtask.Description,
                isDone = subtask.isDone,
            };
            return result;
        }
        public void DeleteSubTask(int subtaskId)
        {
            var user = _userService.loggedUser();
            if (user == null)
            {
                return;
            }
            var subtask = _context.SubTasks.SingleOrDefault(x => x.Id == subtaskId && x.UserId == user.Id);
            if (subtask == null)
            {
                return;
            }
            _context.SubTasks.Remove(subtask);
            _context.SaveChanges();
        }
        public void AddSubTask(CreateSubTaskDto dto, int taskId)
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
            var subtask = new SubTask()
            {
                Description = dto.Description,
                Task = task,
                TaskId = task.Id,
                User = user,
                UserId = user.Id,
            };
            _context.SubTasks.Add(subtask);
            _context.SaveChanges();
        }
        public void EditTask(int subtaskId, CreateSubTaskDto dto)
        {
            var user = _userService.loggedUser();
            if (user == null)
            {
                return;
            }
            var subtask = _context.SubTasks.SingleOrDefault(x => x.Id == subtaskId && x.UserId == user.Id);
            if (subtask == null)
            {
                return;
            }
            subtask.Description = dto.Description;
            _context.SubTasks.Update(subtask);
            _context.SaveChanges();
        }
        public void ToggleDone(int subtaskId)
        {
            var user = _userService.loggedUser();
            if (user == null)
            {
                return;
            }
            var subtask = _context.SubTasks.SingleOrDefault(x => x.Id == subtaskId && x.UserId == user.Id);
            if (subtask == null)
            {
                return;
            }
            subtask.isDone = !subtask.isDone;
            _context.SubTasks.Update(subtask);
            _context.SaveChanges();
        }
    }
}
