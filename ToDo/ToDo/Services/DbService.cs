using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDo.Entities;

namespace ToDo.Services
{
    public class DbService
    {
        private readonly ToDoDbContext _context = new ToDoDbContext();
        public DbService()
        {
            _context.Database.EnsureCreated();
            _context.Users.Load();
            _context.Projects.Load();
            _context.Tasks.Load();
            _context.SubTasks.Load();
        }
        public ToDoDbContext Context()
        {
            return this._context;
        }
    }
}
