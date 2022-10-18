using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text;
using TheStory.Data;
using TheStory.Models;
using TheStory.Utilities;
using TheStory.ViewModels;

namespace TheStory.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationContext _applicationContext;

        private string _generateUserName()
        {
            string baseStr = "123456789abcdefghiklmnpq!@#$%^&*()";
            int userNameLength = 7;

            StringBuilder result = new();

            Random rnd = new();
            for(int i = 1; i <= userNameLength; ++i)
            {
                char rndChar = baseStr[rnd.Next(0, baseStr.Length - 1)];
                result.Append(rndChar);
            }

            return result.ToString();
        }

        public AccountController(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromForm] AuthViewModel authViewModel, [FromQuery] string ReturnUrl)
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

            var user = await _applicationContext.Users!.FirstOrDefaultAsync(x => x.Email == email);
            if (user is null)
            {
                TempData["AuthError"] = new string[] {
                    "Email or Password is incorrect"
                };
                return Redirect(ReturnUrl);
            }

            string salt = user!.Salt!;
            string hashedPassword = user!.Password!;
            if (!PasswordHasher.CheckHash(password, salt, hashedPassword))
            {
                TempData["AuthError"] = new string[]
                {
                    "Email or Password is incorrect"
                };
                return Redirect(ReturnUrl);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.IsPersistent, isPersistent.ToString()),
                new Claim(ClaimTypes.Role, ((RoleEnum)user!.Role!).ToString()),
                new Claim(ClaimTypes.Name, user!.UserName!)
            };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = isPersistent,
                ExpiresUtc = DateTime.UtcNow.AddMonths(1)
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

            TempData["message"] = "Login success";
            return Redirect(ReturnUrl);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromForm] AuthViewModel authViewModel, [FromQuery] string ReturnUrl)
        {
            TempData["AuthType"] = "Register";
            if (!ModelState.IsValid)
            {
                TempData["AuthError"] = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();
                return Redirect(ReturnUrl ?? "/Home/Index");
            }

            string email = authViewModel.RegisterViewModel!.Email!;
            string password = authViewModel.RegisterViewModel!.Password!;
            string salt = PasswordHasher.GetSalt();

            var newUser = new User
            {
                Email = email,
                Password = PasswordHasher.Hash(salt, password),
                Salt = salt,
                Role = RoleEnum.Member,
                UserName = _generateUserName()
            };

            try
            {
                await _applicationContext.Users!.AddAsync(newUser);
                await _applicationContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            when (ex?.InnerException is SqlException sqlEx && (sqlEx.Number == 2601 || sqlEx.Number == 2627))
            {
                // Handle user types existed email
                TempData["AuthError"] = new string[]
                {
                    "Your email already exists, please try again"
                };
                return Redirect(ReturnUrl);
            }

            TempData["message"] = "Register success, please login!";
            return Redirect(ReturnUrl);
        }
    }
}


