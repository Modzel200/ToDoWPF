using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDo.Entities;

namespace ToDo.Services
{
    public class ProjectService
    {
        private readonly ToDoDbContext _context;
        private readonly UserService _userService;
        public ProjectService(ToDoDbContext context, UserService userService)
        {
            this._context = context;
            this._userService = userService;
        }
        public IEnumerable<Project> GetAllProjects()
        {
            var user = _userService.loggedUser();
            if (user == null)
            {
                return null;
            }
            var projects = _context.Projects.Include(x => x.Tasks).Where(x => x.UserId == user.Id).ToList();
            return projects;
        }
        public Project GetProject()
    }
}
