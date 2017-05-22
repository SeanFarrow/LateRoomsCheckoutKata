using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LateRoomsCheckoutKata.Checkout.Domain.Products
{
    public class Product
    {
            public Product(string sku, uint unitPrice)
            {
                if (sku == null)
                {
                    throw new ArgumentNullException("sku", "A null sku has been passed in. The sku cannot be null.");
                }
                
                throw new NotImplementedException();
            }
        }
}
