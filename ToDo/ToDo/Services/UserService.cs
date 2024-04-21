using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDo.Entities;

namespace ToDo.Services
{
    public class UserService
    {
        private readonly ToDoDbContext _context = new ToDoDbContext();
        private User user = new User();
        public void loadDatabase()
        {
            _context.Database.EnsureCreated();
            _context.Users.Load();
            _context.Projects.Load();
            _context.Tasks.Load();
            _context.SubTasks.Load();
        }
        public void addUser(string login, int pin)
        {
            user.Username = login;
            user.Pin = pin;
            user.isLogged = false;
            _context.Users.Add(user);
            _context.SaveChanges();
        }
        public bool loginUser(string login, int pin)
        {
            var user = _context.Users.SingleOrDefault(a=>a.Username == login && a.Pin == pin);
            if(user!=null)
            {
                user.isLogged = true;
                _context.Users.Update(user);
                _context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public void logoutUser(User user)
        {
            user.isLogged = false;
            _context.Users.Update(user);
            _context.SaveChanges();
        }
        public User loggedUser()
        {
            var user = _context.Users.SingleOrDefault(a => a.isLogged == true);
            if (user!=null)
            {
                return user;
            }
            return null;
        }
    }
}
