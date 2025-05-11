using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JeddahGreenHub.Data;
using JeddahGreenHub.Models;
using Microsoft.AspNetCore.Authorization;

namespace JeddahGreenHub.Controllers
{
    public class EventsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EventsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Events
        public async Task<IActionResult> Index()
        {
            return View(await _context.Event.ToListAsync());
        }

        // GET: Events/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Event
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }



        // ------------------------------------------ CREATE ---------------------------------------------
        // GET: Events/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Events/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Event newEvent)
        {
            if (ModelState.IsValid)
            {
                
                if (newEvent.Image != null) {

                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Events");
                    
                    var uniqueFileName =   "_Event_Created_" + DateTime.Now.ToString("_yyyy-MMM-dd__HH-mm-ss") + newEvent.Image.FileName;
                   
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }


                    // Save the file to the file system
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await newEvent.Image.CopyToAsync(fileStream);
                    }

                    // Save the file path to the database
                    newEvent.ImagePath = $"/images/Events/{uniqueFileName}";
               
                } else {
                    newEvent.ImagePath = null;
                    // a default placeholder image if no file is uploaded
                    //newEvent.ImagePath = "/images/Events/no-image.png";
                }


                _context.Add(newEvent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(newEvent);
        }















        // ------------------------------------------ EDIT ---------------------------------------------
        // GET: Events/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Event.FindAsync(id);
            if (@event == null)
            {
                return NotFound();
            }
            return View(@event);
        }




        // POST: Events/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind ("Id, Title, Description, Date, ImagePath, Image")] Event updatedEvent, bool DeleteImage) 
        {
            if (id != updatedEvent.Id)
            {
                return BadRequest();
            }


            if (ModelState.IsValid)
            {
                try
                {


                    // Handle Image UPLOAD ------------------------------
                    if (updatedEvent.Image != null)
                    {


                        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Events");
                        var uniqueFileName = DateTime.Now.ToString("yyyy_MMM_dd__-__HH_mm_ss_") + "_Edited_Events_" + updatedEvent.Image.FileName;
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        // Ensure the folder exists
                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await updatedEvent.Image.CopyToAsync(stream);
                        }
                        updatedEvent.ImagePath = $"/images/Events/{uniqueFileName}";
                    } 

                    _context.Update(updatedEvent);
                    await _context.SaveChangesAsync();
                }


                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(updatedEvent.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));

            }
            return View(updatedEvent);
        }



















        // ------------------------------------------ DELETE ---------------------------------------------
        // GET: Events/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Event
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // POST: Events/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @event = await _context.Event.FindAsync(id);
            if (@event != null)
            {
                _context.Event.Remove(@event);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventExists(int id)
        {
            return _context.Event.Any(e => e.Id == id);
        }
    }
}
