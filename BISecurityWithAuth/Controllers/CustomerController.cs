using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BISecurityWithAuth.Data;
using BISecurityWithAuth.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BISecurityWithAuth.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class CustomerController : Controller {
        private readonly ICustomerRepository _customerRepository;

        public CustomerController(ICustomerRepository customerRepository) {
            _customerRepository = customerRepository;
        }
        // GET: Customer
        public ActionResult Index() {
            return View(_customerRepository.GetAllCustomers());
        }

        // GET: Customer/Details/5
        public ActionResult Details(string id) {
            var sqlId = new Guid(id);
            var customer = _customerRepository.GetCustomerById(sqlId);
            if (customer != null) {
                return View(customer);
            }
            return RedirectToAction("Error", "Home");
        }

        // GET: Customer/Create
        public ActionResult Create() {
            return View();
        }

        // POST: Customer/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([FromForm] Customer c) {
            try {
                var sqlId = Guid.NewGuid();
                var customer = new Customer();
                customer.Id = sqlId;
                customer.Name = c.Name;
                customer.CredentialPrefix = c.CredentialPrefix;
                _customerRepository.AddCustomer(customer);

                return RedirectToAction(nameof(Index));

            } catch {
                return View();
            }
        }

        // GET: Customer/Edit/5
        public ActionResult Edit(string id) {
            var sqlId = new Guid(id);
            var customer = _customerRepository.GetCustomerById(sqlId);
            if (customer != null) {
                return View(customer);
            }
            return RedirectToAction("Error", "Home");
        }

        // POST: Customer/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string id, [FromForm] Customer c) {
            try
            {
                var sqlId = new Guid(id);
                var customer = _customerRepository.GetCustomerById(sqlId);
                if (customer != null) {
                    customer.Name = c.Name;
                    customer.CredentialPrefix = c.CredentialPrefix;
                    _customerRepository.UpdateCustomer(customer);

                    return RedirectToAction(nameof(Index));
                }
                return View();
            }
            catch
            {
                return View();
            }
        }

        // GET: Customer/Delete/5
        public ActionResult Delete(string id)
        {
            var sqlId = new Guid(id);
            var customer = _customerRepository.GetCustomerById(sqlId);
            if (customer != null) {
                return View(customer);
            }
            return RedirectToAction("Error", "Home");
        }

        // POST: Customer/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id, IFormCollection collection)
        {
            try
            {
                var sqlId = new Guid(id);
                var customer = _customerRepository.DeleteCustomerById(sqlId);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

    }

    
}