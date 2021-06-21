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
            if (User.IsInRole("Administrator"))
                return RedirectToAction("AdministratorPanel", "Administrator");
            var user = _userManager.FindById(User.Identity.GetUserId());
            var projectEntries = _context.ProjectEntries.Where(x => x.UserId == user.Id).ToList();
            var profileViewModel = new ProfileViewModel(user, projectEntries);
            return View(profileViewModel);
        }
        public ActionResult EditProfile()
        {
            //nema potreba od restrikcija za koj e korisnikot, vo sekoj slucaj ke se editira negoviot profil
            var user = _userManager.FindById(User.Identity.GetUserId());
            var model = new EditProfileViewModel(user.Id,user.Name, user.Surname, user.UserName, user.ProfilePicture);
            return View(model);
        }
        [HttpPost]
        public ActionResult EditProfile(EditProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                var toUpdate = _context.Users.Find(model.UserId);
                if (toUpdate != null)
                {
                    //tuka proverka za slucajno nekoj drug da ne proba da editni nesto
                    if (toUpdate.Id != model.UserId)
                        return Content("no permissions");

                    toUpdate.Name = model.Name;
                    toUpdate.Surname = model.Surname;
                    toUpdate.UserName = model.UserName;

                    //ako ima nova profilna slika sega da se zacuva i vo soodvetniot folder na korisnikot i da se apdejtira bazata
                    if (Request.Files.Count != 0)
                    {
                        var file = Request.Files[0];
                        var fileName = Path.GetFileName(file.FileName);
                        if (fileName != "")
                        {
                            var path = Path.Combine(Server.MapPath("~/UserUploads"), toUpdate.Id, fileName);
                            file.SaveAs(path);
                            toUpdate.ProfilePicture = Path.Combine(@"/UserUploads", toUpdate.Id,fileName);
                        }
                    }
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
                return new HttpNotFoundResult();
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult ProfileImageUpload()
        {
            //slikata so ova se zacuvuva vo temp folder a potoa ako se klikni na Save se zacuvuva i kaj modelot.
            if (Request.Files.Count != 0)
            {
                var file = Request.Files[0];
                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath("~/UserUploads/Temp"), fileName);
                file.SaveAs(path);
                var newPath= Path.Combine(@"/UserUploads/Temp",Path.GetFileName(fileName));
                return Json(new { success = true, newImagePath = newPath }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Search(string text)
        {
            var model = _context.Users.Where(x => x.UserName.ToLower().Contains(text.ToLower())).ToList();
            return PartialView(model);
        }
        public ActionResult ViewProfile(string id)
        {
            if (User.Identity.GetUserId() == id)
                return RedirectToAction("Index");
            var user = _context.Users.Find(id);
            var projectEntries = _context.ProjectEntries.Where(x => x.UserId == user.Id).ToList();
            var profileViewModel = new ProfileViewModel(user, projectEntries);
            return View(profileViewModel);
        }
        
    }
}