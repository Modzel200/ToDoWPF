using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDo.Entities
{
    public enum Color
    {
        Blue,
        Green,
        Purple,
        Orange,
        Red,
        Yellow
    }
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public Color Color { get; set; }
        public DateTime? DeadLine { get; set; }
        public bool IsDone { get; set; } = false;
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Task> Tasks { get; private set; } = new ObservableCollection<Task>();
    }
}
