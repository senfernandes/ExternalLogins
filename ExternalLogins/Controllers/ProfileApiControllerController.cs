using ExternalLogins.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data.Common;
using System.Threading.Tasks;

namespace ExternalLogins.Controllers
{
    [Route("api/[controller]/")]
    public class ProfileApiController : Controller
    {
        private readonly ProfileContext _context;
        public ProfileApiController(ProfileContext context)
        {
            _context = context;
        }

        // Create Individual
        [HttpPost]
        public async Task<IActionResult> CreateIndividual(Individual model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(model);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", "LoggedIn");

                }
            }
            catch (DbException ex)
            {
                ModelState.AddModelError(ex.ToString(), "Unable to Save changes. Please contact the system administrator.");

            }

            return RedirectToAction("Index", "LoggedIn", model);


        }

        // Edit Individual
        [HttpPost]
        public async Task<IActionResult> EditIndividual(Guid id, Individual model)
        {
            if (id != model.IndividualId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(model);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", "LoggedIn");

                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError(ex.ToString(), "Unable to edit changes. Please contact the system administrator.");


                }


            }

            return RedirectToAction("Index", "LoggedIn", model);
        }

        // Detail Individual
        [HttpGet]
        public async Task<IActionResult> DetailIndividual(Guid? id)
        {
            if (id == null)
            {

                return NotFound();
            }

            var model = await _context.Individuals.SingleOrDefaultAsync(x => x.IndividualId == id);

            if (model == null)
            {

                return NotFound();
            }

            return PartialView(model);

        }
    }
}
