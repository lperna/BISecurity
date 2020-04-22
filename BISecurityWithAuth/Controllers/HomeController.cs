using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BISecurityWithAuth.Models;
using System.Security.Claims;
using BISecurityWithAuth.Data;
using Microsoft.AspNetCore.Authorization;

namespace BISecurityWithAuth.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserRepository _userRepository;
        private readonly ICustomerRepository _customerRepository;

        public HomeController(ILogger<HomeController> logger,IUserRepository userRepository, ICustomerRepository customerRepository) {
            _logger = logger;
            _userRepository = userRepository;
            _customerRepository = customerRepository;
        }

        [AllowAnonymous]
        public IActionResult Index() {
            var loginUserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            var model = new HomeViewModel();
            if (loginUserId != null) {
                model.WindowsCredentials = GetFakeCredentialsList();
                model.User = _userRepository.GetUserByLoginUserId(loginUserId);
                model.Customer = _customerRepository.GetCustomerById(model.User.CustomerId);
            }
            return View(model);
        }

        private static List<WindowsCredential> _fakeCredentials;
        private List<WindowsCredential> GetFakeCredentialsList() {
            if (_fakeCredentials == null) {
                _fakeCredentials = new List<WindowsCredential>();
                _fakeCredentials.Add(new WindowsCredential { Username = "Joe" });
                _fakeCredentials.Add(new WindowsCredential { Username = "Andrew" });
                _fakeCredentials.Add(new WindowsCredential { Username = "Lloyd" });
                _fakeCredentials.Add(new WindowsCredential { Username = "Arthur" });
            }
            
            return _fakeCredentials;
        }

        
        public IActionResult Create() {
            var model = new WindowsCredential();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([FromForm] WindowsCredential cred) {
            _fakeCredentials.Add(cred);
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult Edit(string id) {
            var theTarget = _fakeCredentials.Find(c => c.Username == id);

            return View(theTarget);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string id, [FromForm] WindowsCredential cred) {
            var theTarget = _fakeCredentials.Find(c => c.Username == id);
            theTarget.Username = cred.Username;
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy() {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
