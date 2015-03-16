using Orchard;
using Orchard.ContentManagement;
using Orchard.Core.Common.Models;
using Orchard.Data;
using Orchard.Environment.Extensions;
using Orchard.Localization;
using Orchard.Mvc;
using Orchard.Mvc.Extensions;
using Orchard.Security;
using Orchard.Services;
using Orchard.Themes;
using Orchard.UI.Notify;
using Orchard.Utility.Extensions;
using OShop.Models;
using OShop.Permissions;
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
            IOrchardServices orchardServices,
            IAuthenticationService authenticationService,
            IContentManager contentManager,
            ITransactionManager transactionManager,
            IClock clock,
            ICustomersService customersService
            ) {
            Services = orchardServices;
            _authenticationService = authenticationService;
            _contentManager = contentManager;
            _transactionManager = transactionManager;
            _clock = clock;
            _customersService = customersService;
        }

        public IOrchardServices Services { get; private set; }
        public Localizer T { get; set; }

        [Themed]
        [Authorize]
        public ActionResult Index()
        {            
            var customer = _customersService.GetCustomer();
            if (customer != null) {
                if (Services.Authorizer.Authorize(Orchard.Core.Contents.Permissions.ViewContent, customer, T("You are not allowed to view this account."))) {
                    return new ShapeResult(this, _contentManager.BuildDisplay(customer.ContentItem));
                }
                else {
                    return new HttpUnauthorizedResult();
                }
            }
            else {
                return RedirectToAction("Create");
            }
        }

        [Themed]
        [Authorize]
        [HttpPost, ActionName("Index")]
        [FormValueRequired("Action")]
        public ActionResult IndexPost(string Action, int CustomerAddressId) {
            switch (Action) {
                case "Edit":
                    return RedirectToAction("EditAddress", "Customer", new { area = "OShop", id = CustomerAddressId, ReturnUrl = Url.Action("Index", "Customer", new { area = "OShop" }) });
                case "Remove":
                    var contentItem = _contentManager.Get(CustomerAddressId, VersionOptions.Latest);

                    if (!Services.Authorizer.Authorize(Orchard.Core.Contents.Permissions.DeleteContent, contentItem, T("You are not allowed to remove this address.")))
                        return new HttpUnauthorizedResult();

                    if (contentItem != null) {
                        _contentManager.Remove(contentItem);
                        Services.Notifier.Information(T("Your address was successfully removed."));
                    }
                    return Index();
                default:
                    return Index();
            }
        }

        [Authorize]
        [OutputCache(Duration = 0)]
        public ActionResult PreviewAddress(int id) {
            var address = _contentManager.Get<CustomerAddressPart>(id);
            if (address == null) {
                return new HttpNotFoundResult();
            }
            else if (Services.Authorizer.Authorize(Orchard.Core.Contents.Permissions.ViewContent, address)) {
                return new ShapeResult(this, _contentManager.BuildDisplay(address.ContentItem));
            }
            else {
                return new HttpUnauthorizedResult();
            }
        }

        [Themed]
        [Authorize]
        public ActionResult Create(string returnUrl = null) {
            var customer = _customersService.GetCustomer();
            if (customer == null) {
                if (Services.Authorizer.Authorize(CustomersPermissions.EditOwnCustomerAccount, T("You are not allowed to create an account."))) {
                    return View(_contentManager.BuildEditor(_contentManager.New("Customer")));
                }
                else {
                    return this.RedirectLocal(returnUrl, () => new HttpUnauthorizedResult());
                }
            }
            else {
                return RedirectToAction("Edit", new { ReturnUrl = returnUrl });
            }
        }

        [Authorize]
        [HttpPost, ActionName("Create")]
        [FormValueRequired("submit.Save")]
        public ActionResult CreatePost(string returnUrl = null) {
            if (!Services.Authorizer.Authorize(CustomersPermissions.EditOwnCustomerAccount, T("You are not allowed to create an account."))) {
                return new HttpUnauthorizedResult();
            }

            ContentItem customer;
            var customerPart = _customersService.GetCustomer();
            if (customerPart != null) {
                customer = customerPart.ContentItem;
            }
            else {
                customer = _contentManager.New("Customer");
                _contentManager.Create(customer);
            }

            var model = _contentManager.UpdateEditor(customer, this);
            customerPart = customer.As<CustomerPart>();
            if (customerPart != null) {
                customerPart.User = Services.WorkContext.CurrentUser;
            }

            if (!ModelState.IsValid) {
                _transactionManager.Cancel();
                return View(model);
            }

            Services.Notifier.Information(T("Your customer account was successfully created."));

            return this.RedirectLocal(returnUrl, () => RedirectToAction("Index"));
        }

        [Themed]
        [Authorize]
        public ActionResult Edit(string returnUrl = null) {
            var customer = _customersService.GetCustomer();
            if (customer != null) {
                if (Services.Authorizer.Authorize(CustomersPermissions.EditOwnCustomerAccount, T("You are not allowed to edit this account."))) {
                    return View(_contentManager.BuildEditor(customer.ContentItem));
                }
                else {
                    return this.RedirectLocal(returnUrl, () => new HttpUnauthorizedResult());
                }
            }
            else {
                return RedirectToAction("Create", new { ReturnUrl = returnUrl });
            }
        }

        [Authorize]
        [HttpPost, ActionName("Edit")]
        [FormValueRequired("submit.Save")]
        public ActionResult EditPost(string returnUrl = null) {
            var customerPart = _customersService.GetCustomer();

            var customer = _contentManager.Get(customerPart.ContentItem.Id, VersionOptions.DraftRequired);

            if (!Services.Authorizer.Authorize(CustomersPermissions.EditOwnCustomerAccount, T("You are not allowed to edit this account."))) {
                return new HttpUnauthorizedResult();
            }
            
            var model = _contentManager.UpdateEditor(customer, this);

            if (!ModelState.IsValid) {
                _transactionManager.Cancel();
                return View(model);
            }

            Services.Notifier.Information(T("Your customer account was successfully updated."));

            return this.RedirectLocal(returnUrl, () => RedirectToAction("Index"));
        }

        [Themed]
        [Authorize]
        public ActionResult CreateAddress(string returnUrl = null) {
            var customer = _customersService.GetCustomer();
            if (customer == null) {
                return RedirectToAction("Create", new { ReturnUrl = returnUrl });
            }
            if (Services.Authorizer.Authorize(CustomersPermissions.EditOwnCustomerAccount, T("You are not allowed to create an address."))) {
                return View(_contentManager.BuildEditor(_contentManager.New("CustomerAddress")));
            }
            else {
                return this.RedirectLocal(returnUrl, () => new HttpUnauthorizedResult());
            }
        }

        [Themed]
        [Authorize]
        [HttpPost, ActionName("CreateAddress")]
        [FormValueRequired("submit.Save")]
        public ActionResult CreateAddressPost(bool IsDefaultAddress = false, string returnUrl = null) {
            var customer = _customersService.GetCustomer();
            if (customer == null) {
                return RedirectToAction("Create", new { ReturnUrl = returnUrl });
            }

            var customerAddress = _contentManager.New("CustomerAddress");

            if (!Services.Authorizer.Authorize(CustomersPermissions.EditOwnCustomerAccount, T("You are not allowed to create an address."))) {
                return new HttpUnauthorizedResult();
            }

            _contentManager.Create(customerAddress);
            var model = _contentManager.UpdateEditor(customerAddress, this);

            if (!ModelState.IsValid) {
                _transactionManager.Cancel();
                return View(model);
            }

            if (IsDefaultAddress) {
                SetDefaultAddress(customerAddress);
            }

            Services.Notifier.Information(T("Your address was successfully created."));

            return this.RedirectLocal(returnUrl, () => RedirectToAction("Index"));
        }

        [Themed]
        [Authorize]
        public ActionResult EditAddress(int id, string returnUrl = null) {
            var address = _contentManager.Get<CustomerAddressPart>(id);
            if (address != null) {
                if (Services.Authorizer.Authorize(Orchard.Core.Contents.Permissions.EditContent, address, T("You are not allowed to edit this address."))) {
                    return View(_contentManager.BuildEditor(address));
                }
                else {
                    return this.RedirectLocal(returnUrl, () => new HttpUnauthorizedResult());
                }
            }
            else {
                return RedirectToAction("CreateAddress", new { ReturnUrl = returnUrl });
            }
        }

        [Themed]
        [Authorize]
        [HttpPost, ActionName("EditAddress")]
        [FormValueRequired("submit.Save")]
        public ActionResult EditAddressPost(int id, string returnUrl = null) {
            var address = _contentManager.Get<CustomerAddressPart>(id);
            if (address == null) {
                return HttpNotFound();
            }

            if (!Services.Authorizer.Authorize(Orchard.Core.Contents.Permissions.EditContent, address, T("You are not allowed to edit this address."))) {
                return new HttpUnauthorizedResult();
            }

            var model = _contentManager.UpdateEditor(address.ContentItem, this);

            if (!ModelState.IsValid) {
                _transactionManager.Cancel();
                return View(model);
            }

            Services.Notifier.Information(T("Your address was successfully updated."));

            return this.RedirectLocal(returnUrl, () => RedirectToAction("Index"));
        }

        private void SetDefaultAddress(ContentItem customerAddress) {
            var customer = _customersService.GetCustomer();
            if (customer != null) {
                customer.DefaultAddressId = customerAddress.Id;
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