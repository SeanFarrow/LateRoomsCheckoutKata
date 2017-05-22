using System;
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
    }
}
