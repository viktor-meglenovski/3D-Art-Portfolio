using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace _3D_Art_Portfolio.ViewModels
{
    public class ProjectEntryViewModel
    {
        [Required]
        [MaxLength(160)]
        public string Name { get; set; }
        [Required]
        [MaxLength(160)]
        public string Description { get; set; }
        [Required]
        public HttpPostedFileBase MainImage { get; set; }
        virtual public List<HttpPostedFileBase> ImageUrls { get; set; }
        virtual public List<String> SoftwareUsedUrls { get; set; }
        public ProjectEntryViewModel()
        {
            this.ImageUrls = new List<HttpPostedFileBase>();
            this.SoftwareUsedUrls = new List<String>();
        }
    }
}