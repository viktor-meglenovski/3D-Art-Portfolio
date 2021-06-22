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
            model.AllSoftware = new SelectList(_context.Softwares.ToList(),"Id","Name");
            return View(model);
        }
        [HttpPost]
        public ActionResult AddNewProject(ProjectEntryViewModel model)
        {
            if(ModelState.IsValid)
            {
                //pravime soodveten objekt i go zacuvuvame vo baza so cel da dobieme ID za nego
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

                //ja zacuvuvame glavnata slika
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

                //pri kreiranje na nov proekt site softveri od modelot gi stavame vo baza
                foreach (int a in model.SoftwareUsedUrls)
                    _context.ProjectSoftware.Add(new ProjectSoftware(temp.ProjectId, a));

                //stavame timestamp i gi zacuvuvame site promeni vo bazata
                temp.TimeStamp = DateTime.Now.ToString("MMMM d, yyyy h:mm tt");
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
            var softwareId = _context.ProjectSoftware.Where(x => x.ProjectId == id).Select(x => x.SoftwareId).ToList();
            model.SoftwareUsedUrls = _context.Softwares.Where(x => softwareId.Contains(x.Id)).ToList();
            model.Likes = _context.Likes.Where(x => x.ProjectId == id).Count();
            return View(model);
        }
        public ActionResult DeleteProject(int id)
        {
            var project = _context.ProjectEntries.Find(id);
            if (User.Identity.GetUserId() != project.UserId && !User.IsInRole("Administrator"))
                return Content("no permissions");
            if (project == null)
                return new HttpNotFoundResult();
            DeleteProject(project);
            return RedirectToAction("Index","Profile");
        }
        public void DeleteProject(ProjectEntry project)
        {
            if (User.Identity.GetUserId() == project.UserId)
            {
                var id = project.ProjectId;
                var userId = project.UserId;
                var projectFolder = Path.Combine(Server.MapPath("~/UserUploads/"), userId, id.ToString());
                var images = _context.Images.Where(x => x.ProjectId == id);
                var likes = _context.Likes.Where(x => x.ProjectId == id);
                var software = _context.ProjectSoftware.Where(x => x.ProjectId == id);
                _context.Images.RemoveRange(images);
                _context.Likes.RemoveRange(likes);
                _context.ProjectEntries.Remove(project);
                _context.ProjectSoftware.RemoveRange(software);
                _context.SaveChanges();
                Directory.Delete(projectFolder, true);
            }  
        }
        public ActionResult EditProject(int id)
        { 
            var project = _context.ProjectEntries.Find(id);
            if (project == null)
                return new HttpNotFoundResult();
            if (User.Identity.GetUserId() != project.UserId)
                return Content("no permissions");
            var images = _context.Images.Where(x => x.ProjectId == id).ToList();

            var softwareId = _context.ProjectSoftware.Where(x => x.ProjectId == id).Select(x => x.SoftwareId).ToList();
            var allSoftware = new MultiSelectList(_context.Softwares.ToList(), "Id", "Name", softwareId);
            var software = _context.Softwares.Where(x => softwareId.Contains(x.Id)).Select(x => x.Id).ToList();

            var model = new EditProjectEntryViewModel(id,project.Name,project.Description,project.MainImage,images,allSoftware,software);
            return View(model);
        }
        [HttpPost]
        public ActionResult EditProject(EditProjectEntryViewModel model)
        {
            if(!ModelState.IsValid)
                return View(model);
            var toUpdate = _context.ProjectEntries.Find(model.ProjectId);
            if (toUpdate == null)
                return new HttpNotFoundResult();
            if (User.Identity.GetUserId() != toUpdate.UserId)
                return Content("no permissions");

            toUpdate.Name = model.Name;
            toUpdate.Description = model.Description;

            var projectFolder = Path.Combine(Server.MapPath("~/UserUploads/"), User.Identity.GetUserId(), model.ProjectId.ToString());

            if (model.NewMainImage!=null)
            {
                var newImage = model.NewMainImage;
                var filename = newImage.FileName;
                newImage.SaveAs(Path.Combine(projectFolder, filename));
                var pathInBase = Path.Combine("~/UserUploads/", User.Identity.GetUserId(), model.ProjectId.ToString(), filename);
                toUpdate.MainImage = pathInBase;
            }

            //zacuvuvanje na novite sliki, ovie ne se prikazuvaat vednas za razlika od main image
            foreach(HttpPostedFileBase file in model.NewImageUrls)
            {
                if(file!=null)
                {
                    var filename = file.FileName;
                    file.SaveAs(Path.Combine(projectFolder, filename));
                    var pathInBase = Path.Combine("~/UserUploads/", User.Identity.GetUserId(), model.ProjectId.ToString(), filename);
                    _context.Images.Add(new Image(model.ProjectId, pathInBase));
                }
            }

            //za softverite, prvo gi zemame postoeckite vo baza za toj proekt i gi brisime
            var newSoftware = model.Software;
            var currentSoftware = _context.ProjectSoftware.Where(x => x.ProjectId == model.ProjectId);
            if(currentSoftware!=null)
                _context.ProjectSoftware.RemoveRange(currentSoftware);
            //a potoa site od modelot gi dodavame vo bazata
            if(newSoftware!=null)
                foreach (int a in newSoftware)
                    _context.ProjectSoftware.Add(new ProjectSoftware(model.ProjectId, a));

            _context.SaveChanges();
            var id = model.ProjectId;
            return RedirectToAction("ViewProject/"+id);
        }
        //funkcija so ajax za brisenje na sliki od proektot
        public JsonResult DeleteImage(int id)
        {
            var toDelete = _context.Images.Find(id);
            if (toDelete!=null)
            {
                var project = _context.ProjectEntries.Find(toDelete.ProjectId);

                if(project.UserId!=User.Identity.GetUserId())
                    return Json(false, JsonRequestBehavior.AllowGet);

                var filename = Path.Combine(Server.MapPath("~/UserUploads/"), User.Identity.GetUserId(),project.ProjectId.ToString(), id.ToString());
                System.IO.File.Delete(filename);
                _context.Images.Remove(toDelete);
                _context.SaveChanges();
                return Json(true, JsonRequestBehavior.AllowGet);

            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult ChangeMainImage()
        {
            //funkcija so ajax za stavanje na nova main slika na proektot, vednas se prikazuva, ako ne se klikni Save ne se zacuvuva vo baza ova
            if (Request.Files.Count != 0)
            {
                var file = Request.Files[0];
                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath("~/UserUploads/Temp"), fileName);
                file.SaveAs(path);
                var newPath = Path.Combine(@"/UserUploads/Temp", Path.GetFileName(fileName));
                return Json(new { success = true, newImagePath = newPath }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult LikeProject(int id)
        {
            var project = _context.ProjectEntries.Find(id);
            if(project!=null)
            {
                var userId = User.Identity.GetUserId();
                var exists = _context.Likes.Where(x => x.UserId == userId &&x.ProjectId==project.ProjectId).SingleOrDefault();
                if (exists == null)
                    _context.Likes.Add(new Like(userId, project.ProjectId));
                else
                    _context.Likes.Remove(exists);
                _context.SaveChanges();
                var newCount = _context.Likes.Where(x => x.ProjectId == project.ProjectId).Count();
                return Json(new { success = true, newCount =  newCount}, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ViewLikes(int id)
        {
            var project = _context.ProjectEntries.Find(id);
            if (project != null)
            {
                var userIds = _context.Likes.Where(x => x.ProjectId == id).Select(x=>x.UserId).ToList();
                var users = _context.Users.Where(x => userIds.Contains(x.Id)).ToList();
                return PartialView(users);
            }
            return new HttpNotFoundResult();
        }

    }
}