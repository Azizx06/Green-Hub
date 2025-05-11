using System.ComponentModel.DataAnnotations.Schema;

namespace JeddahGreenHub.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; } // to view current date and time WHIILE selecting the date and time when creating
        public string? ImagePath { get; set; } // Optional, image path
        [NotMapped]
        public IFormFile? Image { get; set; } // Optional, For uploading the image

        public Event()
        {
            
        }
    }


}
