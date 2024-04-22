using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDo.Entities
{
    public class SubTask
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public bool isDone { get; set; } = false;
        public int TaskId { get; set; }
        public virtual Task Task { get; set; }
    }
}
