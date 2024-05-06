using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDo.Entities;

namespace ToDo.Models
{

    public class TaskFilterSortDto
    {
        public Category? CategoryFilter { get; set; }
        public PriorityLevel? Sort { get; set; }
    }

}
