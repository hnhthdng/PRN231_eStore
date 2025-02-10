using eStoreClient.DTO.Member;
using eStoreClient.Services;
using Microsoft.AspNetCore.Mvc;

namespace eStoreClient.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MemberController : Controller
    {
        private readonly IMemberService _memberService;
        public MemberController(IMemberService memberService)
        {
            _memberService = memberService;
        }
        public async Task<IActionResult> Index()
        {
            var members =  _memberService.GetAllMembersAsync().Result;
            members.OrderBy(m => m.MemberId);
            return View(members);
        }

        // Create action to show the form to create a new member
        public IActionResult Create()
        {
            var memberRequestDTO = new MemberRequestDTO(); // Ensure the model is initialized
            return View(memberRequestDTO);
        }

        // Post action to handle form submission for creating a new member
        [HttpPost]
        public async Task<IActionResult> Create(MemberRequestDTO member)
        {
            if (ModelState.IsValid)
            {
                 await _memberService.CreateMemberAsync(member); // Creating the member
                return RedirectToAction(nameof(Index)); // Redirect back to the Index page after creation
            }
            return View(member); // Return to Create view if validation fails
        }

       

        //Action to delete a member
        public async Task<IActionResult> Delete(int id)
        {
            var member = _memberService.GetMemberByIdAsync(id);
            if (member == null)
            {
                return NotFound();
            }
            await _memberService.DeleteMemberAsync(id); // Deleting the member
            return RedirectToAction(nameof(Index)); // Redirect to Index after deletion
        }

        // GET: Admin/Member/Update/{id}
        public async Task<IActionResult> Update(int id)
        {
            // Fetch member details by ID
            var member = await _memberService.GetMemberByIdAsync(id);

            if (member == null)
            {
                return NotFound(); // If member is not found, return a 404 error
            }

            // Create a MemberRequestDTO to bind to the form
            var memberDTO = new MemberResponseDTO
            {
                MemberId = member.MemberId,
                Email = member.Email,
                CompanyName = member.CompanyName,
                City = member.City,
                Country = member.Country,
                Password = member.Password // Include this if you need to allow password updates
            };

            return View(memberDTO); // Return the view with the member details for editing
        }

        // POST: Admin/Member/Update/{id}
        [HttpPost]
        public async Task<IActionResult> Update(MemberResponseDTO member)
        {
            if (ModelState.IsValid)
            {
                var memberRequestDTO = new MemberRequestDTO
                {
                    Email = member.Email,
                    CompanyName = member.CompanyName,
                    City = member.City,
                    Country = member.Country,
                    Password = member.Password
                };
                await _memberService.UpdateMemberAsync(member.MemberId, memberRequestDTO); // Update the member
                return RedirectToAction(nameof(Index)); // Redirect back to the Index page after update
            }
            return View(member); // Return to Update view if validation fails
        }


    }
}
