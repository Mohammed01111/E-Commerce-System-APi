using E_Commerce_System_APi.Models;
using System;

namespace E_Commerce_System_APi.Repositires
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly ApplicationDbContext _context;
        public ReviewRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void AddReview(Review review)
        {
            _context.Reviews.Add(review);
            _context.SaveChanges();
        }

        public Review GetById(int reviewId)
        {
            return _context.Reviews.FirstOrDefault(r => r.RID == reviewId);
        }

        public Review GetReviewByUserAndProduct(int userId, int productId)
        {
            return _context.Reviews.SingleOrDefault(r => r.UserId == userId && r.ProductId == productId);
        }

        public IEnumerable<Review> GetReviewsByProductId(int productId)
        {
            return _context.Reviews
                .Where(r => r.ProductId == productId)
                .ToList();
        }

        // Update an existing review
        public void UpdateReview(Review review)
        {
            _context.Reviews.Update(review);
            _context.SaveChanges();
        }
    }
}
