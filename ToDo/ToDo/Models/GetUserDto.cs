using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDo.Models
{
    public class GetUserDto
    {
        public int Id { get; set; }
        public string Username { get; set; }    
        public int Pin { get; set; }
    }
}
