using System.ComponentModel.DataAnnotations.Schema;
using Humanizer;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace JeddahGreenHub.Models
{
    public class Article
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string? ImagePath { get; set; } // Store file path in the database
        public string Description { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public DateTime? EditedDate { get; set; } // when the file is edited file cam be set in controller - only when edited hence the "?" after data type-


        // [NotMapped] ensures the IFormFile is not saved to the database.
        // Instead, save the file to the file system and store its path in the database.

        [NotMapped]
        public IFormFile? Image { get; set; } // For uploading the image

        public Article()
        {
            
        }
        public Article(int id, string title, string description)
        {
            ID = id;    
            Title = title;
            Description = description;
            
        }
    }
}
