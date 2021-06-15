using _3D_Art_Portfolio.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _3D_Art_Portfolio.ViewModels;
using System.IO;

namespace _3D_Art_Portfolio.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        protected ApplicationDbContext _context { get; set; }
        protected UserManager<ApplicationUser> _userManager;
        public ProfileController()
        {
            this._context = new ApplicationDbContext();
            this._userManager=new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(this._context));
        }
        
        public ActionResult Index()
        {
            var user = _userManager.FindById(User.Identity.GetUserId());
            var projectEntries = _context.ProjectEntries.Where(x => x.UserId == user.Id).ToList();
            var profileViewModel = new ProfileViewModel(user, projectEntries);
            return View(profileViewModel);
        }
        public ActionResult EditProfile()
        {
            var user = _userManager.FindById(User.Identity.GetUserId());
            var model = new EditProfileViewModel(user.Id,user.Name, user.Surname, user.UserName, user.ProfilePicture);
            return View(model);
        }
        [HttpPost]
        public ActionResult EditProfile(EditProfileViewModel model)
        {
            if(ModelState.IsValid)
            {
                var toUpdate = _context.Users.Find(model.UserId);
                if(toUpdate!=null)
                {
                    if(model.NewProfilePicture!=null)
                    {
                        string path = Path.Combine(Server.MapPath(@"~\UserUploads"), model.UserId, Path.GetFileName(model.NewProfilePicture.FileName));
                        model.NewProfilePicture.SaveAs(path);
                        toUpdate.ProfilePicture = Path.Combine(@"~\UserUploads", model.UserId, Path.GetFileName(model.NewProfilePicture.FileName));
                    }
                    toUpdate.Name = model.Name;
                    toUpdate.Surname = model.Surname;
                    toUpdate.UserName = model.UserName;
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }
    }
}