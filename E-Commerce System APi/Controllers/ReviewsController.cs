using E_Commerce_System_APi.DTO;
using E_Commerce_System_APi.Services;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_System_APi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewsController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpPost]
        public IActionResult AddReview([FromBody] ReviewDto model)
        {
            try
            {
                var review = _reviewService.AddReview(model, userId: 1); // Hardcoded userId for simplicity
                return Ok(review);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // UpdateReview method
        [HttpPut("{reviewId}")]
        public IActionResult UpdateReview(int reviewId, [FromBody] ReviewDto model)
        {
            try
            {
                // Assuming the service method will return the updated review or throw an exception if not found
                var updatedReview = _reviewService.UpdateReview(reviewId, model, userId: 1); // Hardcoded userId for simplicity
                return Ok(updatedReview);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

