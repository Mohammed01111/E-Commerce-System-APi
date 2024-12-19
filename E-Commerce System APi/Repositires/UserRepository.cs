
using System;
using E_Commerce_System_APi.Models;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_System_APi.Repositires
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public User GetByEmail(string email)
        {
            return _context.Users.FirstOrDefault(u => u.Email == email);
        }

        public User GetById(int id)
        {
            return _context.Users.FirstOrDefault(u => u.UID == id);
        }

        public void AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }


        public User GetUser(string email, string password)
        {
            return _context.Users
                .Where(u => u.Email == email && u.Password == password)
                .FirstOrDefault();
        }
    }
}