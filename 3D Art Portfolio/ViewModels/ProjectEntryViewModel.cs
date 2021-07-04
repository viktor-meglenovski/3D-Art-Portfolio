using _3D_Art_Portfolio.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _3D_Art_Portfolio.ViewModels
{
    public class ProjectEntryViewModel
    {
        [Required(ErrorMessage ="Please enter a Name for the new project!")]
        [MaxLength(160)]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please enter a Description for the new project!")]
        [MaxLength(160)]
        public string Description { get; set; }
        [Required(ErrorMessage = "Please select a Main Image for the new project!")]
        [Display(Name="Main Image")]
        public HttpPostedFileBase MainImage { get; set; }
        [Display(Name = "Other Images")]
        virtual public List<HttpPostedFileBase> ImageUrls { get; set; }
        public MultiSelectList AllSoftware { get; set; }
        [Display(Name="Used Software")]
        virtual public List<int> SoftwareUsedUrls { get; set; }
        public ProjectEntryViewModel()
        {
            this.AllSoftware = null;
            this.ImageUrls = new List<HttpPostedFileBase>();
            this.SoftwareUsedUrls = new List<int>();
        }
    }
}