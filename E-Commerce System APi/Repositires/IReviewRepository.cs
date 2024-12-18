using E_Commerce_System_APi.Models;

namespace E_Commerce_System_APi.Repositires
{
    public interface IReviewRepository
    {
        void AddReview(Review review);
        Review GetById(int reviewId);
        Review GetReviewByUserAndProduct(int userId, int productId);
        IEnumerable<Review> GetReviewsByProductId(int productId);
        void UpdateReview(Review review);
    }
}