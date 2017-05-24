namespace LateRoomsCheckoutKata.Checkout.Domain.Products
{
    /// <summary>
    /// An interface containing the functionality for discounting a specific <see cref="Product"/> stock keeping unit.
    /// </summary>
    public interface IProductDiscountRule
    {
        int QuantityToDiscount { get; }

        int CalculateDiscount(int numberOfScannedProducts, int unitPrice);
    }
}