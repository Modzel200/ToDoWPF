using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ToDo.Entities;
using ToDo.Models;

namespace ToDo.Services
{
    public class UserService
    {
        private readonly ToDoDbContext _context;
        public UserService(ToDoDbContext context)
        {
            this._context = context;
        }
        public bool addUser(string login, int pin)
        {
            if (_context.Users.SingleOrDefault(x => x.Username == login)!=null)
            {
                return false;
            }
            User user = new User();
            user.Username = login;
            user.Pin = pin;
            _context.Users.Add(user);
            _context.SaveChanges();
            return true;
        }

        public bool editUser(string login, int pin)
        {
            var user = _context.Users.SingleOrDefault(u => u.Id == _context.LoggedUsers.SingleOrDefault().Id);
            if (user is null || _context.Users.Any(x => x.Username == login))
            {
                return false;
            }
            
            user.Username = login;
            user.Pin = pin;
            _context.Users.Update(user);
            _context.SaveChanges();
            return true;
        }

        public GetUserDto GetUser()
        {
            var user = _context.Users.SingleOrDefault(u => u.Id == _context.LoggedUsers.SingleOrDefault().Id);
            if (user is null)
            {
                return new GetUserDto(){ };
            }

            return new GetUserDto()
            {
                Id = user.Id,
                Username = user.Username,
                Pin = user.Pin,
            };
        }

        public bool loginUser(string login, int pin)
        {
            var user = _context.Users.SingleOrDefault(a=>a.Username == login && a.Pin == pin);
            if(user!=null)
            {
                var loggedUser = _context.LoggedUsers.SingleOrDefault();
                if(loggedUser == null)
                {
                    loggedUser = new LoggedUser()
                    {
                        Id = user.Id,
                        Username = user.Username,
                    };
                    _context.LoggedUsers.Add(loggedUser);
                } else
                {
                    loggedUser.Id = user.Id;
                    loggedUser.Username = user.Username;
                    _context.LoggedUsers.Update(loggedUser);
                }
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
            _context.LoggedUsers.Remove(_context.LoggedUsers.SingleOrDefault());
            _context.SaveChanges();
        }
        public User loggedUser()
        {
            var user = _context.LoggedUsers.SingleOrDefault();
            if(user is null)
            {
                return null;
            }
            var result = _context.Users.SingleOrDefault(x => x.Id == user.Id);
            if (result == null)
            {
                return null;
            }
            return result;
        }
    }
}
