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
            var notifications = _context.Notifications.Include(x => x.Task).Include(x => x.Project).Where(x => x.UserId == user.Id && x.isRead == false && ( (x.Task.DeadLine >= DateTime.Today && x.Task.DeadLine <= DateTime.Today.AddDays(3)) || (x.Project.DeadLine >= DateTime.Today && x.Project.DeadLine <= DateTime.Today.AddDays(7)))).Select(x => new NotificationDto()
            {
                Id = x.Id,
                Message = (x.Project == null) ? $"You have {((x.Task.DeadLine == null ? DateTime.Today : (DateTime)x.Task.DeadLine) - DateTime.Today).TotalDays} days to complete task: {x.Task.Name}" : $"You have {((x.Project.DeadLine == null ? DateTime.Today : (DateTime)x.Project.DeadLine) - DateTime.Today).TotalDays} days to complete project: {x.Project.Name}",
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
            var notification = _context.Notifications.SingleOrDefault(x => x.UserId == user.Id && x.isRead == false && x.Id == notificationId);
            if (notification == null)
            {
                return;
            }
            notification.isRead = true;
            _context.Notifications.Update(notification);
            _context.SaveChanges();
        }

    }
}
