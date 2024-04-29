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
    public class NotificationService
    {
        private readonly ToDoDbContext _context;
        private readonly UserService _userService;
        public NotificationService(ToDoDbContext context, UserService userService)
        {
            _context = context;
            _userService = userService;
        }
        public List<NotificationDto> GetAllNotifications()
        {
            var user = _userService.loggedUser();
            if (user == null)
            {
                return null;
            }
            var notifications = _context.Notifications.Include(x => x.Task).Include(x => x.Project).Where(x => x.UserId == user.Id && x.isRead == false).Select(x => new NotificationDto()
            {
                Id = x.Id,
                Message = (x.ProjectId == null) ? $"You have {(DateTime.Now - (x.Task.DeadLine == null ? DateTime.Now : (DateTime)x.Task.DeadLine)).TotalDays} to complete task: {x.Task.Name}" : $"You have {(DateTime.Now - (x.Project.DeadLine == null ? DateTime.Now : (DateTime)x.Project.DeadLine)).TotalDays} to complete project: {x.Project.Name}",
                isProjectNotification = (x.ProjectId == null) ? false : true
            }).ToList();
            return notifications;
        }
        public void MarkAsRead(int notificationId)
        {
            var user = _userService.loggedUser();
            if (user == null)
            {
                return;
            }
            var notification = _context.Notifications.SingleOrDefault(x => x.UserId == user.Id && x.isRead == false);
            notification.isRead = true;
            _context.Notifications.Update(notification);
            _context.SaveChanges();
        }

    }
}
