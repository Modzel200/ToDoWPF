using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDo.Entities
{
    public class Notification
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public bool isRead { get; set; } = false;
        public int? ProjectId { get; set; }
        public int? TaskId { get; set; }
        public virtual Project? Project { get; set; }
        public virtual Task? Task { get; set; }
        public int UserId {  get; set; }
    }
}
