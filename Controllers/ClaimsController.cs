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
        public async Task<IActionResult> SubmitClaim(Claim claim)
        {
            if (ModelState.IsValid)
            {
               
                _context.Claims.Add(claim);
                await _context.SaveChangesAsync();

                return RedirectToAction("ClaimSuccess", "Home");
            }

          
            return View(claim); 
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
