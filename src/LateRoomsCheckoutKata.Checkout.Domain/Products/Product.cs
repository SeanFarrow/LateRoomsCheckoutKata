﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LateRoomsCheckoutKata.Checkout.Domain.Products
{
    public class Product
    {
            public Product(string sku, int unitPrice)
            {
                if (sku == null)
                {
                    throw new ArgumentNullException("sku", "A null sku has been passed in. The sku cannot be null.");
                }
                else if (!sku.Any())
                {
                    throw new ArgumentException("An empty sku has been passed in. The sku cannot be an empty string.", "sku");
                }
                else if (unitPrice <= 0)
                {
                    throw new ArgumentOutOfRangeException(
                        "unitPrice",
                        "A unit price less than or equal to 0 has been passed in.The unit price must be positive.");
                }
                
                this.SKU = sku;
                this.UnitPrice = unitPrice;
            }
        /// <summary>
        /// The stock keeping unit of the <see cref="Product"/>.
        /// </summary>
            public string SKU { get; }
        /// <summary>
        /// The <see cref="Product"/>s price.
        /// </summary>
        public int UnitPrice { get; }
    }
}
