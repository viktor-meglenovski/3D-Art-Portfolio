using _3D_Art_Portfolio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _3D_Art_Portfolio.ViewModels
{
    public class ProfileViewModel
    {
        public ApplicationUser applicationUser { get; set; }
        public List<ProjectEntry> projectEntries { get; set; }
        public int TotalLikes { get; set; }
        public ProfileViewModel()
        {
            this.projectEntries = new List<ProjectEntry>();
        }
        public ProfileViewModel(ApplicationUser applicationUser, List<ProjectEntry> projectEntries, int TotalLikes)
        {
            this.applicationUser = applicationUser;
            this.projectEntries = projectEntries;
            this.TotalLikes = TotalLikes;
        }
    }
}