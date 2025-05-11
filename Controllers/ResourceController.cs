using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using JeddahGreenHub.Data;
using JeddahGreenHub.Models;
using Microsoft.AspNetCore.Authorization;

public class ResourcesController : Controller
{
    private readonly ApplicationDbContext _context;

    public ResourcesController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: /Resource/Index
    public IActionResult Index()
    {
        // Get the list of existing comments
        var comments = _context.Comments.OrderByDescending(c => c.DatePosted).ToList();

        // save the existing comments lists? in the ResourcesViewModel
        var viewModel = new ResourcesViewModel
        { Comments = comments };

        return View(viewModel); // Pass comments to the view


        //var comments = await _context.Comments
        //             .OrderByDescending(c => c.DatePosted) // Sort newest first
        //             .ToListAsync();

        //return View(comments);
    }




    // POST: /Resource/AddComment
    [Authorize]
    [HttpPost]
     // Ensure only logged-in users can add comments
    public async Task<IActionResult> AddComment(ResourcesViewModel model)
    {
        if (string.IsNullOrWhiteSpace(model.CommentContent) || model.CommentContent.Length < 6)
        {
            ModelState.AddModelError("commentContent", "Comment must be at least 6 characters long.");
            model.Comments = _context.Comments.OrderByDescending(c => c.DatePosted).ToList();
            return View("Index", model); // Return with the comments if the comment is too short
        }

        // Create a new comment
        var comment = new Comment
        {
            UserName = User.Identity.Name, // Use logged-in user's name
            Content = model.CommentContent,
            DatePosted = DateTime.Now
        };

        _context.Comments.Add(comment);
        await _context.SaveChangesAsync();
        TempData["SuccessMessage"] = "Comment Posted Successfully ✅";

        return RedirectToAction("Index"); // Reload the page with the new comment
    }






    // GET: /Resource/EditComment
    [HttpGet]
    public async Task<IActionResult> EditComment(int id)
    {
        // Fetch the comment by its ID
        var comment = await _context.Comments.FindAsync(id);

        // If the comment doesn't exist, return a 404 error
        if (comment == null)
        {
            return NotFound();
        }

        if (comment.UserName != User.Identity.Name && User.Identity.Name != "admin@kau.sa")
        {
            return Forbid(); // Prevent unauthorized deletion
        }

        // Create a ViewModel to hold the comment and the list of existing comments
        var viewModel = new ResourcesViewModel
        {
            CommentContent = comment.Content, // Pre-fill the textarea with current comment
            Comments = _context.Comments.OrderByDescending(c => c.DatePosted).ToList(), // Fetch all comments in descending order
        };

        // Pass the comment ID to the view to identify which comment is being edited
        ViewBag.CommentId = id;

        // Return the view with the ViewModel
        return View("Index", viewModel);
    }






    // POST: /Resource/SaveComment
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> SaveComment(int id, string updatedContent)
    {
        // Check if the new content is valid (at least 6 characters)
        if (string.IsNullOrWhiteSpace(updatedContent) || updatedContent.Length < 6)
        {
            ModelState.AddModelError("CommentContent", "Comment must be at least 6 characters long.");
            var viewModel = new ResourcesViewModel
            { Comments = _context.Comments.OrderByDescending(c => c.DatePosted).ToList() };

            // Retain the editing state
            ViewBag.CommentId = id;

            return View("Index", viewModel); // Reload the page with validation errors
        }

        // Find the comment in the database by ID
        var comment = await _context.Comments.FindAsync(id);
        if (comment == null)
        {
            return NotFound();
        }


        // ------------- Ensure the logged-in user is the one who wrote the comment --------
        if (comment.UserName != User.Identity.Name)
        {
            return Forbid(); 
        }


        // Update the content/comment and timestamp of the comment
        comment.DatePosted = DateTime.Now;
        comment.Content = updatedContent;
        await _context.SaveChangesAsync();
        TempData["SuccessMessage"] = "Comment Edited Successfully ✔✍";


        return RedirectToAction("Index");
    }






    // POST: /Resource/DeleteComment
    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteComment(int id)
    {
        var comment = await _context.Comments.FindAsync(id);

        if (comment == null)
        {
            return NotFound();
        }
        if (comment.UserName != User.Identity.Name && User.Identity.Name != "admin@kau.sa")
        {
            return Forbid(); // Prevent unauthorized deletion
        }
       

        _context.Comments.Remove(comment);
        await _context.SaveChangesAsync();
        // Set a success message
        TempData["SuccessMessage"] = "Comment Deleted Successfully 🗑";

        return RedirectToAction("Index");
    }
}

