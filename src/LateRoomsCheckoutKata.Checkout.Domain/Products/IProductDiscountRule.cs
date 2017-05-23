namespace LateRoomsCheckoutKata.Checkout.Domain.Products
{
    public interface IProductDiscountRule
    {
        uint QuantityToDiscount { get; }
        int CalculateDiscount(uint t1, uint t2);
    }
}
