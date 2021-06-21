using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace _3D_Art_Portfolio.Models
{
    public class ProjectEntry
    {
        [Key]
        public int ProjectId { get; set; }
        public string UserId { get; set; }
        [Required]
        [MaxLength(160)]
        [Display(Name="Project Name")]
        public string Name { get; set; }
        [Required]
        [MaxLength(160)]
        [Display(Name = "Project Description")]
        public string Description { get; set; }
        public string MainImage { get; set; }
        public string TimeStamp { get; set; }
        virtual public List<Image> ImageUrls { get; set; }
        virtual public List<Software> SoftwareUsedUrls { get; set; }
        public int Likes { get; set; }
        public ProjectEntry()
        {
            this.ImageUrls = new List<Image>();
            this.SoftwareUsedUrls = new List<Software>();
            this.Likes = 0;
        }
        public ProjectEntry(string UserId, string Name, string Description, string MainImage, List<Image> ImageUrls, List<Software> SoftwareUsedUrls)
        {
            this.UserId = UserId;
            this.Name = Name;
            this.Description = Description;
            this.MainImage = MainImage;
            this.ImageUrls = ImageUrls;
            this.SoftwareUsedUrls = SoftwareUsedUrls;
            this.Likes = 0;
        }
        public void addLike()
        {
            Likes++;
        }
        public void removeLike()
        {
            Likes--;
        }
    }
}