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
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
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
                    toUpdate.Name = model.Name;
                    toUpdate.Surname = model.Surname;
                    toUpdate.UserName = model.UserName;
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult ProfileImageUpload()
        {
            if (Request.Files.Count != 0)
            {
                var file = Request.Files[0];
                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath("~/UserUploads/"), User.Identity.GetUserId(), fileName);
                file.SaveAs(path);
                var toUpdate = _context.Users.Find(User.Identity.GetUserId());
                var newPath= toUpdate.ProfilePicture = Path.Combine(@"/UserUploads", toUpdate.Id, Path.GetFileName(fileName));
                if (toUpdate!=null)
                {
                    toUpdate.ProfilePicture = newPath;
                    _context.SaveChanges();
                }
                return Json(new { success = true, newImagePath = newPath }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }
    }
}