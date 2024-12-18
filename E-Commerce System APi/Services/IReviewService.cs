using E_Commerce_System_APi.DTO;
using E_Commerce_System_APi.Models;

namespace E_Commerce_System_APi.Services
{
    public interface IReviewService
    {
        Review AddReview(ReviewDto model, int userId);
        Review UpdateReview(int reviewId, ReviewDto model, int userId);
    }
}