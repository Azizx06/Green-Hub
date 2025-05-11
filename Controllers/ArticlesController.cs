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
    public class ArticlesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ArticlesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Articles
        public async Task<IActionResult> Index()
        {
            return _context.Article != null ?
                  View(await _context.Article.ToListAsync()) :
                  Problem("Entity set 'ApplicationDbContext.Article'  is null.");
        }

        // GET: Articles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _context.Article
                .FirstOrDefaultAsync(m => m.ID == id);
            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }


        // ------------------------------------------ CREATE ---------------------------------------------
        // GET: Articles/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Article article)
        {
            if (ModelState.IsValid)
            {
                // Handle image upload
                if (article.Image != null)
                {
                    // Set the folder for saving images
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Articles");

                    // Generate a unique file name for the uploaded image
                    var uniqueFileName = DateTime.Now.ToString("yyyy_MMM_dd__-__HH_mm_ss_") + "_ArticleMade_" + article.Image.FileName;

                    // Combine the folder and file name to get the full path
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);


                    // Ensure the folder exists, or create a new one if not. (Basically no need to create but just in case.)
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }


                    // Save the file to the file system
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await article.Image.CopyToAsync(fileStream);
                    }

                    // Save the file path to the database
                    article.ImagePath = $"/images/Articles/{uniqueFileName}";
                } else {
                    //// a default placeholder image if no file is uploaded
                    article.ImagePath = "/images/Articles/no-image.png";
                }

                // Set the current date and time when article was made
                article.CreationDate = DateTime.Now;

                // Save the article to the database
                _context.Add(article);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(article);
        }









        // ------------------------------------------ EDIT ---------------------------------------------
        // GET: Articles/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _context.Article.FindAsync(id);
            if (article == null)
            {
                return NotFound();
            }
            return View(article);
        }





        // POST: Articles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Title,ImagePath,Description,CreationDate")] Article article, IFormFile? Image, bool DeleteImage)
        {
            if (id != article.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    // Handle image REUPLOAD  ---------------------------------------------------
                    if (Image != null)
                    {
                        
                        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Articles");
                        // Easy Readable date when saving the New File Name.....
                        var uniqueFileName = DateTime.Now.ToString("yyyy_MMM_dd__-__HH_mm_ss_") + "_Edited_Article_" + Image.FileName;
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);


                        // Ensure the folder exists
                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }


                        // Save the new file
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await Image.CopyToAsync(fileStream);
                        }

                        // Update the image path
                        article.ImagePath = $"/images/Articles/{uniqueFileName}";
                    }


                    // Set the EditedDate to the current date and time (only when editing)
                    article.EditedDate = DateTime.Now;
                    _context.Update(article);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));


                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArticleExists(article.ID))
                    {
                        return NotFound();
                    }
                    else
                    {

                        throw;
                    }
                }

            }

            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }
            }
            return View(article);
        }












        // GET: Articles/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _context.Article
                .FirstOrDefaultAsync(m => m.ID == id);
            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }


        // POST: Articles/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var article = await _context.Article.FindAsync(id);
            if (article != null)
            {
                _context.Article.Remove(article);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArticleExists(int id)
        {
            return _context.Article.Any(e => e.ID == id);
        }





        // SEARCH FORM   --------------------------

        // GET: Articles/SearchForm
        public async Task<IActionResult> SearchForm()
        {
            return View();
        }

        // Edited; added 'If' function and edited 'Lambda' within to retreive search by Article Title, Description or both.
        
        // POST: Articles/ShowSearchFormResult
        public async Task<IActionResult> ShowSearchFormResult(String SearchArticle)
        {
            if (_context.Article == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Article' is null.");
            }

            var filteredArticles = await _context.Article 
                .Where(j => j.Title.Contains(SearchArticle) || j.Description.Contains(SearchArticle))
                .ToListAsync();
            return View("Index", filteredArticles);
        }



    }
}
