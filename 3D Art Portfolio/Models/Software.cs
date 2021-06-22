using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace _3D_Art_Portfolio.Models
{
    public class Software
    {
        [PrimaryKey]
        public int Id { get; set; }
        [Display(Name = "Software Name")]
        public string Name { get; set; }
        [Display(Name = "Logo")]
        public string ImageUrl { get; set; }
        public Software() { }
        public Software(string Name, string ImageUrl)
        {
            this.Name = Name;
            this.ImageUrl = ImageUrl;
        }
        public override string ToString()
        {
            return Id.ToString();
        }
    }
}