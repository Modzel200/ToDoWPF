using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDo.Entities;

namespace ToDo.Models
{
    public class NotificationDto
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public bool isProjectNotification { get; set; }
    }
}
