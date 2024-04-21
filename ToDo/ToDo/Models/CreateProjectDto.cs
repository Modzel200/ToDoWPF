using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDo.Entities;

namespace ToDo.Models
{
    public class CreateProjectDto
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public Color Color { get; set; }
        public DateTime? DeadLine { get; set; }
    }
}
