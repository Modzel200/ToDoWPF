using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDo.Entities
{
    public enum Type
    {
        Project,
        Task
    }
    public class Notification
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public Type Type { get; set; }


        public int? ProjectId { get; set; }
        public int? TaskId { get; set; }


        public Project? Project { get; set; }
        public Task? Task { get; set; }
    }
}
