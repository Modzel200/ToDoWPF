using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDo.Entities;

namespace ToDo.Models
{
    public class ProjectFilterSortDto
    {
        public List<Color> Colors { get; set; } = new List<Color>();
        public bool? IsDone { get; set; } = null;
        public string Search { get; set; } = "";
        public string SortBy { get; set; } = "Id";
        public SortDirection SortDirection { get; set; } = SortDirection.ASC;
    }
}
