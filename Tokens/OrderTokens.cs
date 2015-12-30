using Orchard.ContentManagement;
using Orchard.Environment.Extensions;
using Orchard.Localization;
using Orchard.Tokens;
using OShop.Models;

namespace OShop.Tokens {
    [OrchardFeature("OShop.Orders")]
    public class OrderTokens : ITokenProvider {
        public OrderTokens() {
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        public void Describe(DescribeContext context) {
            context.For("Content", T("Order"), T("Tokens for order"))
                .Token("Reference", T("Reference"), T("Order reference"), "Text")
                ;
        }

        public void Evaluate(EvaluateContext context) {
            context.For<IContent>("Content")
                .Token("Reference", order => order.As<OrderPart>().Reference)
                .Chain("Reference", "Text", order => order.As<OrderPart>().Reference)
                ;
        }
    }
}