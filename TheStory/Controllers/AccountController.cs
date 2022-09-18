﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TheStory.Data;
using TheStory.ViewModels;
using TheStory.Models;
using System.Security.Cryptography;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using TheStory.Utilities;
using System.ComponentModel.DataAnnotations;

namespace TheStory.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationContext _applicationContext;
        public AccountController(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Login([FromForm]AuthViewModel authViewModel, [FromQuery]string ReturnUrl)
        {
            TempData["AuthType"] = "Login";
            if (!ModelState.IsValid)
            {
                TempData["AuthError"] = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToArray() as string[];
                return Redirect(ReturnUrl);
            }

            string email = authViewModel!.loginViewModel!.Email!;
            string password = authViewModel!.loginViewModel!.Password!;
            bool isPersistent = authViewModel!.loginViewModel!.IsPersistent;

            string hashed = PasswordHasher.Hash(password);
            var user = await _applicationContext.Users!.FirstOrDefaultAsync(x => x.Email == email && x.Password == PasswordHasher.Hash(password));
            if(user is null)
            {
                TempData["AuthError"] = new string[] { 
                    "Email or Password is incorrect"
                };
                return Redirect(ReturnUrl);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.IsPersistent, isPersistent.ToString())
            };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = isPersistent
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

            return Ok(authViewModel!.loginViewModel!.IsPersistent.ToString());
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Register([FromForm] AuthViewModel authViewModel, [FromQuery]string ReturnUrl)
        {
            TempData["AuthType"] = "Register";
            if (!ModelState.IsValid)
            {
                TempData["AuthError"] = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();
                return Redirect(ReturnUrl ?? "/Home/Index");
            }

            string email = authViewModel.RegisterViewModel!.Email!;
            string password = authViewModel.RegisterViewModel!.Password!;

            var newUser = new User
            {
                Email = email,
                Password = PasswordHasher.Hash(password)
            };

            try
            {
                await _applicationContext.Users!.AddAsync(newUser);
                await _applicationContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            when(ex?.InnerException is SqlException sqlEx && (sqlEx.Number == 2601 || sqlEx.Number == 2627))
            {
                // Handle user types existed email
                TempData["AuthError"] = new string[]
                {
                    "Your email already exists, please try again"
                };
                return Redirect(ReturnUrl);
            }

            return Redirect("/Home/Index");
        }
    }
}

