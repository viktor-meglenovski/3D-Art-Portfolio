using _3D_Art_Portfolio.Models;
using _3D_Art_Portfolio.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _3D_Art_Portfolio.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdministratorController : Controller
    {
        protected ApplicationDbContext _context { get; set; }
        protected UserManager<ApplicationUser> _userManager;
        public AdministratorController()
        {
            this._context = new ApplicationDbContext();
            this._userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(this._context));

        }
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        public ActionResult AdministratorPanel()
        {
            return View();
        }
        public ActionResult CleanTemp()
        {
            var path = Server.MapPath("~/UserUploads/Temp");
            Directory.Delete(path, true);
            Directory.CreateDirectory(path);
            return View("AdministratorPanel");
        }
        public ActionResult ManageUsers()
        {
            //var users = _context.Roles.Where(x => x.Name == "User").Select(x => x.Users).ToList();
            var users = _context.Users.ToList();//ova ke go koristam za da gi izbrisam starite akaunti
            var model = new List<ApplicationUser>();
            foreach (ApplicationUser a in users)
                if (!_userManager.IsInRole(a.Id, "Administrator"))
                    model.Add(a);
            return View(model);
        }
        public ActionResult DeleteUser(string id)
        {
            if (_userManager.IsInRole(id, "Administrator"))
                return Content("You cannot delete administrator accounts");
            var user = _context.Users.Find(id);
            //se povikuva brisenje na site proekti ne korisnikot (so toa se brisat stavkite od Images, Likes(za konkretniot proekt) i ProjectEntries)
            var projects = _context.ProjectEntries.Where(x => x.UserId == user.Id).ToList();
                foreach (ProjectEntry p in projects)
                    DeleteProject(p);
            //se brisat od baza site stavki za koi ovaj user ima staveno like
            var userLikes = _context.Likes.Where(x => x.UserId == user.Id);
            _context.Likes.RemoveRange(userLikes);
            //ja gradime patekata na root folderot na korisnikot
            var path = Path.Combine(Server.MapPath("~/UserUploads/"), user.Id);
            _userManager.RemoveFromRole(user.Id, "User");
            _context.Users.Remove(user);
            _context.SaveChanges();
            Directory.Delete(path, true);
            return RedirectToAction("ManageUsers");
        }
        public void DeleteProject(ProjectEntry project)
        {
            if (User.Identity.GetUserId() == project.UserId || User.IsInRole("Administrator"))
            {
                var id = project.ProjectId;
                var userId = project.UserId;
                var projectFolder = Path.Combine(Server.MapPath("~/UserUploads/"), userId, id.ToString());
                var images = _context.Images.Where(x => x.ProjectId == id);
                var likes = _context.Likes.Where(x => x.ProjectId == id);
                _context.Images.RemoveRange(images);
                _context.Likes.RemoveRange(likes);
                _context.ProjectEntries.Remove(project);
                _context.SaveChanges();
            }
        }

        //add i delete na novi softveri ova go pravi samo administratorot
        public ActionResult ShowAllSoftware()
        {
            var model=_context.Softwares.ToList();
            return View(model);
        }
        public ActionResult AddSoftware()
        {
            var model = new AddSoftwareViewModel();
            return View(model);
        }
        [HttpPost]
        public ActionResult AddSoftware(AddSoftwareViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var filename = Path.GetFileName(model.ImageUrl.FileName);
            var path = Path.Combine(Server.MapPath("~/Content/Images/Software/"), filename);
            model.ImageUrl.SaveAs(path);
            _context.Softwares.Add(new Software(model.Name, Path.Combine(@"~\Content\Images\Software\", filename)));
            _context.SaveChanges();
            return RedirectToAction("ShowAllSoftware");
        }
        public ActionResult DeleteSoftware(int id)
        {
            var toDelete = _context.Softwares.Find(id);
            if (toDelete == null)
                return new HttpNotFoundResult();
            var pathToDelete = Path.Combine(Server.MapPath(toDelete.ImageUrl));
            System.IO.File.Delete(pathToDelete);
            _context.Softwares.Remove(toDelete);
            _context.SaveChanges();
            return RedirectToAction("ShowAllSoftware");
        }
    }
}