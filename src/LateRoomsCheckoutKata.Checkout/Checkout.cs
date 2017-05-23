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
        private IProductRepository _productRepository;
        private IProductDiscountRuleRepository _productDiscountRuleRepository;
        private IDictionary<Product, uint> _till;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="Checkout"/> class.
        /// </summary>
        /// <param name="productRepository">The repository providing access to the <see cref="Product"/>s available in the supermarket.</param>
        /// <param name="productDiscountRuleRepository">The repository providing access to the discount rules for <see cref="Product"/>s in the supermarket.</param>
        /// <param name="till">The till to be used when unit testing the class. If this is <c>null</c> a new till will be assigned to the private field in the constructor.</param>
        ///<exception cref="ArgumentNullException">The <paramref name="productRepository"/> or <paramref name="productDiscountRuleRepository"/> passed in are <c>null</c>.</exception>
        public Checkout(IProductRepository productRepository, IProductDiscountRuleRepository productDiscountRuleRepository, IDictionary<Product, uint> till =null)
        {
            if (productRepository == null)
            {
                throw new ArgumentNullException("productRepository", "A null product repository has been passed in. The product repository cannot be null.");
            }
            else if (productDiscountRuleRepository == null)
            {
                throw new ArgumentNullException("productDiscountRuleRepository", "A null product discount rule repository has been passed in. The product discount rule repository cannot be null.");
            }
            
            this._productRepository = productRepository;
            this._productDiscountRuleRepository = productDiscountRuleRepository;
            this._till = till ?? new Dictionary<Product, uint>();
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

            try
            {
                var product = this._productRepository.FindProductBySKU(item);
                uint currentQuantity = 0;
                if (!this._till.TryGetValue(product, out currentQuantity))
                {
                    //The product doesn't exist in the till so add it with a quantity of 1.
                    this._till.Add(product, 1);
                }
                else
                {
                    //the product has been scanned at least once, so increment the quantity.
                    this._till[product] = ++currentQuantity;
                }
                }
            catch (ProductWithSKUNotFoundException)
            {
                throw;
            }
        }
        
        ///<InheritDoc/>
        public int GetTotalPrice()
        {
            return 0;
        }
    }
}
