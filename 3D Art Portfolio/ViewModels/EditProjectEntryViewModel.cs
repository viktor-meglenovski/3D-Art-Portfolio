using _3D_Art_Portfolio.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace _3D_Art_Portfolio.ViewModels
{
    public class EditProjectEntryViewModel
    {
        public int ProjectId { get; set; }
        [Required]
        [MaxLength(160)]
        public string Name { get; set; }
        [Required]
        [MaxLength(160)]
        public string Description { get; set; }
        public string MainImage { get; set; }
        public HttpPostedFileBase NewMainImage { get; set; }
        public List<Image> Images { get; set; }
        public List<HttpPostedFileBase> NewImageUrls { get; set; }
        public EditProjectEntryViewModel()
        {
            this.Images = new List<Image>();
            this.NewImageUrls = new List<HttpPostedFileBase>();
        }
        public EditProjectEntryViewModel(int ProjectId, string Name, string Description, string MainImage, List<Image> Images)
        {
            this.ProjectId = ProjectId;
            this.Name = Name;
            this.Description = Description;
            this.MainImage = MainImage;
            this.Images = Images;
            this.NewImageUrls = new List<HttpPostedFileBase>();
        }
    }
}