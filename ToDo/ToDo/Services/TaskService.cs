using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDo.Entities;

namespace ToDo.Services
{
    public class TaskService
    {
        private readonly ToDoDbContext _context;
        private readonly UserService _userService;
        public TaskService(ToDoDbContext context, UserService userService)
        {
            this._context = context;
            this._userService = userService;
        }
    }
}
