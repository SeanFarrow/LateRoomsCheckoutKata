namespace LateRoomsCheckoutKata.Checkout.Domain.Products
{
    /// <summary>
    /// An interface containing the functionality for discounting a specific <see cref="Product"/> stock keeping unit.
    /// </summary>
    public interface IProductDiscountRule
    {
        /// <summary>
        /// The quantity of the product this rule applies to that can be discounted.
        /// </summary>
        int QuantityToDiscount { get; }
        /// <summary>
        /// Calculates the discount that is applicable to the <see cref="Product"/> this rule is applicable to.
        /// </summary>
        /// <param name="numberOfScannedProducts">The total number of this <see cref="Product"/> that have been scanned throu the till.</param>
        /// <param name="unitPrice"><The unit price of the <see cref="Product"/> this rule applies to./param>
        /// <returns>The total price for the discounted portion of the order.</returns>
        int CalculateDiscount(int numberOfScannedProducts, int unitPrice);
    }
}