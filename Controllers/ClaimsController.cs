using Microsoft.AspNetCore.Mvc;
using ONELLOTARJANNEST10178800PROG6212POEPART2.Data;
using ONELLOTARJANNEST10178800PROG6212POEPART2.Models;
using System.Threading.Tasks;

namespace ONELLOTARJANNEST10178800PROG6212POEPART2.Controllers
{
    public class ClaimsController : Controller
    {
        private readonly AddDbContext _context;

        public ClaimsController(AddDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> SubmitClaim(Claim claim, IFormFile uploadedFile)
        {
            // Validate file
            if (uploadedFile == null || uploadedFile.Length == 0)
            {
                ModelState.AddModelError("UploadedFilePath", "File is required.");
                return View("post", claim); // Re-render the form with validation errors
            }

            // Set the file path where the file will be saved
            var filePath = Path.Combine("wwwroot/uploads", Path.GetFileName(uploadedFile.FileName));

            // Upload the file
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await uploadedFile.CopyToAsync(stream);
            }

            // Save the file path in the claim model
            claim.UploadedFilePath = filePath;

            // Save the claim to the database
            _context.Claims.Add(claim);
            await _context.SaveChangesAsync();

            // Redirect to the success page
            return RedirectToAction("ClaimSuccess", "Home");
        }
        [HttpGet]
        public IActionResult DownloadFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return NotFound(); // Return 404 if the file path is null or empty
            }

            var fileName = Path.GetFileName(filePath);
            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", fileName);

            if (!System.IO.File.Exists(fullPath))
            {
                return NotFound(); // Return 404 if the file does not exist
            }

            var fileBytes = System.IO.File.ReadAllBytes(fullPath);
            return File(fileBytes, "application/octet-stream", fileName);
        }

        [HttpPost]
        public async Task<IActionResult> ApproveClaim(int claimId)
        {
            var claim = _context.Claims.FirstOrDefault(c => c.ClaimId == claimId);
            if (claim != null)
            {
                claim.ClaimStatus = "Approved";
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Track");
        }

        [HttpPost]
        public async Task<IActionResult> RejectClaim(int claimId)
        {
            var claim = _context.Claims.FirstOrDefault(c => c.ClaimId == claimId);
            if (claim != null)
            {
                claim.ClaimStatus = "Rejected";
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Track");
        }
        public IActionResult ClaimView()
        {
            var claims = _context.Claims.ToList(); 
            return View("ClaimView", claims); 
        }


        public IActionResult Track()
        {
            var pendingClaims = _context.Claims.Where(c => c.ClaimStatus == "Pending").ToList();
            return View("Track", pendingClaims);
        }
    }
}
