namespace BugTracker.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Text { get; set; }

        public virtual User Author { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }

        [Required]
        public virtual Bug Bug { get; set; }
    }
}
