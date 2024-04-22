using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDo.Models
{
    public class SubTaskDto
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public bool isDone { get; set; } = false;
    }
}
