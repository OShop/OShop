using Orchard.ContentManagement;
using Orchard.Data;
using Orchard.Environment.Extensions;
using Orchard.Mvc;
using Orchard.Mvc.Extensions;
using Orchard.Security;
using Orchard.Services;
using Orchard.Themes;
using OShop.Models;
using OShop.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OShop.Controllers
{
    [OrchardFeature("OShop.Customers")]
    public class CustomerController : Controller, IUpdateModel
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IContentManager _contentManager;
        private readonly ITransactionManager _transactionManager;
        private readonly IClock _clock;
        private readonly ICustomersService _customersService;

        public CustomerController(
            IAuthenticationService authenticationService,
            IContentManager contentManager,
            ITransactionManager transactionManager,
            IClock clock,
            ICustomersService customersService
            ) {
            _authenticationService = authenticationService;
            _contentManager = contentManager;
            _transactionManager = transactionManager;
            _clock = clock;
            _customersService = customersService;
        }

        [Themed]
        public ActionResult Index()
        {
            var user = _authenticationService.GetAuthenticatedUser();
            if (user == null) {
                return RedirectToAction("LogOn", "Account", new { area = "Orchard.Users", ReturnUrl = Url.Action("Index", "Customer", new { area = "OShop" }) });
            }
            
            var customer = _customersService.GetCustomer(user.Id);
            if (customer != null) {
                return new ShapeResult(this, _contentManager.BuildDisplay(customer.ContentItem));
            }
            else {
                return RedirectToAction("Create");
            }
        }

        [Themed]
        public ActionResult Create(string returnUrl = null) {
            var user = _authenticationService.GetAuthenticatedUser();
            if (user == null) {
                return RedirectToAction("LogOn", "Account", new { area = "Orchard.Users", ReturnUrl = Url.Action("Create", "Customer", new { area = "OShop" }) });
            }

            var customer = _customersService.GetCustomer(user.Id);
            if (customer == null) {
                return View(_contentManager.BuildEditor(_contentManager.New("Customer")));
            }
            else {
                return RedirectToAction("Edit");
            }
        }

        [HttpPost, ActionName("Create")]
        public ActionResult CreatePost(string returnUrl = null) {
            var user = _authenticationService.GetAuthenticatedUser();
            if (user == null) {
                return RedirectToAction("LogOn", "Account", new { area = "Orchard.Users", ReturnUrl = Url.Action("Create", "Customer", new { area = "OShop" }) });
            }

            var customer = _contentManager.New("Customer");

            _contentManager.Create(customer, VersionOptions.Draft);
            var model = _contentManager.UpdateEditor(customer, this);

            if (!ModelState.IsValid) {
                _transactionManager.Cancel();
                return View(model);
            }

            _contentManager.Publish(customer);

            return this.RedirectLocal(returnUrl, () => RedirectToAction("Index"));
        }

        [Themed]
        public ActionResult Edit() {
            var user = _authenticationService.GetAuthenticatedUser();
            if (user == null) {
                return RedirectToAction("LogOn", "Account", new { area = "Orchard.Users", ReturnUrl = Url.Action("Edit", "Customer", new { area = "OShop" }) });
            }

            var customer = _customersService.GetCustomer(user.Id);
            if (customer != null) {
                return new ShapeResult(this, _contentManager.BuildEditor(customer.ContentItem));
            }
            else {
                return RedirectToAction("Create");
            }
        }

        void IUpdateModel.AddModelError(string key, Orchard.Localization.LocalizedString errorMessage) {
            ModelState.AddModelError(key, errorMessage.ToString());
        }

        bool IUpdateModel.TryUpdateModel<TModel>(TModel model, string prefix, string[] includeProperties, string[] excludeProperties) {
            return TryUpdateModel(model, prefix, includeProperties, excludeProperties);
        }
    }
}