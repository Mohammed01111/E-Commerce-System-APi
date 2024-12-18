
using System;
using E_Commerce_System_APi.Models;

namespace E_Commerce_System_APi.Repositires
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;


        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public User GetById(int id)
        {

            return _context.Users.SingleOrDefault(u => u.UID == id);

        }


        public void UpdateUser(User user)
        {

            _context.Users.Update(user);


            _context.SaveChanges();
        }


        public User GetByEmail(string email)
        {
            return _context.Users.SingleOrDefault(u => u.Email == email);
        }


        public void AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }
    }
}
