using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DemoApp.Models;

namespace DemoApp.Controllers
{
    [Route("Auth")]
    public class AuthController : Controller
    {
        private readonly OnlineShopContext _context;

        public AuthController(OnlineShopContext context)
        {
            _context = context;
        }
 
        public ViewResult Index()
        {
            return View();
        }


        [HttpPost]
        [Route("Signup")]
        public async Task<IActionResult> SignUp(User user)
        {
            if (user == null)
            {
                return BadRequest("Invalid user data");
            }

            var exists = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
            if (exists != null)
            {
                return BadRequest("Email already exists");
            }

           
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Auth");  
        }

        [HttpPost]
        [Route("Signin")]
        public async Task<IActionResult> SignIn(string email, string password)
        {

            Console.WriteLine(email);
            Console.WriteLine(password);

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email && u.Password == password);
            if (user != null)
            {
                return LocalRedirect("/Admin");
            }

            return LocalRedirect("/auth");
        }

    }
}
