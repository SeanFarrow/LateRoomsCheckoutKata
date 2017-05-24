namespace LateRoomsCheckoutKata.Checkout.Domain.Products
{
    /// <summary>
    ///An interface for a repository used to interact with <see cref="IProductDiscountRule"/> instances.
    /// </summary>
    public interface IProductDiscountRuleRepository
    {
        IProductDiscountRule GetDiscountRuleForSKU(string sku);
    }
}