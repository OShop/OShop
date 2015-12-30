using Orchard.ContentManagement;
using Orchard.Environment.Extensions;
using Orchard.Localization;
using Orchard.Tokens;
using OShop.Models;

namespace OShop.Tokens {
    [OrchardFeature("OShop.Customers")]
    public class CustomerOrderTokens : ITokenProvider {
        public CustomerOrderTokens() {
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        public void Describe(DescribeContext context) {
            context.For("Content", T("Order"), T("Tokens for order"))
                .Token("Customer", T("Customer"), T("Order's customer"), "Content")
                ;

            context.For("Customer", T("Customer"), T("Tokens for customer"))
                .Token("FirstName", T("First name"), T("Customer's first name"))
                .Token("LastName", T("Last name"), T("Customer's last name"))
                .Token("Email", T("Email"), T("Customer's e-mail"))
                ;
        }

        public void Evaluate(EvaluateContext context) {
            context.For<IContent>("Content")
                .Token("Customer", order => order.As<CustomerOrderPart>().Customer.Title)
                .Chain("Customer", "Content", order => order.As<CustomerOrderPart>().Customer.ContentItem)
                ;

            context.For<IContent>("Content")
                .Token("FirstName", customer => customer.As<CustomerPart>().FirstName)
                .Chain("FirstName", "Text", customer => customer.As<CustomerPart>().FirstName)
                .Token("LastName", customer => customer.As<CustomerPart>().LastName)
                .Chain("LastName", "Text", customer => customer.As<CustomerPart>().LastName)
                .Token("Email", customer => customer.As<CustomerPart>().Email)
                .Chain("Email", "Text", customer => customer.As<CustomerPart>().Email)
                ;
        }
    }
}