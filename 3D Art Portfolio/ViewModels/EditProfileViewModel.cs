using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace _3D_Art_Portfolio.ViewModels
{
    public class EditProfileViewModel
    {
        public string UserId  { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public string UserName { get; set; }
        public string ProfilePicture { get; set; }
        public HttpPostedFileBase NewProfilePicture { get; set; }
        public EditProfileViewModel()
        {
            
        }
        public EditProfileViewModel(string UserId, string Name, string Surname, string UserName, string ProfilePicture)
        {
            this.UserId = UserId;
            this.Name = Name;
            this.Surname = Surname;
            this.UserName = UserName;
            this.ProfilePicture = ProfilePicture;
        }
    }
}