using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDo.Entities;

namespace ToDo.Models
{
    public class ProjectDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public Color Color { get; set; }
        public DateTime? DeadLine { get; set; }
        public bool IsDone { get; set; } = false;
        public ICollection<TaskDto>? Tasks { get; set; } = new ObservableCollection<TaskDto>();
        public string DoneRatio { get; set; }
    }
}
