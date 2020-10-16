using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Uyelik.Models;

namespace Uyelik.Controllers
{
    public class AdminController : Controller
    {

        private UserManager<AppUser> userManager { get; }

        public AdminController(UserManager<AppUser> userManager)
        {
            this.userManager = userManager;
        }


        public IActionResult Index()
        {
            return View(userManager.Users.ToList());
        }
    }
}
