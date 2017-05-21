namespace LateRoomsCheckoutKata.Checkout.Domain.Products
{
    /// <summary>
    ///An interface for a repository used to deal with products the supermarket sells.
    /// </summary>
    public interface IProductRepository
    {
        Product FindProductBySKU(string SKU);
    }
}