namespace JeddahGreenHub.Models
{
    public class Comment
    {

        public int Id { get; set; }
        public string UserName { get; set; } // Will store the logged-in user's email
        public string Content { get; set; }
        public DateTime DatePosted { get; set; }

        public Comment()
        {
            
        }

    }

    
}
