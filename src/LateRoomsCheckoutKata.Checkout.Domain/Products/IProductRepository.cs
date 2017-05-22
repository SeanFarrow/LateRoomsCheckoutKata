namespace LateRoomsCheckoutKata.Checkout.Domain.Products
{
    /// <summary>
    ///An interface for a repository used to deal with products the supermarket sells.
    /// </summary>
    public interface IProductRepository
    {
        /// <summary>
        /// Find a product sold by the supermarket.
        /// </summary>
        /// <param name="SKU">The stock keeping unit of the <see cref="Product"/> to find.</param>
        /// <returns>The <see cref="Product"/> if found.</returns>
        ///<exception cref="ArgumentNullException">The <paramref name="sku"/> passed in is <c>null</c>.</exception>
        ///<exception cref="ArgumentException">The <paramref name="sku"/> passed in is an empty string.></exception>
        /// <exception cref="ProductWithSKUNotFoundException"><paramref name="sku"/> The passed in stock keeping unit is no longer sold by the supermarket.</exception>
        Product FindProductBySKU(string SKU);
    }
}