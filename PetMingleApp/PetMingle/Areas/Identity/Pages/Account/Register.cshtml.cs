using PetMingle.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace PetMingle.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RegisterModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }
        
        public string ReturnUrl { get; set; }

        public void OnGet()
        {
            ReturnUrl = Url.Content("~/");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ReturnUrl = Url.Content("~/");

            if (ModelState.IsValid)
            {
                var userExists = await _userManager.FindByNameAsync(Input.Email);
                if (userExists != null)
                {
                    ModelState.AddModelError(string.Empty, "This email is already linked to an account. Please try another email.");
                    return Page();
                }

                var identity = new ApplicationUser { UserName = Input.Email, RegistrationDate = DateTime.Now };
                var result = await _userManager.CreateAsync(identity, Input.Password);

                string userRole;
                if (Input.Email.StartsWith("admin."))
                {
                    userRole = "Admin";
                }
                else
                {
                    userRole = "User";
                }

                var role = new IdentityRole(userRole);
                var addRoleResults = await _roleManager.CreateAsync(role);
                var addUserRoleResults = await _userManager.AddToRoleAsync(identity, userRole);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(identity, isPersistent: false);
                    return LocalRedirect(ReturnUrl);
                }
            }

            ModelState.AddModelError(string.Empty, "Please enter a valid email."); //fix
            return Page();
        }

        public class InputModel
        {
            private string? _email;

            [Required]
            public string Email
            {
                get { return _email; }
                set
                {
                    if (value == _email) return;

                    if (!value.Contains('@') && !value.Contains(".com"))
                    {
                        return;
                    }

                    _email = value;
                }
            }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }
    }
}