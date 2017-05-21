using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LateRoomsCheckoutKata.Checkout.Contracts;
using LateRoomsCheckoutKata.Checkout.Domain.Products;

namespace LateRoomsCheckoutKata.Checkout
{
    /// <summary>
    /// An implementation of a supermarket checkout.
    /// </summary>
    public class Checkout: ICheckout
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Checkout"/> class.
        /// </summary>
        /// <param name="productRepository">The repository providing access to the <see cref="Product"/>s available in the supermarket.</param>
        /// <param name="productDiscountRuleRepository">The repository providing access to the discount rules for <see cref="Product"/>s in the supermarket.</param>
        public Checkout(IProductRepository productRepository, IProductDiscountRuleRepository productDiscountRuleRepository)
        {
            if (productRepository == null)
            {
                throw new ArgumentNullException(
                    "productRepository",
                    "A null product repository has been passed in. The product repository cannot be null.");
            }
            
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="Checkout"/> class.
        /// </summary>
        public Checkout()
        {
            // TODO: Complete member initialization
        }
        
        ///<InheritDoc/>
        public void Scan(string item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item", "A null item has been passed in. The item cannot be null.");
            }
            else if (!item.Any())
            {
                throw new ArgumentException("An empty item has been passed in. The item cannot be an empty string.", "item");
            }

            throw new NotImplementedException();
        }
        
        ///<InheritDoc/>
        public int GetTotalPrice()
        {
            throw new NotImplementedException();
        }
    }
}
