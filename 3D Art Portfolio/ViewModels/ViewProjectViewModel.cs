using _3D_Art_Portfolio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _3D_Art_Portfolio.ViewModels
{
    public class ViewProjectViewModel
    {
        public ProjectEntry ProjectEntry { get; set; }
        public bool IsLiked { get; set; }
        public ViewProjectViewModel(ProjectEntry ProjectEntry, bool IsLiked)
        {
            this.ProjectEntry = ProjectEntry;
            this.IsLiked = IsLiked;
        }
    }
}