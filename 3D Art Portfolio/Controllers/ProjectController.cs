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
    public class ProjectController : Controller
    {
        protected ApplicationDbContext _context { get; set; }
        protected UserManager<ApplicationUser> _userManager;
        public ProjectController()
        {
            this._context = new ApplicationDbContext();
            this._userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(this._context));
        }
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AddNewProject()
        {
            var model = new ProjectEntryViewModel();
            return View(model);
        }
        [HttpPost]
        public ActionResult AddNewProject(ProjectEntryViewModel model)
        {
            if(ModelState.IsValid)
            {
                ProjectEntry temp = new ProjectEntry() { 
                UserId= User.Identity.GetUserId(),
                Name=model.Name,
                Description=model.Description,
                };
                _context.ProjectEntries.Add(temp);
                _context.SaveChanges();
                //prvo go pravime folderot vrz osnova na ID to dodeleno vo bazata
                var path = Server.MapPath("~/UserUploads/" + User.Identity.GetUserId() + "/" + temp.ProjectId);
                Directory.CreateDirectory(path);
                //a potoa gi zacuvuvame site sliki vo soodvetniot folder
                var image = Path.GetFileName(model.MainImage.FileName);
                path = Path.Combine(Server.MapPath("~/UserUploads/"), User.Identity.GetUserId(), temp.ProjectId.ToString(),image);
                model.MainImage.SaveAs(path);
                temp.MainImage= Path.Combine("~/UserUploads/", User.Identity.GetUserId(), temp.ProjectId.ToString(), image);
                //i site ostanati sliki
                foreach (HttpPostedFileBase img in model.ImageUrls)
                {
                    image=Path.GetFileName(img.FileName);
                    path = Path.Combine(Server.MapPath("~/UserUploads/"), User.Identity.GetUserId(), temp.ProjectId.ToString(), image);
                    img.SaveAs(path);
                    _context.Images.Add(new Image(temp.ProjectId, Path.Combine("~/UserUploads/", User.Identity.GetUserId(), temp.ProjectId.ToString(), image)));
                }
                _context.SaveChanges();
                return RedirectToAction("ViewProject/"+ temp.ProjectId);
            }
            return View(model);
        }
        public ActionResult ViewProject(int id)
        {
            var model = _context.ProjectEntries.Find(id);
            if (model == null)
                return new HttpNotFoundResult();
            model.ImageUrls = _context.Images.Where(x => x.ProjectId == id).ToList();
            return View(model);
        }
        public ActionResult Delete(int id)
        {
            var project = _context.ProjectEntries.Find(id);
            //if(User.Identity.GetUserId()!=project.UserId) nekoj drug korisnik probuva da izbrisi, ova da ne se dozvoli i vo drugi funkcii - potocno edit za drugi ne znnam dali ke treba
            if (project == null)
                return new HttpNotFoundResult();
            var userId = project.UserId;
            var projectFolder = Path.Combine(Server.MapPath("~/UserUploads/"), userId, id.ToString());
            var images = _context.Images.Where(x => x.ProjectId == id);
            _context.Images.RemoveRange(images);
            _context.ProjectEntries.Remove(project);
            _context.SaveChanges();
            Directory.Delete(projectFolder,true);
            return RedirectToAction("Index","Profile");
        }
    }
}