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
        [Required]
        [MaxLength(160)]
        public string Name { get; set; }
        [Required]
        [MaxLength(160)]
        public string Description { get; set; }
        [Required]
        public HttpPostedFileBase MainImage { get; set; }
        virtual public List<HttpPostedFileBase> ImageUrls { get; set; }
        public MultiSelectList AllSoftware { get; set; }
        virtual public List<int> SoftwareUsedUrls { get; set; }
        public ProjectEntryViewModel()
        {
            this.AllSoftware = null;
            this.ImageUrls = new List<HttpPostedFileBase>();
            this.SoftwareUsedUrls = new List<int>();
        }
    }
}