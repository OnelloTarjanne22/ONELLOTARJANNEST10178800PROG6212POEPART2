﻿using Microsoft.AspNetCore.Mvc;
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
        //Submit claim method
        [HttpPost]
        public async Task<IActionResult> SubmitClaim(Claim claim, IFormFile uploadedFile)
        {
            // Validation of  numeric values to ensure they are not negative
            if (claim.LecturerId < 0)
            {
                ModelState.AddModelError("LecturerId", "Lecturer ID cannot be negative.");
            }

            if (claim.Rate < 0)
            {
                ModelState.AddModelError("Rate", "Rate cannot be negative.");
            }

            if (claim.Hours < 0)
            {
                ModelState.AddModelError("Hours", "Hours worked cannot be negative.");
            }

           
            if (!ModelState.IsValid)
            {
                return View("post", claim);
            }

            
            if (uploadedFile == null || uploadedFile.Length == 0)
            {
                ModelState.AddModelError("UploadedFilePath", "File is required.");
                return View("post", claim); 
            }

          
            var uploadsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
            var filePath = Path.Combine(uploadsDirectory, Path.GetFileName(uploadedFile.FileName));

            
            if (System.IO.File.Exists(filePath))
            {
                ModelState.AddModelError("UploadedFilePath", "A file with this name already exists. Please rename your file.");
                return View("post", claim); 
            }

            
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await uploadedFile.CopyToAsync(stream);
            }

           
            claim.UploadedFilePath = filePath;

            // Save the claim to the database
            _context.Claims.Add(claim);
            await _context.SaveChangesAsync();

            // Redirect to the success page
            return RedirectToAction("ClaimSuccess", "Home");
        }

        ///Download file method
        [HttpGet]
        public IActionResult DownloadFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return NotFound(); 
            }

            var fileName = Path.GetFileName(filePath);
            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", fileName);

            if (!System.IO.File.Exists(fullPath))
            {
                return NotFound(); 
            }

            var fileBytes = System.IO.File.ReadAllBytes(fullPath);
            return File(fileBytes, "application/octet-stream", fileName);
        }
        //Approve the claim method
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
        //Reject claim method
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

        //Method to regard all claims as pending until the stautus is changed by admin
        public IActionResult Track()
        {
            var pendingClaims = _context.Claims.Where(c => c.ClaimStatus == "Pending").ToList();
            return View("Track", pendingClaims);
        }
    }
}
