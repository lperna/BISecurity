using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using BISecurityWithAuth.Models;
using BISecurityWithAuth.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Differencing;

namespace BISecurityWithAuth.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class UserController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ICustomerRepository _customerRepository;
        private readonly IUserRepository _userRepository;
        private readonly RoleManager<IdentityRole> _roleManager;

        private static IdentityRole _adminRole;
        private static IdentityRole _customerRole;


        [BindProperty]
        public AddUserModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public UserController(UserManager<IdentityUser> userManager,
                                RoleManager<IdentityRole> roleManager,
                                ICustomerRepository customerRepository, IUserRepository userRepository) {
            _userManager = userManager;
            _userRepository = userRepository;
            _customerRepository = customerRepository;
            _roleManager = roleManager;
            
        }

        private async Task InitializeRolesAsync() {
            _adminRole = await _roleManager.FindByNameAsync("Administrator");
            if (_adminRole == null) {
                var newAdminRole = new IdentityRole("Administrator");
                var adminResult = await _roleManager.CreateAsync(newAdminRole);
                if (!adminResult.Succeeded) {
                    throw new Exception("Unable to create admin role,");
                }
                _adminRole = await _roleManager.FindByNameAsync("Administrator");
            };
            _customerRole = await _roleManager.FindByNameAsync("Customer");
            if (_customerRole == null) {
                var newCustomerRole = new IdentityRole("Customer");
                var customerResult = await _roleManager.CreateAsync(newCustomerRole);
                if (!customerResult.Succeeded) {
                    throw new Exception("Unable to create admin role,");
                }
                _customerRole = await _roleManager.FindByNameAsync("Customer");
            };
        }

        public IActionResult Index() {
           

           var model = new List<UserViewModel>();
            var loginUsers = _userManager.Users.ToList();
            foreach (var lu in loginUsers) {
                var vm = CreateUserViewModel(lu);
                if (vm != null) {
                    model.Add(vm);
                }
            }
            return View(model);
        }
        public UserViewModel CreateUserViewModel(IdentityUser LoginUser) {
            if (LoginUser != null) {
                var user = _userRepository.GetUserByLoginUserId(LoginUser.Id);
                if (user != null) {
                    var customer = _customerRepository.GetCustomerById(user.CustomerId);
                    return new UserViewModel(user, LoginUser, customer);
                }
            }
            return null;
        }

        public async Task<ActionResult> EditAsync(string id) {
            var sqlId = new Guid(id);
            var user = _userRepository.GetUserById(sqlId);
            if (user != null) {
                var loginUser = await _userManager.FindByIdAsync(user.LoginUserId);
                var model = new EditUserModel();
                
                model.Email = loginUser.Email;
                model.CustomerId = user.CustomerId.ToString();
                model.IsAdmin = user.IsAdmin;

                if (model != null) {
                    ViewBag.CustomerList = _customerRepository.GetAllCustomers();
                    return View(model);
                }
            }
            return RedirectToAction("Error", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync(string Id, [FromForm] EditUserModel u) {
            if (!String.IsNullOrEmpty(Id)) {
                var sqlId = new Guid(Id);
                var user = _userRepository.GetUserById(sqlId);
                if (user != null) {
                    user.CustomerId = new Guid(u.CustomerId);
                    user.IsAdmin = u.IsAdmin;
                    // add or remove admin credentials here
                    await ModifyRoles(user);

                    var result = _userRepository.UpdateUser(user);
                    if (result > 0) {
                        return RedirectToAction(nameof(Index));
                    }

                    ViewBag.CustomerList = _customerRepository.GetAllCustomers();
                    return View(u);
                }
            }
            return RedirectToAction("Error", "Home");
        }

        private async Task ModifyRoles(User user) {
            await InitializeRolesAsync();
            var loginUser = await _userManager.FindByIdAsync(user.LoginUserId);
            if (user.IsAdmin) {
                await _userManager.AddToRoleAsync(loginUser, "Administrator");
                await _userManager.RemoveFromRoleAsync(loginUser, "Customer");
            } else {
                await _userManager.AddToRoleAsync(loginUser, "Customer");
                await _userManager.RemoveFromRoleAsync(loginUser, "Administrator");
            }
        }

        public IActionResult Create(string returnUrl = null) {
            ReturnUrl = returnUrl;
            ViewBag.CustomerList = _customerRepository.GetAllCustomers();
            return View();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAsync([FromForm] AddUserModel u) {
            await InitializeRolesAsync();
            if (ModelState.IsValid) {

                var user = new User() {
                    Id = Guid.NewGuid(),
                    CustomerId = new Guid(u.CustomerId),
                    IsAdmin = u.IsAdmin
                };
                var userResult = _userRepository.AddUser(user);
                if (userResult > 0) {
                    var loginUser = new IdentityUser { UserName = u.Email, Email = u.Email };
                    var result = await _userManager.CreateAsync(loginUser, u.Password);
                    if (result.Succeeded) {
                        loginUser = await _userManager.FindByEmailAsync(loginUser.Email);
                        var updateUser = _userRepository.GetUserById(user.Id);
                        updateUser.LoginUserId = loginUser.Id;
                        var updateResult = _userRepository.UpdateUser(updateUser);
                        await ModifyRoles(updateUser);
                        if (updateResult > 0) {
                            return RedirectToAction("Index");
                        }
                    }
                    _userRepository.DeleteUserById(user.Id);
                    foreach (var error in result.Errors) {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            // If we got this far, something failed, redisplay form
            ViewBag.CustomerList = _customerRepository.GetAllCustomers();
            return View(u);
        }
    }
}