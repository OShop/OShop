using Orchard.Data;
using OShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OShop.Helpers {
    public static class AddressExtensions {
        public static void CopyTo(this IOrderAddress srcAddress, IOrderAddress destAddress) {
            destAddress.Company = srcAddress.Company;
            destAddress.FirstName = srcAddress.FirstName;
            destAddress.LastName = srcAddress.LastName;
            destAddress.Address1 = srcAddress.Address1;
            destAddress.Address2 = srcAddress.Address2;
            destAddress.Zipcode = srcAddress.Zipcode;
            destAddress.City = srcAddress.City;
            destAddress.Country = srcAddress.Country;
            destAddress.State = srcAddress.State;
        }

        public static int CreateOrUpdate(this IRepository<OrderAddressRecord> repository, OrderAddressRecord record) {
            if (record != null) {
                if (record.Id > 0) {
                    repository.Update(record);
                }
                else {
                    repository.Create(record);
                }
                return record.Id;
            }
            else {
                return 0;
            }
        }
    }
}