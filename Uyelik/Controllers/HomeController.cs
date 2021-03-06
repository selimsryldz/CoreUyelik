﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Protocol;
using Uyelik.Models;
using Uyelik.ViewModels;

namespace Uyelik.Controllers
{
    public class HomeController : Controller
    {
        public UserManager<AppUser> userManager { get; }
        public SignInManager<AppUser> signInManager { get; }


        public HomeController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult LogIn(string ReturnUrl)
        {
            TempData["ReturnUrl"] = ReturnUrl;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LogIn(LoginViewModel userLogIn)
        {

            if (ModelState.IsValid)
            {

                AppUser user = await userManager.FindByEmailAsync(userLogIn.Email);

                if (user != null)
                {

                    if (await userManager.IsLockedOutAsync(user))
                    {
                        ModelState.AddModelError("", "Hesabınız bir süreliğine kilitlenmiştir. Lütfen daha sonra tekrar deneyiniz.");
                        return View(userLogIn);
                    }


                    await signInManager.SignOutAsync();

                    Microsoft.AspNetCore.Identity.SignInResult result = await signInManager.PasswordSignInAsync(user,
                        userLogIn.Password, userLogIn.RememberMe, false);

                    if (result.Succeeded)
                    {
                        await userManager.ResetAccessFailedCountAsync(user);
                        if (TempData["ReturnUrl"] != null)
                        {
                            return Redirect(TempData["ReturnUrl"].ToString());
                        }

                        return RedirectToAction("Index", "Member");
                    }

                    else
                    {
                        await userManager.AccessFailedAsync(user);

                        int fail = await userManager.GetAccessFailedCountAsync(user);

                        ModelState.AddModelError("", $"{fail} kez başarısız giriş yaptınız");



                        if (fail == 3)

                        {
                            await userManager.SetLockoutEndDateAsync(user, new DateTimeOffset(DateTime.Now.AddMinutes(20)));
                            ModelState.AddModelError("", "Hesabınız 3 başarısız girişten dolayı 20 dakika süreyle kitlenmiştir. " +
                                "Lütfen daha sonra tekrar deneyiniz");
                        }

                        else
                        {
                            ModelState.AddModelError("", "Geçersiz email adresi veya şifre");
                        }

                    }


                }
                else
                {
                    ModelState.AddModelError("", "Bu email adresine kayıtlı kullanıcı bulunamamıştır.");
                }
            }

            return View(userLogIn);
        }


        public IActionResult SignUp()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(UserViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                AppUser user = new AppUser();
                user.UserName = userViewModel.UserName;
                user.Email = userViewModel.Email;
                user.PhoneNumber = userViewModel.PhoneNumber;

                IdentityResult result = await userManager.CreateAsync(user, userViewModel.Password);

                if (result.Succeeded)
                {

                    return RedirectToAction("LogIn");
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }

            }

            return View(userViewModel);
        }


        public IActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ResetPassword(PasswordResetViewModel passwordResetViewModel)
        {
            AppUser user = userManager.FindByEmailAsync(passwordResetViewModel.Email).Result;

            if (user != null)
            {
                string passwordResetToken = userManager.GeneratePasswordResetTokenAsync(user).Result;

                string passwordResetLink = Url.Action("ResetPasswordConfirm", "Home", new
                {
                    userId = user.Id,
                    token = passwordResetToken

                }, HttpContext.Request.Scheme);

                Helper.PasswordReset.PasswordResetSendEmail(passwordResetLink, passwordResetViewModel.Email);

                ViewBag.status = "success";

            }
            else
            {
                ModelState.AddModelError("", "Sistemde kayıtlı email adresi bulunamamıştır.");
            }

            return View(passwordResetViewModel);
        }

        public IActionResult ResetPasswordConfirm(string userId, string token)
        {
            TempData["userId"] = userId;
            TempData["token"] = token;

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> ResetPasswordConfirm([Bind("PasswordNew")] PasswordResetViewModel passwordResetViewModel)
        {
            string token = TempData["token"].ToString();
            string userId = TempData["userId"].ToString();
            Console.WriteLine("userId: " + userId);
            Console.WriteLine("token: " + token);

          


            AppUser user = await userManager.FindByIdAsync(userId);

            if (user != null)
            {
                IdentityResult result = await userManager.ResetPasswordAsync(user, token, passwordResetViewModel.PasswordNew);

                if (result.Succeeded)
                {
                    await userManager.UpdateSecurityStampAsync(user);

                    TempData["passwordResetInfo"] = "Şifreniz başarıyla yenilenmiştir. Yeni şifre ile giriş yapabilirsiniz.";

                    ViewBag.status = "success";
                }

                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);

                    }
                }
            }

            else
            {
                ModelState.AddModelError("", "İlgili kullanıcı bulunamamıştır.");
            }


            return View(passwordResetViewModel);
        }

    }


}
