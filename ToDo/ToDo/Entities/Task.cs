using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDo.Entities
{
    public enum Category
    {
        Home,
        Work
    }
    public enum PriorityLevel
    {
        Not_Important,
        Important,
        Very_Important
    }
    public class Task
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public Category? Category { get; set; }
        public PriorityLevel? PriorityLevel { get; set; }
        public DateTime? DeadLine {get; set;}
        public bool IsDone { get; set; } = false;
        public int ProjectId { get; set; }
        public virtual Project Project { get; set; }
        public virtual ICollection<SubTask> SubTasks { get; set;} = new ObservableCollection<SubTask>();
        public int UserId { get; set; }
        public virtual User User { get; set; }

    }
}
