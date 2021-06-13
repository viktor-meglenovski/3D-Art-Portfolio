using _3D_Art_Portfolio.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _3D_Art_Portfolio.ViewModels;

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
        
    }
}