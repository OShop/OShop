using Orchard.ContentManagement.Drivers;
using Orchard.Environment.Extensions;
using OShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OShop.Drivers {
    [OrchardFeature("OShop.Customers")]
    public class CustomerOrderPartDriver : ContentPartDriver<CustomerOrderPart> {

    }
}