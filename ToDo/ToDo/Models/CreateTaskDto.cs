using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDo.Entities;

namespace ToDo.Models
{
    public class CreateTaskDto
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public Category? Category { get; set; }
        public PriorityLevel? PriorityLevel { get; set; }
        public DateTime? DeadLine { get; set; }
    }
}
