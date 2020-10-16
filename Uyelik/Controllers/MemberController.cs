using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Uyelik.Models;
using Uyelik.ViewModels;

namespace Uyelik.Controllers
{

    [Authorize]
    public class MemberController : Controller
    {

        public UserManager<AppUser> userManager { get; }
        public SignInManager<AppUser> signInManager { get; }


        public MemberController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }




        public IActionResult Index()
        {

            
            AppUser user = userManager.FindByNameAsync(User.Identity.Name).Result;

            UserViewModel userViewModel = user.Adapt<UserViewModel>();

           
            

            return View(userViewModel);
        }

        public IActionResult PasswordChange()
        {
            return View();
        }

        
        [HttpPost]
        public IActionResult PasswordChange(PasswordChangeViewModel passwordChangeViewModel)
        {

            if(ModelState.IsValid)
            {
                AppUser user = userManager.FindByNameAsync(User.Identity.Name).Result;

                if (user!=null)
                {
                    bool exit = userManager.CheckPasswordAsync(user,passwordChangeViewModel.PasswordOld).Result;

                   


                }


            }



            return View();
        }


    }
}
