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
        private readonly ToDoDbContext _context;
        public UserService(ToDoDbContext context)
        {
            this._context = context;
        }
        public void addUser(string login, int pin)
        {
            User user = new User();
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
