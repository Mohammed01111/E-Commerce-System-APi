using E_Commerce_System_APi.DTO;
using E_Commerce_System_APi.Models;
using E_Commerce_System_APi.Repositires;

namespace E_Commerce_System_APi.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepo;
        private readonly IProductRepository _productRepo;

        public ReviewService(IReviewRepository reviewRepo, IProductRepository productRepo)
        {
            _reviewRepo = reviewRepo;
            _productRepo = productRepo;
        }

        public Review AddReview(ReviewDto model, int userId)
        {
            // Check if product is purchased by user and review is not already given
            var existingReview = _reviewRepo.GetReviewByUserAndProduct(userId, model.ProductId);
            if (existingReview != null)
                throw new Exception("You have already reviewed this product.");

            var review = new Review
            {
                UserId = userId,
                ProductId = model.ProductId,
                Rating = model.Rating,
                Comment = model.Comment,
                ReviewDate = DateTime.UtcNow
            };

            _reviewRepo.AddReview(review);

            // Recalculate product's overall rating
            var product = _productRepo.GetById(model.ProductId);
            product.OverallRating = (decimal)_reviewRepo.GetReviewsByProductId(model.ProductId)
                .Average(r => r.Rating);
            _productRepo.UpdateProduct(product);

            return review;
        }

        public Review UpdateReview(int reviewId, ReviewDto model, int userId)
        {
            // Retrieve the existing review
            var existingReview = _reviewRepo.GetById(reviewId);
            if (existingReview == null)
                throw new Exception("Review not found.");

            // Ensure that the review belongs to the user
            if (existingReview.UserId != userId)
                throw new Exception("You are not authorized to update this review.");

            // Update the review details
            existingReview.Rating = model.Rating;
            existingReview.Comment = model.Comment;
            existingReview.ReviewDate = DateTime.UtcNow; // Optionally update the review date

            _reviewRepo.UpdateReview(existingReview);

            // Recalculate the product's overall rating
            var product = _productRepo.GetById(existingReview.ProductId);
            product.OverallRating = (decimal)_reviewRepo.GetReviewsByProductId(existingReview.ProductId)
                .Average(r => r.Rating);
            _productRepo.UpdateProduct(product);

            return existingReview;
        }
    }
}
