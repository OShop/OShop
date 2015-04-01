
namespace OShop.Models {
    public class TaxAmount {
        public TaxAmount(ITax tax) {
            Tax = tax;
        }

        public decimal TaxBase;
        public ITax Tax;
    }
}