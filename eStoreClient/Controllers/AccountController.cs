using eStoreClient.DTO.Member;
using eStoreClient.Services;
using Microsoft.AspNetCore.Mvc;
using Refit;

namespace eStoreClient.Controllers
{
    public class AccountController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IMemberService _memberService;

        public AccountController(IConfiguration configuration, IMemberService memberService)
        {
            _configuration = configuration;
            _memberService = memberService;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            string storedEmail = _configuration["Credentials:Email"];
            string storedPassword = _configuration["Credentials:Password"];
            if (email == storedEmail && password == storedPassword)
            {
                // Tạo session lưu thông tin người dùng
                HttpContext.Session.SetString("UserEmail", email);
                HttpContext.Session.SetString("Role", "Admin");
                HttpContext.Session.SetString("IsLoggedIn", "true");

                return RedirectToAction("Index", "Home");
            }

            try
            {
                var members = _memberService.GetMemberByEmailAsync(email).Result;
                if (members.Password == password)
                {
                    // Tạo session lưu thông tin người dùng
                    HttpContext.Session.SetString("UserEmail", email);
                    HttpContext.Session.SetString("Role", "Member");
                    HttpContext.Session.SetString("IsLoggedIn", "true");
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception)
            {
                ViewBag.Message = "Invalid email ";
                return View();
            }
            
            
            ViewBag.Message = "Invalid password.";
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(MemberRequestDTO member)
        {
            try
            {
                var createdMember = _memberService.CreateMemberAsync(member).Result;
                return RedirectToAction("Login");
            }
            catch (Exception)
            {
                ViewBag.Message = "Can not register with this email !";
                return View();
            }
        }

        public IActionResult Profile()
        {
            var member = _memberService.GetMemberByEmailAsync(HttpContext.Session.GetString("UserEmail")).Result;
            return View(member);
        }

        [HttpPost]
        public IActionResult Profile(MemberRequestDTO memberDto)
        {
            var member = _memberService.GetMemberByEmailAsync(HttpContext.Session.GetString("UserEmail")).Result;
            memberDto.Email = member.Email;
            memberDto.Password = member.Password;
            var updateMember = _memberService.UpdateMemberAsync(member.MemberId, memberDto).Result;
            return RedirectToAction("Profile");
        }

        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ChangePassword(string oldPassword, string newPassword)
        {
            var member = _memberService.GetMemberByEmailAsync(HttpContext.Session.GetString("UserEmail")).Result;
            if (member.Password == oldPassword)
            {
                MemberRequestDTO memberDTO = new MemberRequestDTO()
                {
                    City = member.City,
                    CompanyName = member.CompanyName,
                    Country = member.Country,
                    Email = member.Email,
                    Password = newPassword
                };
                var updateMember = _memberService.UpdateMemberAsync(member.MemberId, memberDTO).Result;
                
                HttpContext.Session.Clear();
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Message = "Invalid password.";
            return View();
        }

        public IActionResult Logout()
        {
            // Xóa session khi đăng xuất
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
