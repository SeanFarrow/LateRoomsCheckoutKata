using System;
using System.Linq;

namespace LateRoomsCheckoutKata.Checkout.Domain.Products
{
    /// <summary>
    ///Exception thrown when a <see cref="Product"/> with the passed in SKU cannot be found in the <see cref="IProductRepository"/> implementation. 
    /// </summary>
    ///<remarks>
    ///This should only happen when the <see cref="Product"/> is no longer sold by the supermarket.
    /// </remarks>
    public class ProductWithSKUNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProductWithSKUNotFoundException"/> class.
        /// </summary>
        /// <param name="sku">The stock keeping unit that could not be found.</param>
        public ProductWithSKUNotFoundException(string sku)
            : base(CheckSKUIsValid(sku))
        {
        }
        
        /// <summary>
        /// Ensure the stock keeping unit passed in is not <c>null</c> or an empty <c>string</c>.
        /// </summary>
        /// <param name="sku">The stock keeping unit to validate.</param>
        /// <returns>The formatted message containing the invalid stock keeping unit to pass to the <see cref="Exception"/>s constructor.</returns>
        ///<exception cref="ArgumentNullException">The <paramref name="sku"/> passed in is <c>null</c>.</exception>
        ///<exception cref="ArgumentException">The <paramref name="sku"/> passed in is an empty string.></exception>
        private static string CheckSKUIsValid(string sku)
        {
            if (sku == null)
            {
                throw new ArgumentNullException("sku", "A null sku has been passed in. The sku cannot be null.");
            }
            else if (!sku.Any())
            {
                throw new ArgumentException("An empty sku has been passed in. The sku cannot be an empty string.", "sku");
            }
            
            return string.Format("The product with stock keeping unit '{0}' could not be found in the product repository.", sku);
        }
    }
}