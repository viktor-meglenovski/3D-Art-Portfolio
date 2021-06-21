using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace _3D_Art_Portfolio.ViewModels
{
    public class AddSoftwareViewModel
    { 
        [Required]
        public string Name { get; set; }
        [Required]
        public HttpPostedFileBase ImageUrl { get; set; }
        public AddSoftwareViewModel(){}
        public AddSoftwareViewModel(string Name, HttpPostedFileBase ImageUrl)
        {
            this.Name = Name;
            this.ImageUrl = ImageUrl;
        }
    }
}