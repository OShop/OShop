using Orchard.ContentManagement;

namespace OShop.Models {
    public class ShippingPart : ContentPart<ProductPartRecord>, IShippingInfo {
        public bool RequiresShipping {
            get { return this.Retrieve(x => x.RequiresShipping); }
            set { this.Store(x => x.RequiresShipping, value); }
        }

        public double Weight {
            get { return this.Retrieve(x => x.Weight); }
            set { this.Store(x => x.Weight, value); }
        }

        public double Width {
            get { return this.Retrieve(x => x.Width); }
            set { this.Store(x => x.Width, value); }
        }

        public double Height {
            get { return this.Retrieve(x => x.Height); }
            set { this.Store(x => x.Height, value); }
        }

        public double Lenght {
            get { return this.Retrieve(x => x.Lenght); }
            set { this.Store(x => x.Lenght, value); }
        }
    }
}