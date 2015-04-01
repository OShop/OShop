using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OShop.Models {
    public class Tax : ITax {
        public Tax() { }

        public Tax(ITax iTax) {
            Name = iTax.Name;
            Rate = iTax.Rate;
        }

        public string Name;
        public decimal Rate;

        string ITax.Name {
            get { return Name; }
        }

        decimal ITax.Rate {
            get { return Rate; }
        }
    }
}