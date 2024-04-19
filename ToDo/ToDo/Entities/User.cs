using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDo.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public int Pin { get; set; }
        public virtual ICollection<Project> Projects { get; private set; } = new ObservableCollection<Project>();
    }
}
